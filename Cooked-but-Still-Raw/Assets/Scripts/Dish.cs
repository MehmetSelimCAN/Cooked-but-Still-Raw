using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dish : Item {

    [SerializeField] protected Transform ingredientSlot;
    protected int ingredientCapacity;
    public bool HasAnyIngredientOnTop { get { return currentIngredientQuantity > 0; } }
    protected int currentIngredientQuantity = 0;
    protected List<Ingredient> currentIngredients = new List<Ingredient>();

    public virtual bool CanAddIngredient(Item droppedItem) { return false; }
    public virtual void AddIngredient(Ingredient droppedIngredient) { }
    public virtual void ClearCurrentIngredients() { }
    public virtual void ClearTimers() { }
}
