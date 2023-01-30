using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CounterTop : Furniture {

    public override bool CanSetItemOnTop(Item droppedItem) {
        if (ItemOnTop == null) return true;

        if (itemOnTop != null) {
            if (itemOnTop is Dish) {
                return (itemOnTop as Dish).CanAddIngridient(droppedItem);
                //return (itemOnTop as Dish).AcceptIngridients(droppedItem);
            }
        }
        //else {
        //    droppedItem.transform.SetParent(itemSlot);
        //    droppedItem.transform.localPosition = Vector3.zero;
        //    itemOnTop = droppedItem;
        //    return true;
        //}

        return false;
    }

    public override void SetItemOnTop(Item droppedItem) {
        droppedItem.transform.SetParent(itemSlot);
        droppedItem.transform.localPosition = Vector3.zero;
        droppedItem.transform.localRotation = Quaternion.identity;
        itemOnTop = droppedItem;
    }
}
