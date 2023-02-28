using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BurgerStove : Furniture {

    public override Item GetItemOnTop() {
        //Timer durdur
        Pan panOnTop = itemOnTop as Pan;
        panOnTop.StopAllCoroutines();

        return itemOnTop;
    }

    public override bool CanSetItemOnTop(Item droppedItem) {
        if (itemOnTop == null) {
            //Üstü boþ ve Pan býrakmaya çalýþýyorsak
            if (droppedItem is Pan) {
                return true;
            }

            //Üstü boþ ve Pan dýþýnda bir þey býrakmaya çalýþýyorsak
            return false;
        }
        else {
            //Üstünde Pan var ve bir þey býrakmaya çalýþýyorsak
            Pan panOnTop = itemOnTop as Pan;
            return panOnTop.CanAddIngredient(droppedItem);
        }
    }

    public override void SetItemOnTop(Item droppedItem) {
        if (itemOnTop == null) {
            droppedItem.transform.SetParent(itemSlot);
            droppedItem.transform.localPosition = Vector3.zero;
            itemOnTop = droppedItem;

            Pan panOnTop = itemOnTop as Pan;
            if (panOnTop.HasAnyIngredientOnTop) {
                Ingredient ingredientOnPan = panOnTop.GetIngredientOnTop();
                IFryable fryableOnPan = panOnTop.GetFryableOnTop();

                if (ingredientOnPan.IngredientStatus == IngredientStatus.Cooked) {
                    panOnTop.StartCoroutine(panOnTop.BurningTimer(fryableOnPan.BurningTimerMax));
                }
                else if (ingredientOnPan.IngredientStatus != IngredientStatus.Burned) {
                    panOnTop.StartCoroutine(panOnTop.FryingTimer(fryableOnPan.FryingTimerMax));
                }
            }
        }
        else {
            Pan panOnTop = itemOnTop as Pan;
            Ingredient droppedIngredient = droppedItem as Ingredient;
            panOnTop.AddIngredient(droppedIngredient);
        }
    }
}
