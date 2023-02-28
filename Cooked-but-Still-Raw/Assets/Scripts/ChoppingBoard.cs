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
            Ingredient droppedIngredient = droppedItem as Ingredient;
            dishOnTop.AddIngredient(droppedIngredient);
        }

        cuttingProcess = 0;
    }

    public override void Interact() {
        if (itemOnTop == null) return;
        if (!(itemOnTop is Ingredient)) return;

        Ingredient ingredientOnTop = itemOnTop as Ingredient;

        if (ingredientOnTop.IngredientStatus != IngredientStatus.Raw) return;
        if (!(ingredientOnTop is ICuttable)) return;

        ICuttable cuttableOnTop = ingredientOnTop as ICuttable;

        cuttingProcess++;
        if (cuttingProcess >= cuttableOnTop.CuttingProcessCountMax) {
            Debug.Log("Sliced");
            cuttableOnTop.SlicedUp();
        }
    }
}
