using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BurgerStove : Furniture {

    public override Item GetItemOnTop() {
        Item tempItem = itemOnTop;
        itemOnTop = null;
        return tempItem;
    }

    public override bool TrySetItemOnTop(Item droppedItem) {
        if (itemOnTop == null) {
                //�st� bo� ve Pan b�rakmaya �al���yorsak
            if (droppedItem is Pan) {
                droppedItem.transform.SetParent(itemSlot);
                droppedItem.transform.localPosition = Vector3.zero;
                itemOnTop = droppedItem;
                return true;
            }

            //�st� bo� ve Pan d���nda bir Dish b�rakmaya �al���yorsak
            return false;
        }
        else {
            //�st�nde Pan var ve Pi�ecek malzeme b�rakmaya �al���yorsak
            return (itemOnTop as Pan).AcceptIngridients(droppedItem);
        }
    }
}
