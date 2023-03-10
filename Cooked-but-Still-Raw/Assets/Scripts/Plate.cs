using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Plate : Dish {

    [SerializeField] private List<Recipe> allPossibleRecipes = new List<Recipe>();
    [SerializeField] private bool isDirty = false;
    public bool IsDirty { get { return isDirty; } }

    private int washingProcessCount = 5;
    public int WashingProcessCount { get { return washingProcessCount; } }

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

        for (int i = 0; i < allPossibleRecipes.Count; i++)
        {
            Recipe temp = new Recipe();
            temp.recipeName = allPossibleRecipes[i].recipeName;
            temp.recipePrepareTime = allPossibleRecipes[i].recipePrepareTime;
            temp.isAvailableOnThisLevel = allPossibleRecipes[i].isAvailableOnThisLevel;
            temp.ingredientInformations = new List<IngredientInformation>(allPossibleRecipes[i].ingredientInformations);
            for (int j = 0; j < allPossibleRecipes[i].ingredientInformations.Count; j++)
            {
                if (allPossibleRecipes[i].ingredientInformations[j].ingredientType == addedIngredient.IngredientType)
                {
                    temp.ingredientInformations.Remove(temp.ingredientInformations[j]);
                    if (temp.ingredientInformations.Count == 0)
                    {
                        Debug.Log(temp.recipeName + " is ready to serve.");
                    }
                }
            }
            allPossibleRecipes[i] = temp;
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
        //Mesh deðiþtir.
        isDirty = true;
    }

    public void SetClean() {
        //Mesh deðiþtir.
        isDirty = false;
    }
}
