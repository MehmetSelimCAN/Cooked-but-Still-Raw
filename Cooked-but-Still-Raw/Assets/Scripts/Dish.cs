using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dish : Item {

    [SerializeField] protected Transform ingredientSlot;
    [SerializeField] protected Transform ingredientUICanvasArea;

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
    public virtual void TransferIngredients(Dish dishToBeTransferred) { }
    public virtual void ClearCurrentIngredients() { }
    public virtual void ClearIngredientUI() { }
    public virtual void ClearTimers() { }

    public virtual void ThrowInTheGarbage() {
        foreach (Transform ingredient in ingredientSlot) {
            Destroy(ingredient.gameObject);
        }

        ClearCurrentIngredients();
        ClearTimers();
    }
}
