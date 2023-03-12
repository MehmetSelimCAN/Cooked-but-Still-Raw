using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotStove : Furniture {

    [SerializeField] private Transform fireParticleEffect;

    public override void ClearItemOnTop() {
        //Pause the timer when we lift up the pot from the stove.
        Pot potOnTop = itemOnTop as Pot;
        potOnTop.StopAllCoroutines();
        TurnOff();
        base.ClearItemOnTop();
    }

    public override bool CanSetItemOnTop(Item droppedItem) {
        if (itemOnTop == null) {
            //If we are trying to put the pot on top of the empty stove.
            if (droppedItem is Pot) {
                return true;
            }

            //Stove has nothing on it but we are trying to put something else than a pot.
            return false;
        }
        else {
            //There is pot on top of the stove and we are trying to add something to pot.
            Pot potOnTop = itemOnTop as Pot;
            return potOnTop.CanAddIngredient(droppedItem);
        }
    }

    public override void SetItemOnTop(Item droppedItem) {
        if (itemOnTop == null) {
            HandleDroppedItemPosition(droppedItem);
            itemOnTop = droppedItem;

            Pot potOnTop = itemOnTop as Pot;
            //Start the timers if we put a pot on the stove while there are some ingredients inside of it.
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
        //If we are trying to add an ingredient inside the pot on top of the stove.
        else
        {
            Pot potOnTop = itemOnTop as Pot;
            Ingredient droppedIngredient = droppedItem as Ingredient;
            potOnTop.AddIngredient(droppedIngredient);
        }
    }

    public void TurnOn() {
        fireParticleEffect.gameObject.SetActive(true);
    }

    public void TurnOff() {
        fireParticleEffect.gameObject.SetActive(false);
    }
}
