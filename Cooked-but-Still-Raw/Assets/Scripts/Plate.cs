using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Plate : Dish {

    [SerializeField] private List<IngredientType> currentIngredientTypes = new List<IngredientType>();
    [SerializeField] private List<Recipe> allPossibleRecipes = new List<Recipe>();

    private void Awake() {
        ingredientCapacity = 10;
    }

    private void Start() {
        allPossibleRecipes = new List<Recipe>(RecipeManager.Instance.Recipes);
    }

    public override bool CanAddIngredient(Item droppedItem) {
        if (!(droppedItem is Ingredient)) return false;

        Ingredient droppedIngredient = droppedItem as Ingredient;

        if (currentIngredientTypes.Contains(droppedIngredient.IngredientType)) return false;

        bool isThereRecipeWithCurrentIngredients = false;
        foreach (Recipe recipe in allPossibleRecipes) {
            foreach (IngredientInformation ingredientInformation in recipe.ingredientInformations) {
                if (ingredientInformation.ingredientType == droppedIngredient.IngredientType) {
                    if (ingredientInformation.ingredientStatus == droppedIngredient.IngredientStatus) {
                        UpdatePossibleRecipes(droppedIngredient);
                        isThereRecipeWithCurrentIngredients = true;

                        return isThereRecipeWithCurrentIngredients;
                    }
                }
            }
        }

        return isThereRecipeWithCurrentIngredients;
    }

    public override void AddIngredient(Ingredient droppedIngredient) {
        HandleDroppedIngredientPosition(droppedIngredient);

        CurrentIngredientQuantity++;
        currentIngredientTypes.Add(droppedIngredient.IngredientType);
        currentIngredients.Add(droppedIngredient);

        AddIngredientUI(droppedIngredient);
    }

    public override void AddIngredientUI(Ingredient droppedIngredient) {
        droppedIngredient.HideUI();

        if (!ingredientUI_Icons.gameObject.activeInHierarchy) {
            ingredientUI_Icons.gameObject.SetActive(true);
        }

        ingredientUI_Icons.transform.GetChild(CurrentIngredientQuantity - 1).gameObject.SetActive(true);
        ingredientUI_Icons.transform.GetChild(CurrentIngredientQuantity - 1).GetComponent<Image>().sprite = droppedIngredient.IngredientSprite;
    }

    public override void ClearCurrentIngredients() {
        allPossibleRecipes = new List<Recipe>(RecipeManager.Instance.Recipes);
        CurrentIngredientQuantity = 0;
        currentIngredientTypes.Clear();
        currentIngredients.Clear();

        ClearIngredientUI();
    }

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
    }
}
