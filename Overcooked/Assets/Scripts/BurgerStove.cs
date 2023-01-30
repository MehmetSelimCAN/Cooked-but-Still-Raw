using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BurgerStove : Furniture {

    public override bool CanSetItemOnTop(Item droppedItem) {
        if (itemOnTop == null) {
            //�st� bo� ve Pan b�rakmaya �al���yorsak
            if (droppedItem is Pan) {
                return true;
            }

            //�st� bo� ve Pan d���nda bir �ey b�rakmaya �al���yorsak
            return false;
        }
        else {
            //�st�nde Pan var ve bir �ey b�rakmaya �al���yorsak
            return (ItemOnTop as Pan).CanAddIngridient(droppedItem);
            //return (itemOnTop as Pan).AcceptIngridients(droppedItem);
        }
    }

    public override void SetItemOnTop(Item droppedItem) {
        droppedItem.transform.SetParent(itemSlot);
        droppedItem.transform.localPosition = Vector3.zero;
        itemOnTop = droppedItem;
    }
}
