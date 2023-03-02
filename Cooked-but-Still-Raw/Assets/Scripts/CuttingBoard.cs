using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CuttingBoard : CounterTop {

    private int currentCuttingProcess = 0;
    [SerializeField] private Transform progressBarUI;
    [SerializeField] private Image progressBarFill;

    public override void SetItemOnTop(Item droppedItem) {
        base.SetItemOnTop(droppedItem);

        currentCuttingProcess = 0;
        progressBarUI.gameObject.SetActive(false);
    }

    public override void Interact() {
        if (itemOnTop == null) return;
        if (!(itemOnTop is Ingredient)) return;

        Ingredient ingredientOnTop = itemOnTop as Ingredient;

        if (ingredientOnTop.IngredientStatus != IngredientStatus.Raw) return;
        if (!(ingredientOnTop is ICuttable)) return;

        ICuttable cuttableOnTop = ingredientOnTop as ICuttable;

        if (!progressBarUI.gameObject.activeInHierarchy) {
            ShowProgressBarUI();
        }

        currentCuttingProcess++;
        progressBarFill.fillAmount = currentCuttingProcess / cuttableOnTop.CuttingProcessCount;

        if (currentCuttingProcess >= cuttableOnTop.CuttingProcessCount) {
            cuttableOnTop.SlicedUp();
            HideProgressBarUI();
        }
    }

    private void ShowProgressBarUI() {
        progressBarUI.gameObject.SetActive(true);
    }

    private void HideProgressBarUI() {
        progressBarUI.gameObject.SetActive(false);
    }
}
