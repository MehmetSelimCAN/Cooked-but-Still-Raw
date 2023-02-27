using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pot : Dish {

    private float soupSnappingOffSet;
    [SerializeField] private Transform ingredientSlot;
    [SerializeField] private List<Ingredient> currentIngredients = new List<Ingredient>();

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
        foreach (Transform ingredient in ingredientSlot) {
            Destroy(ingredient.gameObject);
        }

        currentIngredientQuantity = 0;
        Debug.Log("Clear Pot");
    }
}