using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChoppingBoard : CounterTop {

    private int cuttingProcess = 0;

    public override void SetItemOnTop(Item droppedItem) {
        if (ItemOnTop == null) {
            droppedItem.transform.SetParent(itemSlot);
            droppedItem.transform.localPosition = Vector3.zero;
            droppedItem.transform.localRotation = Quaternion.identity;
            itemOnTop = droppedItem;
        }
        else {
            Dish dishOnTop = ItemOnTop as Dish;
            Ingridient droppedIngridient = droppedItem as Ingridient;
            dishOnTop.AddIngridient(droppedIngridient);
        }

        cuttingProcess = 0;
    }

    public override void Interact() {
        if (itemOnTop == null) return;
        if (!(itemOnTop is Ingridient)) return;

        Ingridient ingridientOnTop = itemOnTop as Ingridient;

        if (ingridientOnTop.IngridientStatus != IngridientStatus.Raw) return;
        if (!(ingridientOnTop is ICuttable)) return;

        ICuttable cuttableOnTop = ingridientOnTop as ICuttable;

        cuttingProcess++;
        if (cuttingProcess >= cuttableOnTop.ProcessCountMax) {
            Debug.Log("Sliced");
            cuttableOnTop.SlicedUp();
        }
    }
}
