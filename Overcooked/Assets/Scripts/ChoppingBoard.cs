using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChoppingBoard : Furniture {

    public override Item GetItemOnTop() {
        Item tempItem = itemOnTop;
        itemOnTop = null;
        return tempItem;
    }

    public override bool TrySetItemOnTop(Item droppedItem) {
        if (itemOnTop != null) return false;
        if (droppedItem is Dish) return false;

        droppedItem.transform.SetParent(itemSlot);
        droppedItem.transform.localPosition = Vector3.zero;
        itemOnTop = droppedItem;
        return true;
    }
}
