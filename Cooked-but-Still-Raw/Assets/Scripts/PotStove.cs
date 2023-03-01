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
            //Üstü boþ ve Pot býrakmaya çalýþýyorsak
            if (droppedItem is Pot) {
                return true;
            }

            //Üstü boþ ve Pot dýþýnda bir þey býrakmaya çalýþýyorsak
            return false;
        }
        else {
            //Üstünde Pot var ve bir þey býrakmaya çalýþýyorsak
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
