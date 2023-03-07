using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Sink : Furniture {

    private int currentWashingProcess = 0;
    [SerializeField] private Transform cleanPlateSlot;
    [SerializeField] private Transform progressBarUI;
    [SerializeField] private Image progressBarFill;

    //Access the item currently on the furniture.
    public override Item GetItemOnTop() {
        //Return the plate on top of the clean plate stack.
        return cleanPlateSlot.GetComponentInChildren<Plate>();
    }

    //Responsible for checking if the furniture can accept a new item.
    public override bool CanSetItemOnTop(Item droppedItem) {
        //Nothing can be placed on top except dirty plates.
        if (droppedItem is DirtyPlateStack) return true;

        return false;
    }

    //Responsible for placing a new item on top of the furniture.
    public override void SetItemOnTop(Item droppedItem) {
        HandleDroppedItemPosition(droppedItem);

        //Prepare UI for dropped item.
        currentWashingProcess = 0;
        HideProgressBarUI();
    }

    //Responsible for handling interactions with the furniture.
    public override void Interact() {
        //If there are no dirty plates, don't interact.
        if (itemSlot.childCount == 0) return;
        //If there is at least one dirty plate, the script gets a reference
        //to the DirtyPlateStack component, which holds the stack of dirty plates.
        //It then gets a reference to the first Plate object in the stack.

        DirtyPlateStack stackedDirtyPlates = itemSlot.GetComponentInChildren<DirtyPlateStack>();

        Plate dirtyPlate = stackedDirtyPlates.transform.GetComponentInChildren<Plate>();

        if (!progressBarUI.gameObject.activeInHierarchy) {
            ShowProgressBarUI();
        }

        currentWashingProcess++;
        progressBarFill.fillAmount = (float)currentWashingProcess / dirtyPlate.WashingProcessCount;

        if (currentWashingProcess >= dirtyPlate.WashingProcessCount) {
            dirtyPlate.CleanedUp();
            dirtyPlate.transform.SetParent(cleanPlateSlot);
            dirtyPlate.transform.localPosition = Vector3.zero;
            currentWashingProcess = 0;
            HideProgressBarUI();

            if (stackedDirtyPlates.transform.childCount == 1) {
                Destroy(stackedDirtyPlates.gameObject);
            }
        }
    }

    private void ShowProgressBarUI() {
        progressBarUI.gameObject.SetActive(true);
    }

    private void HideProgressBarUI() {
        progressBarUI.gameObject.SetActive(false);
    }
}
