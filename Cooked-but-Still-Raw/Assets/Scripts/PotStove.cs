using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotStove : Furniture {

    public override Item GetItemOnTop() {
        //Timer durdur
        Pot potOnTop = itemOnTop as Pot;
        potOnTop.StopAllCoroutines();

        return itemOnTop;
    }

    public override bool CanSetItemOnTop(Item droppedItem) {
        if (itemOnTop == null) {
            //�st� bo� ve Pot b�rakmaya �al���yorsak
            if (droppedItem is Pot) {
                return true;
            }

            //�st� bo� ve Pot d���nda bir �ey b�rakmaya �al���yorsak
            return false;
        }
        else {
            //�st�nde Pot var ve bir �ey b�rakmaya �al���yorsak
            Pot potOnTop = itemOnTop as Pot;
            return potOnTop.CanAddIngredient(droppedItem);
        }
    }

    public override void SetItemOnTop(Item droppedItem) {
        if (itemOnTop == null) {
            droppedItem.transform.SetParent(itemSlot);
            droppedItem.transform.localPosition = Vector3.zero;
            itemOnTop = droppedItem;

            Pot potOnTop = itemOnTop as Pot;
            if (potOnTop.HasAnyIngredientOnTop) {
                Ingredient ingredientOnPot = potOnTop.GetIngredientOnTop();

                if (ingredientOnPot.IngredientStatus == IngredientStatus.Cooked) {
                    potOnTop.StartCoroutine(potOnTop.BurningTimer());
                }
                else if (ingredientOnPot.IngredientStatus != IngredientStatus.Burned) {
                    potOnTop.StartCoroutine(potOnTop.CookingTimer());
                }
            }
        }
        else {
            Pot potOnTop = itemOnTop as Pot;
            Ingredient droppedIngredient = droppedItem as Ingredient;
            potOnTop.AddIngredient(droppedIngredient);
        }
    }
}
