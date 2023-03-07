using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Sink : Furniture {

    private int currentWashingProcess = 0;
    [SerializeField] private Transform cleanPlateSlot;
    [SerializeField] private Transform progressBarUI;
    [SerializeField] private Image progressBarFill;

    public override Item GetItemOnTop() {
        return cleanPlateSlot.GetComponentInChildren<Plate>();
    }

    public override bool CanSetItemOnTop(Item droppedItem) {
        if (droppedItem is StackedDirtyPlate) {
            return true;
        }

        return false;


        ////Sadece tabak býrakýlabilecek.
        //if (droppedItem is Plate) {
        //    Plate droppedPlate = droppedItem as Plate;
        //    //Ayrýca tabak kirli olacak.
        //    if (droppedPlate.IsDirty) {
        //        return true;
        //    }
        //}

        //return false;
    }

    public override void SetItemOnTop(Item droppedItem) {
        //itemOnTop = droppedItem;
        HandleDroppedItemPosition(droppedItem);

        currentWashingProcess = 0;
        HideProgressBarUI();
    }

    public override void Interact() {
        if (itemSlot.childCount == 0) return;
        StackedDirtyPlate stackedDirtyPlates = itemSlot.GetComponentInChildren<StackedDirtyPlate>();

        Plate dirtyPlate = stackedDirtyPlates.transform.GetChild(0).GetComponent<Plate>();

        if (!progressBarUI.gameObject.activeInHierarchy) {
            ShowProgressBarUI();
        }

        currentWashingProcess++;
        progressBarFill.fillAmount = (float)currentWashingProcess / dirtyPlate.WashingProcessCount;

        if (currentWashingProcess >= 5) {
            dirtyPlate.CleanedUp();
            dirtyPlate.transform.SetParent(cleanPlateSlot);
            dirtyPlate.transform.localPosition = Vector3.zero;
            currentWashingProcess = 0;
            HideProgressBarUI();

            if (stackedDirtyPlates.transform.childCount == 0) {
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
