using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirtyPlateCounter : Furniture {

    [SerializeField] private Transform dirtyPlateStackPrefab;
    private Transform dirtyPlateStack;

    public override Item GetItemOnTop() {
        if (itemSlot.childCount == 0) return null;

        return itemSlot.GetComponentInChildren<StackedDirtyPlate>();
    }

    public override bool CanSetItemOnTop(Item droppedItem) {
        return false;
    }

    public override void SetItemOnTop(Item droppedItem) {
        Plate droppedDirtyPlate = droppedItem as Plate;
        if (itemSlot.childCount == 0) {
            dirtyPlateStack = Instantiate(dirtyPlateStackPrefab, itemSlot);
            droppedDirtyPlate.transform.SetParent(dirtyPlateStack);
            droppedDirtyPlate.transform.localPosition = Vector3.zero;

            itemOnTop = dirtyPlateStack.GetComponent<StackedDirtyPlate>();
        }
        else {
            dirtyPlateStack = itemSlot.GetComponentInChildren<StackedDirtyPlate>().transform;
            droppedDirtyPlate.transform.SetParent(dirtyPlateStack);
            droppedDirtyPlate.transform.localPosition = Vector3.zero;
        }

        return;
    }
}
