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
                //Üstü boþ ve Pan býrakmaya çalýþýyorsak
            if (droppedItem is Pan) {
                droppedItem.transform.SetParent(itemSlot);
                droppedItem.transform.localPosition = Vector3.zero;
                itemOnTop = droppedItem;
                return true;
            }

            //Üstü boþ ve Pan dýþýnda bir Dish býrakmaya çalýþýyorsak
            return false;
        }
        else {
            //Üstünde Pan var ve Piþecek malzeme býrakmaya çalýþýyorsak
            return (itemOnTop as Pan).AcceptIngridients(droppedItem);
        }
    }
}
