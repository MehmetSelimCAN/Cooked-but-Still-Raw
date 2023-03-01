using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChoppingBoard : CounterTop {

    private int cuttingProcess = 0;
    [SerializeField] private Transform progressBarUI;
    [SerializeField] private Image progressBarFill;

    public override void SetItemOnTop(Item droppedItem) {
        if (itemOnTop == null) {
            droppedItem.transform.SetParent(itemSlot);
            droppedItem.transform.localPosition = Vector3.zero;
            droppedItem.transform.localRotation = Quaternion.identity;
            itemOnTop = droppedItem;
        }
        else {
            Dish dishOnTop = itemOnTop as Dish;
            Ingredient droppedIngredient = droppedItem as Ingredient;
            dishOnTop.AddIngredient(droppedIngredient);
        }

        cuttingProcess = 0;
        progressBarUI.gameObject.SetActive(false);
    }

    public override void Interact() {
        if (itemOnTop == null) return;
        if (!(itemOnTop is Ingredient)) return;

        Ingredient ingredientOnTop = itemOnTop as Ingredient;

        if (ingredientOnTop.IngredientStatus != IngredientStatus.Raw) return;
        if (!(ingredientOnTop is ICuttable)) return;

        ICuttable cuttableOnTop = ingredientOnTop as ICuttable;

        if (cuttingProcess == 0) {
            progressBarUI.gameObject.SetActive(true);
        }

        cuttingProcess++;
        progressBarFill.fillAmount = cuttingProcess / cuttableOnTop.CuttingProcessCountMax;
        if (cuttingProcess >= cuttableOnTop.CuttingProcessCountMax) {
            Debug.Log("Sliced");
            cuttableOnTop.SlicedUp();
            progressBarUI.gameObject.SetActive(false);
        }
    }
}
