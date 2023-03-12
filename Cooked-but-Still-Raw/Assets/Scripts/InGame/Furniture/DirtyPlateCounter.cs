using UnityEngine;

public class DirtyPlateCounter : Furniture {

    [SerializeField] private Transform dirtyPlateStackPrefab;
    private Transform dirtyPlateStackTransform;
    private DirtyPlateStack dirtyPlateStack;

    private Vector3 plateStackOffset = new Vector3(0, 0.3f, 0);

    //Access the item currently on the furniture.
    public override Item GetItemOnTop() {
        //Returns dirty plate stack object.
        //-Dirty plate stack object is just an empty parent object for dirty plates-
        if (itemSlot.childCount == 0) return null;

        DirtyPlateStack dirtyPlateStack = itemSlot.GetComponentInChildren<DirtyPlateStack>();
        return dirtyPlateStack;
    }

    //Responsible for checking if the furniture can accept a new item.
    public override bool CanSetItemOnTop(Item droppedItem) {
        //Nothing can be placed on top.
        return false;
    }

    //Responsible for placing a new item on top of the furniture.
    public override void SetItemOnTop(Item droppedItem) {
        Plate droppedDirtyPlate = droppedItem as Plate;
        //Instantiate the Dirty Plate Stack on top of the DirtyPlateCounter if no stack exists.
        if (itemSlot.childCount == 0) {
            dirtyPlateStackTransform = Instantiate(dirtyPlateStackPrefab, itemSlot);
            itemOnTop = dirtyPlateStackTransform.GetComponent<DirtyPlateStack>();
            HandleDroppedItemPosition(droppedDirtyPlate);
        }
        //If a stack already exists, adds the Dropped Plate to the top of the stack.
        else {
            dirtyPlateStackTransform = itemSlot.GetComponentInChildren<DirtyPlateStack>().transform;
            HandleDroppedItemPosition(droppedDirtyPlate);
        }
        return;
    }

    //Responsible for handling the position of a newly dropped item.
    public override void HandleDroppedItemPosition(Item droppedItem) {
        dirtyPlateStack = itemOnTop as DirtyPlateStack;
        dirtyPlateStack.dirtyPlateCount++;

        droppedItem.transform.SetParent(dirtyPlateStackTransform);
        droppedItem.transform.localPosition = Vector3.zero + plateStackOffset * (dirtyPlateStack.dirtyPlateCount - 1);
    }
}
