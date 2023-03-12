using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CuttingBoard : DefaultCounter {

    private int currentCuttingProcess = 0;
    [SerializeField] private Animator cuttingKnifeAnimator;
    [SerializeField] private Transform progressBarUI;
    [SerializeField] private Image progressBarFill;

    public override Item GetItemOnTop() {
        currentCuttingProcess = 0;
        HideProgressBarUI();
        return itemOnTop;
    }

    public override void SetItemOnTop(Item droppedItem) {
        base.SetItemOnTop(droppedItem);

        currentCuttingProcess = 0;
        HideProgressBarUI();
    }

    public override bool Interact() {
        if (itemOnTop == null) return false;
        if (!(itemOnTop is Ingredient)) return false;

        Ingredient ingredientOnTop = itemOnTop as Ingredient;

        if (ingredientOnTop.IngredientStatus != IngredientStatus.Raw) return false;
        if (!(ingredientOnTop is ICuttable)) return false;
        ICuttable cuttableOnTop = ingredientOnTop as ICuttable;

        if (!progressBarUI.gameObject.activeInHierarchy) {
            ShowProgressBarUI();
        }

        //Process each cut and show the progress on the UI.
        currentCuttingProcess++;
        AudioManager.Instance.PlayCuttingAudio();
        progressBarFill.fillAmount = currentCuttingProcess / cuttableOnTop.CuttingProcessCount;

        if (currentCuttingProcess >= cuttableOnTop.CuttingProcessCount) {
            cuttableOnTop.SlicedUp();
            HideProgressBarUI();
        }

        return true;
    }

    public override void InteractAnimation(PlayerController player) {
        player.PlayerAnimator.SetTrigger("Cutting");
        cuttingKnifeAnimator.SetTrigger("Cutting");
    }

    private void ShowProgressBarUI() {
        progressBarUI.gameObject.SetActive(true);
    }

    private void HideProgressBarUI() {
        progressBarUI.gameObject.SetActive(false);
    }
}
