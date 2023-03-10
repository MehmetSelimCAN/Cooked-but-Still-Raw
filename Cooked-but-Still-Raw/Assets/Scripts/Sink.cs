using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Sink : Furniture {

    [SerializeField] private ParticleSystem washingParticleEffect;

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
    public override bool Interact() {
        //If there are no dirty plates, don't interact.
        if (itemSlot.childCount == 0) return false;

        //If there is any dirty plates in the sink, get the reference of the plate.
        DirtyPlateStack stackedDirtyPlates = itemSlot.GetComponentInChildren<DirtyPlateStack>();
        Plate dirtyPlate = stackedDirtyPlates.transform.GetComponentInChildren<Plate>();

        if (!progressBarUI.gameObject.activeInHierarchy) {
            ShowProgressBarUI();
        }

        //Update the cleaning process of the dirty plate and show it on UI.
        currentWashingProcess++;
        progressBarFill.fillAmount = (float)currentWashingProcess / dirtyPlate.WashingProcessCount;

        //If dirty plate scrubbed enough to get cleaned.
        if (currentWashingProcess >= dirtyPlate.WashingProcessCount) {
            //Restore the plate functionality.
            dirtyPlate.SetClean();

            //Replace the plate on the counter.
            dirtyPlate.transform.SetParent(cleanPlateSlot);
            dirtyPlate.transform.localPosition = Vector3.zero;

            //Prepare sink for next cleaning process.
            currentWashingProcess = 0;
            HideProgressBarUI();

            //If there are no more dirty plates in the sink we don't need the stack object anymore.
            if (stackedDirtyPlates.transform.childCount == 1) {
                Destroy(stackedDirtyPlates.gameObject);
            }
        }

        return true;
    }

    public override void InteractAnimation(PlayerController player) {
        player.PlayerAnimator.SetTrigger("Washing");
        washingParticleEffect.Play();
    }

    //Deactivate cleaning progress bar.
    private void ShowProgressBarUI() {
        progressBarUI.gameObject.SetActive(true);
    }

    //Activate cleaning progress bar.
    private void HideProgressBarUI() {
        progressBarUI.gameObject.SetActive(false);
    }
}
