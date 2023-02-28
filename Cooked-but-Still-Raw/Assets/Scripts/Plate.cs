using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        droppedIngredient.transform.SetParent(ingredientSlot);
        droppedIngredient.transform.localPosition = Vector3.zero;
        currentIngredientQuantity++;
        currentIngredientTypes.Add(droppedIngredient.IngredientType);
        currentIngredients.Add(droppedIngredient);

    }

    public override void ClearCurrentIngredients() {
        allPossibleRecipes = new List<Recipe>(RecipeManager.Instance.Recipes);
        Debug.Log("MSC " + allPossibleRecipes.Count);
        currentIngredientQuantity = 0;
        currentIngredientTypes.Clear();
        Debug.Log("Clear Plate");
    }

    public override void TransferIngredients(Dish dishToBeTransferred) {
        if (dishToBeTransferred.HasAnyIngredientOnTop) {
            if (!isFull) {
                bool ingredientsMatched = true;
                foreach (Ingredient ingredientInDishToBeTransferred in dishToBeTransferred.CurrentIngredients) {
                    ingredientsMatched = CanAddIngredient(ingredientInDishToBeTransferred);
                    if (!ingredientsMatched) {
                        break;
                    }
                }

                if (ingredientsMatched) {
                    foreach (Ingredient ingredientInDishToBeTransferred in dishToBeTransferred.CurrentIngredients) {
                        AddIngredient(ingredientInDishToBeTransferred);
                    }

                    Debug.Log("MSC clear ");
                    dishToBeTransferred.ClearCurrentIngredients();
                }
                else {
                    Debug.Log("Recipe uyuþmuyor, transfer gerçekleþtirelemedi");
                    return;
                }
            }
        }

        else {
            bool ingredientsMatched = true;
            foreach (Ingredient ingredientInDish in currentIngredients) {
                ingredientsMatched = dishToBeTransferred.CanAddIngredient(ingredientInDish);
                if (!ingredientsMatched) {
                    break;
                }
            }

            if (ingredientsMatched) {
                foreach (Ingredient ingredientInDish in currentIngredients) {
                    dishToBeTransferred.AddIngredient(ingredientInDish);
                }

                ClearCurrentIngredients();
            }

        }
    }

    private void UpdatePossibleRecipes(Ingredient addedIngredient) {
        List<Recipe> recipesToBeDeleted = new List<Recipe>();

        foreach (Recipe recipe in allPossibleRecipes) {
            bool isRecipeContainsAddedIngredient = false;
            foreach (IngredientInformation ingredientInformation in recipe.ingredientInformations) {
                if (ingredientInformation.ingredientType == addedIngredient.IngredientType &&
                    ingredientInformation.ingredientStatus == addedIngredient.IngredientStatus) {
                        isRecipeContainsAddedIngredient = true;
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
