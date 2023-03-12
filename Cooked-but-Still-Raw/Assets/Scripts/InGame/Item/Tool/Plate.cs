using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Plate : Dish {

    [SerializeField] private List<Recipe> allPossibleRecipes = new List<Recipe>();
    private bool isDirty = false;
    public bool IsDirty { get { return isDirty; } }

    private int washingProcessCount = 5;
    public int WashingProcessCount { get { return washingProcessCount; } }

    public Recipe readyToServeRecipe;

    private void Awake() {
        ingredientCapacity = 10;
    }

    private void Start() {
        allPossibleRecipes = new List<Recipe>(RecipeManager.Instance.Recipes);
    }

    //Returns a boolean showing that whether an ingredient can be added into the plate or not.
    public override bool CanAddIngredient(Item droppedItem) {
        if (!(droppedItem is Ingredient)) return false;
        Ingredient droppedIngredient = droppedItem as Ingredient;

        bool isThereRecipeWithCurrentIngredients = false;
        foreach (Recipe recipe in allPossibleRecipes) {
            foreach (IngredientInformation ingredientInformation in recipe.ingredientInformations) {
                if (ingredientInformation.ingredientType == droppedIngredient.IngredientType &&
                    ingredientInformation.ingredientStatus == droppedIngredient.IngredientStatus) {
                        isThereRecipeWithCurrentIngredients = true;
                        UpdatePossibleRecipes(droppedIngredient);

                        return isThereRecipeWithCurrentIngredients;
                }
            }
        }

        return isThereRecipeWithCurrentIngredients;
    }

    //Stores the possible recipe list that can be achieved from current ingredients.
    private void UpdatePossibleRecipes(Ingredient addedIngredient) {
        List<Recipe> recipesToBeDeleted = new List<Recipe>();

        foreach (Recipe recipe in allPossibleRecipes) {
            bool isRecipeContainsAddedIngredient = false;
            foreach (IngredientInformation ingredientInformation in recipe.ingredientInformations) {
                if (ingredientInformation.ingredientType == addedIngredient.IngredientType &&
                    ingredientInformation.ingredientStatus == addedIngredient.IngredientStatus) {
                    isRecipeContainsAddedIngredient = true;
                    break;
                }
            }

            if (!isRecipeContainsAddedIngredient) {
                recipesToBeDeleted.Add(recipe);
            }
        }

        foreach (Recipe recipeToBeDeleted in recipesToBeDeleted) {
            allPossibleRecipes.Remove(recipeToBeDeleted);
        }

        for (int i = 0; i < allPossibleRecipes.Count; i++) {
            Recipe copyRecipe = new Recipe();
            copyRecipe.recipeName = allPossibleRecipes[i].recipeName;
            copyRecipe.recipePrepareTime = allPossibleRecipes[i].recipePrepareTime;
            copyRecipe.isAvailableOnThisLevel = allPossibleRecipes[i].isAvailableOnThisLevel;
            copyRecipe.ingredientInformations = new List<IngredientInformation>(allPossibleRecipes[i].ingredientInformations);
            for (int j = 0; j < allPossibleRecipes[i].ingredientInformations.Count; j++) {
                if (allPossibleRecipes[i].ingredientInformations[j].ingredientType == addedIngredient.IngredientType &&
                    allPossibleRecipes[i].ingredientInformations[j].ingredientStatus == addedIngredient.IngredientStatus) {
                    copyRecipe.ingredientInformations.Remove(copyRecipe.ingredientInformations[j]);
                    if (copyRecipe.ingredientInformations.Count == 0) {
                        readyToServeRecipe = copyRecipe;
                    }
                    break;
                }
            }
            allPossibleRecipes[i] = copyRecipe;
        }
    }

    //Adds the ingredient to the plate and updates itself accordingly.
    public override void AddIngredient(Ingredient droppedIngredient) {
        HandleDroppedIngredientPosition(droppedIngredient);

        CurrentIngredientQuantity++;
        currentIngredients.Add(droppedIngredient);

        AddIngredientUI(droppedIngredient);
    }

    //Reset Plate.
    public override void ClearCurrentIngredients() {
        allPossibleRecipes = new List<Recipe>(RecipeManager.Instance.Recipes);
        CurrentIngredientQuantity = 0;
        currentIngredients.Clear();

        ClearIngredientUI();
    }

    public void SetDirty() {
        isDirty = true;
    }

    public void SetClean() {
        isDirty = false;
    }
}
