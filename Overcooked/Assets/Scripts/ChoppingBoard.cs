using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChoppingBoard : Furniture {

    public override bool CanSetItemOnTop(Item droppedItem) {
        if (itemOnTop != null) return false;
        if (droppedItem is Dish) return false;

        return true;
    }

    public override void SetItemOnTop(Item droppedItem) {
        droppedItem.transform.SetParent(itemSlot);
        droppedItem.transform.localPosition = Vector3.zero;
        itemOnTop = droppedItem;
    }
}
