using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dish : Item {

    protected int ingridientCapacity;
    protected int currentIngridientQuantity = 0;

    public virtual bool CanAddIngridient(Item droppedItem) { return false; }
    public virtual void AddIngridient(Ingridient droppedIngridient) { }
    public virtual void ClearCurrentIngridients() { }
}
