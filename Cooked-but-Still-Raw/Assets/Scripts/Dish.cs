using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dish : Item {

    [SerializeField] protected Transform ingredientSlot;

    protected int ingredientCapacity;
    private int currentIngredientQuantity = 0;
    public int CurrentIngredientQuantity { get { return currentIngredientQuantity; } set { currentIngredientQuantity = Mathf.Clamp(value, 0, 6); } }
    public bool HasAnyIngredientOnTop { get { return currentIngredientQuantity > 0; } }
    public bool isFull { get { return currentIngredientQuantity >= ingredientCapacity; } }

    protected List<Ingredient> currentIngredients = new List<Ingredient>();
    public List<Ingredient> CurrentIngredients { get { return currentIngredients; } }

    public virtual bool CanAddIngredient(Item droppedItem) { return false; }
    public virtual void AddIngredient(Ingredient droppedIngredient) { }
    public virtual void AddIngredientUI(Ingredient droppedIngredient) { }

    public virtual void TryToTransferIngredients(Dish dishToBeTransferred) {
        bool ingredientsMatched = CheckIngredientMatches(dishToBeTransferred);
        if (ingredientsMatched) {
            TransferIngredients(dishToBeTransferred);
        }
        else {
            Debug.Log("Recipeler uyuþmuyor");
        }
    }

    public virtual bool CheckIngredientMatches(Dish dishToBeTransferred) {
        bool ingredientsMatched = true;
        foreach (Ingredient ingredientInDish in CurrentIngredients) {
            ingredientsMatched = dishToBeTransferred.CanAddIngredient(ingredientInDish);
            if (!ingredientsMatched) {
                break;
            }
        }

        return ingredientsMatched;
    }

    public virtual void TransferIngredients(Dish dishToBeTransferred) {
        foreach (Ingredient ingredientInDish in CurrentIngredients) {
            dishToBeTransferred.AddIngredient(ingredientInDish);
        }

        ClearCurrentIngredients();
    }

    public virtual void ClearCurrentIngredients() { }
    public virtual void ClearTimers() { }

    public virtual void ClearIngredientUI() {
        foreach (Transform ingredientUI in ingredientUI_Icons) {
            ingredientUI.gameObject.SetActive(false);
        }

        ingredientUI_Icons.gameObject.SetActive(false);
    }

    public virtual void HandleDroppedIngredientPosition(Ingredient droppedIngredient) {
        droppedIngredient.transform.SetParent(ingredientSlot);
        droppedIngredient.transform.localPosition = Vector3.zero;
    }

    public override void ThrowInTheGarbage() {
        foreach (Transform ingredient in ingredientSlot) {
            Destroy(ingredient.gameObject);
        }

        ClearCurrentIngredients();
        ClearTimers();
    }
}
