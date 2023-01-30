using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotStove : Furniture {

    public override bool CanSetItemOnTop(Item droppedItem) {
        if (itemOnTop == null) {
            //�st� bo� ve Pot b�rakmaya �al���yorsak
            if (droppedItem is Pot) {
                return true;
            }

            //�st� bo� ve Pot d���nda bir �ey b�rakmaya �al���yorsak
            return false;
        }
        else {
            //�st�nde Pot var ve bir �ey b�rakmaya �al���yorsak
            return (itemOnTop as Pot).CanAddIngridient(droppedItem);
            //return (itemOnTop as Pot).AcceptIngridients(droppedItem);
        }
    }

    public override void SetItemOnTop(Item droppedItem) {
        droppedItem.transform.SetParent(itemSlot);
        droppedItem.transform.localPosition = Vector3.zero;
        itemOnTop = droppedItem;
    }
}
