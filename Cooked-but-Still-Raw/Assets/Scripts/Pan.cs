using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pan : Dish {

    [SerializeField] private Transform ingredientSlot;

    private void Awake() {
        ingredientCapacity = 1;
    }

    public override bool CanAddIngredient(Item droppedItem) {
        if (currentIngredientQuantity >= ingredientCapacity) return false;
        if (!(droppedItem is Ingredient)) return false;

        Ingredient droppedIngredient = droppedItem as Ingredient;

        if (droppedIngredient.IngredientStatus != IngredientStatus.Processed) return false;
        if (!(droppedIngredient is IFryable)) return false;

        return true;
    }

    public override void AddIngredient(Ingredient droppedIngredient) {
        droppedIngredient.transform.SetParent(ingredientSlot);
        droppedIngredient.transform.localPosition = Vector3.zero;
        currentIngredientQuantity++;
    }

    public override void ClearCurrentIngredients() {
        foreach (Transform ingredient in ingredientSlot) {
            Destroy(ingredient.gameObject);
        }

        currentIngredientQuantity = 0;
        Debug.Log("Clear Pan");
    }
}
