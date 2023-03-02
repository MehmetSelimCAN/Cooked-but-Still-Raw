using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BurgerStove : Furniture {

    public override void ClearItemOnTop() {
        //�st�ndeki pan'� ald���m�zda timer'� durdur.
        Pan panOnTop = itemOnTop as Pan;
        panOnTop.StopAllCoroutines();

        base.ClearItemOnTop();
    }

    public override bool CanSetItemOnTop(Item droppedItem) {
        if (itemOnTop == null) {
            //�st� bo� ve Pan b�rakmaya �al���yorsak...
            if (droppedItem is Pan) {
                return true;
            }

            //�st� bo� ve Pan d���nda bir �ey b�rakmaya �al���yorsak...
            return false;
        }
        else {
            //�st�nde Pan var ve bir �ey b�rakmaya �al���yorsak...
            Pan panOnTop = itemOnTop as Pan;
            return panOnTop.CanAddIngredient(droppedItem);
        }
    }

    public override void SetItemOnTop(Item droppedItem) {
        if (itemOnTop == null) {
            HandleDroppedItemPosition(droppedItem);
            itemOnTop = droppedItem;

            Pan panOnTop = itemOnTop as Pan;
            //Pan� b�rakt���m�zda �st�nde ingredient var ise timer'lar� ba�lat.
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
