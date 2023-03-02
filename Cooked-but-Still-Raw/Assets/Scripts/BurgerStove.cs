using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BurgerStove : Furniture {

    public override void ClearItemOnTop() {
        //Üstündeki pan'ý aldýðýmýzda timer'ý durdur.
        Pan panOnTop = itemOnTop as Pan;
        panOnTop.StopAllCoroutines();

        base.ClearItemOnTop();
    }

    public override bool CanSetItemOnTop(Item droppedItem) {
        if (itemOnTop == null) {
            //Üstü boþ ve Pan býrakmaya çalýþýyorsak...
            if (droppedItem is Pan) {
                return true;
            }

            //Üstü boþ ve Pan dýþýnda bir þey býrakmaya çalýþýyorsak...
            return false;
        }
        else {
            //Üstünde Pan var ve bir þey býrakmaya çalýþýyorsak...
            Pan panOnTop = itemOnTop as Pan;
            return panOnTop.CanAddIngredient(droppedItem);
        }
    }

    public override void SetItemOnTop(Item droppedItem) {
        if (itemOnTop == null) {
            HandleDroppedItemPosition(droppedItem);
            itemOnTop = droppedItem;

            Pan panOnTop = itemOnTop as Pan;
            //Paný býraktýðýmýzda üstünde ingredient var ise timer'larý baþlat.
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
        else {
            Pan panOnTop = itemOnTop as Pan;
            Ingredient droppedIngredient = droppedItem as Ingredient;
            panOnTop.AddIngredient(droppedIngredient);
        }
    }
}
