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
