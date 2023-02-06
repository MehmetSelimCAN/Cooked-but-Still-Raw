using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChoppingBoard : Furniture {

    [SerializeField] private List<IngridientType> cuttableIngridientTypes;
    private int cuttingProcess = 0;

    public override bool CanSetItemOnTop(Item droppedItem) {
        if (itemOnTop != null) return false;
        if (droppedItem is Dish) return false;

        if (droppedItem is Ingridient) {
            Ingridient droppedIngridient = droppedItem as Ingridient;
            if (droppedIngridient.IngridientType == IngridientType.Bread) {
                return false;
            }
        }

        return true;
    }

    public override void SetItemOnTop(Item droppedItem) {
        droppedItem.transform.SetParent(itemSlot);
        droppedItem.transform.localPosition = Vector3.zero;
        itemOnTop = droppedItem;

        cuttingProcess = 0;
    }

    public override void Interact() {
        if (itemOnTop == null) return;
        if (!(itemOnTop is Ingridient)) return;

        Ingridient ingridientOnTop = itemOnTop as Ingridient;

        if (ingridientOnTop.IngridientStatus != IngridientStatus.Raw) return;
        if (!(cuttableIngridientTypes.Contains(ingridientOnTop.IngridientType))) return;

        cuttingProcess++;
        if (cuttingProcess >= ingridientOnTop.ProcessCountMax) {
            //Change mesh to sliced one.
            ingridientOnTop.ChangeStatus(IngridientStatus.Processed);
            Debug.Log("Sliced");
        }
    }
}
