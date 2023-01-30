using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotStove : Furniture {

    public override Item GetItemOnTop() {
        Item tempItem = itemOnTop;
        itemOnTop = null;
        return tempItem;
    }

    public override bool TrySetItemOnTop(Item droppedItem) {
        if (itemOnTop == null && droppedItem is Pot) {
            //Pot b�rakmaya �al���yorsak
            droppedItem.transform.SetParent(itemSlot);
            droppedItem.transform.localPosition = Vector3.zero;
            itemOnTop = droppedItem;
            return true;
        }
        else {
            //soup malzemesi b�rakmaya �al���yorsak
            return (itemOnTop as Pot).AcceptIngridients(droppedItem);
        }
    }
}
