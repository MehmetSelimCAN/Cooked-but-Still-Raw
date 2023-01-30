using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotStove : Furniture {

    public override bool CanSetItemOnTop(Item droppedItem) {
        if (itemOnTop == null) {
            //Üstü boþ ve Pot býrakmaya çalýþýyorsak
            if (droppedItem is Pot) {
                return true;
            }

            //Üstü boþ ve Pot dýþýnda bir þey býrakmaya çalýþýyorsak
            return false;
        }
        else {
            //Üstünde Pot var ve bir þey býrakmaya çalýþýyorsak
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
