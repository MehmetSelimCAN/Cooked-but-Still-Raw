using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CounterTop : Furniture {

    public override bool CanSetItemOnTop(Item droppedItem) {
        if (itemOnTop == null) return true;

        if (itemOnTop != null) {
            if (itemOnTop is Dish) {
                Dish dishOnTop = itemOnTop as Dish;
                return dishOnTop.CanAddIngredient(droppedItem);
            }
        }

        return false;
    }

    public override void SetItemOnTop(Item droppedItem) {
        if (itemOnTop == null) {
            HandleDroppedItemPosition(droppedItem);
            itemOnTop = droppedItem;
        }
        else {
            Dish dishOnTop = itemOnTop as Dish;
            Ingredient droppedIngredient = droppedItem as Ingredient;
            dishOnTop.AddIngredient(droppedIngredient);
        }
    }
}
