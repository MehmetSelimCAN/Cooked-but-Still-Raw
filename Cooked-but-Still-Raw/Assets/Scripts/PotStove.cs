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
            Pot potOnTop = itemOnTop as Pot;
            return potOnTop.CanAddIngridient(droppedItem);
        }
    }

    public override void SetItemOnTop(Item droppedItem) {
        if (ItemOnTop == null) {
            droppedItem.transform.SetParent(itemSlot);
            droppedItem.transform.localPosition = Vector3.zero;
            itemOnTop = droppedItem;
        }
        else {
            Pot potOnTop = ItemOnTop as Pot;
            Ingridient droppedIngridient = droppedItem as Ingridient;
            potOnTop.AddIngridient(droppedIngridient);
        }
    }
}
