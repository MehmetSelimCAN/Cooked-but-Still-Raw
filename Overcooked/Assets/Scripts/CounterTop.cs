using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CounterTop : Furniture {

    public override Item GetItemOnTop() {
        Item tempItem = itemOnTop;
        itemOnTop = null;
        return tempItem;
    }

    public override bool TrySetItemOnTop(Item droppedItem) {
        if (itemOnTop != null) {
            if (itemOnTop is Dish) {
                return (itemOnTop as Dish).AcceptIngridients(droppedItem);
            }
        }
        else {
            droppedItem.transform.SetParent(itemSlot);
            droppedItem.transform.localPosition = Vector3.zero;
            itemOnTop = droppedItem;
            return true;
        }

        return false;
    }
}
