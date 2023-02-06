using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CounterTop : Furniture {

    public override bool CanSetItemOnTop(Item droppedItem) {
        if (ItemOnTop == null) return true;

        if (itemOnTop != null) {
            if (itemOnTop is Dish) {
                Dish dishOnTop = itemOnTop as Dish;
                return dishOnTop.CanAddIngridient(droppedItem);
            }
        }

        return false;
    }

    public override void SetItemOnTop(Item droppedItem) {
        if (ItemOnTop == null) {
            droppedItem.transform.SetParent(itemSlot);
            droppedItem.transform.localPosition = Vector3.zero;
            droppedItem.transform.localRotation = Quaternion.identity;
            itemOnTop = droppedItem;
        }
        else {
            Dish dishOnTop = ItemOnTop as Dish;
            Ingridient droppedIngridient = droppedItem as Ingridient;
            dishOnTop.AddIngridient(droppedIngridient);
        }
    }
}
