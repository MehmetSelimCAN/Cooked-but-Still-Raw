using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dish : Item {

    protected int ingredientCapacity;
    protected int currentIngredientQuantity = 0;

    public virtual bool CanAddIngredient(Item droppedItem) { return false; }
    public virtual void AddIngredient(Ingredient droppedIngredient) { }
    public virtual void ClearCurrentIngredients() { }
}
