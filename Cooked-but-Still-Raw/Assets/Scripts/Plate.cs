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
        droppedIngredient.transform.SetParent(ingredientSlot);
        droppedIngredient.transform.localPosition = Vector3.zero;

        CurrentIngredientQuantity++;
        currentIngredientTypes.Add(droppedIngredient.IngredientType);
        currentIngredients.Add(droppedIngredient);

        AddIngredientUI(droppedIngredient);
    }

    public override void AddIngredientUI(Ingredient droppedIngredient) {
        droppedIngredient.HideUI();

        if (CurrentIngredientQuantity == 1) {
            ingredientUICanvasArea.gameObject.SetActive(true);
        }

        ingredientUICanvasArea.transform.GetChild(CurrentIngredientQuantity - 1).gameObject.SetActive(true);
        ingredientUICanvasArea.transform.GetChild(CurrentIngredientQuantity - 1).GetComponent<Image>().sprite = droppedIngredient.IngredientSprite;
    }

    public override void ClearCurrentIngredients() {
        allPossibleRecipes = new List<Recipe>(RecipeManager.Instance.Recipes);
        CurrentIngredientQuantity = 0;
        currentIngredientTypes.Clear();
        currentIngredients.Clear();
        Debug.Log("Clear Plate");

        ClearIngredientUI();
    }

    public override void ClearIngredientUI() {
        foreach (Transform ingredientUI in ingredientUICanvasArea) {
            ingredientUI.gameObject.SetActive(false);
        }

        ingredientUICanvasArea.gameObject.SetActive(false);
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

                    dishToBeTransferred.ClearCurrentIngredients();
                }
                else {
                    Debug.Log("Recipe uyuşmuyor, transfer gerçekleştirelemedi");
                    return;
                }
            }
            else {
                foreach (Ingredient ingredientInDish in currentIngredients) {
                    dishToBeTransferred.AddIngredient(ingredientInDish);
                }

                ClearCurrentIngredients();
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
