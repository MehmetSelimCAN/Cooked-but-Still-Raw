using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BurgerStove : Furniture {

    public override void ClearItemOnTop() {
        //Pause the timer when we lift up the pan on top of it.
        Pan panOnTop = itemOnTop as Pan;
        panOnTop.StopAllCoroutines();

        base.ClearItemOnTop();
    }

    public override bool CanSetItemOnTop(Item droppedItem) {
        if (itemOnTop == null) {
            //If we are trying to put a pan on top of the empty burger stove.
            if (droppedItem is Pan) {
                return true;
            }

            //If we are trying to put something other than a pan on top of the empty burger stove.
            return false;
        }
        else {
            //If we are trying to add an ingredient inside the pan on top of the burger stove.
            Pan panOnTop = itemOnTop as Pan;
            return panOnTop.CanAddIngredient(droppedItem);
        }
    }

    public override void SetItemOnTop(Item droppedItem) {
        if (itemOnTop == null) {
            HandleDroppedItemPosition(droppedItem);
            itemOnTop = droppedItem;

            Pan panOnTop = itemOnTop as Pan;
            //If there is an ingredient inside the pan when we put it on top of the stove.
            if (panOnTop.HasAnyIngredientOnTop) {
                Ingredient ingredientOnPan = panOnTop.GetIngredientOnTop();

                if (ingredientOnPan.IngredientStatus == IngredientStatus.Cooked) {
                    panOnTop.StartCoroutine(panOnTop.BurningTimer());
                }
                else if (ingredientOnPan.IngredientStatus != IngredientStatus.Burned) {
                    panOnTop.StartCoroutine(panOnTop.FryingTimer());
                }
            }
        }
        //If we try to add an ingredient to the pan on top of the burger stove.
        else
        {
            Pan panOnTop = itemOnTop as Pan;
            Ingredient droppedIngredient = droppedItem as Ingredient;
            panOnTop.AddIngredient(droppedIngredient);
        }
    }
}
