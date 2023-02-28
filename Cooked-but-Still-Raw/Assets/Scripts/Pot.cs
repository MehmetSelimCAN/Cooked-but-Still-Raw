using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pot : Dish {

    private float soupSnappingOffSet;

    private void Awake() {
        soupSnappingOffSet = 20;
        ingredientCapacity = 3;
    }

    public override bool CanAddIngredient(Item droppedItem) {
        if (currentIngredientQuantity >= ingredientCapacity) return false;
        if (!(droppedItem is Ingredient)) return false;

        Ingredient droppedIngredient = droppedItem as Ingredient;

        if (droppedIngredient.IngredientStatus != IngredientStatus.Processed) return false;
        if (!(droppedIngredient is ICookable)) return false;

        return true;
    }

    public override void AddIngredient(Ingredient droppedIngredient) {
        currentIngredients.Add(droppedIngredient);
        currentIngredientQuantity++;

        ICookable cookableIngredient = droppedIngredient as ICookable;
        cookableIngredient.Liquize();
        droppedIngredient.transform.SetParent(ingredientSlot);
        droppedIngredient.transform.localPosition = Vector3.up * soupSnappingOffSet * (currentIngredientQuantity - 1);
        droppedIngredient.transform.localScale = Vector3.one;
    }

    public override void ClearCurrentIngredients() {
        currentIngredientQuantity = 0;
        currentIngredients.Clear();
        Debug.Log("Clear Pot");
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
                    Debug.Log("Recipe uyuþmuyor, transfer gerçekleþtirelemedi");
                    return;
                }
            }
        }

        else {
            foreach (Ingredient ingredientInDish in currentIngredients) {
                dishToBeTransferred.AddIngredient(ingredientInDish);
            }

            ClearCurrentIngredients();
        }
    }
}