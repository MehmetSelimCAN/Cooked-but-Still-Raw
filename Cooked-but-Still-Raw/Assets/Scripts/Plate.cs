using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plate : Dish {

    [SerializeField] private List<IngredientType> currentIngredientTypes = new List<IngredientType>();
    [SerializeField] private List<Recipe> allPossibleRecipes = new List<Recipe>();

    private void Start() {
        allPossibleRecipes = RecipeManager.Instance.Recipes;
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
                        UpdatePossibleRecipes(droppedIngredient.IngredientType);
                        isThereRecipeWithCurrentIngredients = true;

                        return isThereRecipeWithCurrentIngredients;
                    }
                }
            }
        }

        return isThereRecipeWithCurrentIngredients;
    }

    public override void AddIngredient(Ingredient droppedIngredient) {
        currentIngredientTypes.Add(droppedIngredient.IngredientType);

        droppedIngredient.transform.SetParent(ingredientSlot);
        droppedIngredient.transform.localPosition = Vector3.zero;
    }

    public override void ClearCurrentIngredients() {
        foreach (Transform ingredient in ingredientSlot) {
            Destroy(ingredient.gameObject);
        }

        allPossibleRecipes = RecipeManager.Instance.Recipes;
        currentIngredientTypes.Clear();
        Debug.Log("Clear Plate");
    }

    private void UpdatePossibleRecipes(IngredientType addedIngredientType) {
        List<Recipe> recipesToBeDeleted = new List<Recipe>();

        foreach (Recipe recipe in allPossibleRecipes) {
            bool isRecipeContainsAddedIngredientType = false;
            foreach (IngredientInformation ingredientInformation in recipe.ingredientInformations) {
                if (ingredientInformation.ingredientType == addedIngredientType) {
                    isRecipeContainsAddedIngredientType = true;
                }
            }

            if (!isRecipeContainsAddedIngredientType) {
                recipesToBeDeleted.Add(recipe);
            }
        }

        foreach (Recipe recipeToBeDeleted in recipesToBeDeleted) {
            allPossibleRecipes.Remove(recipeToBeDeleted);
        }
    }
}
