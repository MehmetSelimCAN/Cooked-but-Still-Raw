using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pan : Dish {

    private float currentFryingTime = 0;
    private float currentBurningTime = 0;

    private void Awake() {
        ingredientCapacity = 1;
    }

    public override bool CanAddIngredient(Item droppedItem) {
        if (currentIngredientQuantity >= ingredientCapacity) return false;
        if (!(droppedItem is Ingredient)) return false;

        Ingredient droppedIngredient = droppedItem as Ingredient;

        if (droppedIngredient.IngredientStatus != IngredientStatus.Processed) return false;
        if (!(droppedIngredient is IFryable)) return false;

        return true;
    }

    public override void AddIngredient(Ingredient droppedIngredient) {
        droppedIngredient.transform.SetParent(ingredientSlot);
        droppedIngredient.transform.localPosition = Vector3.zero;
        currentIngredientQuantity++;
        currentIngredients.Add(droppedIngredient);

        BurgerStove burgerStoveUnder = transform.GetComponentInParent<BurgerStove>();
        //Eðer ingredient eklendiðinde pan; burger stove'un üstündeyse frying timer baþlat.
        if (burgerStoveUnder != null) {
            IFryable droppedFryableIngredient = droppedIngredient as IFryable;
            StartCoroutine(FryingTimer(droppedFryableIngredient.FryingTimerMax));
        }
    }

    public override void ClearCurrentIngredients() {
        currentIngredientQuantity = 0;
        currentIngredients.Clear();
        Debug.Log("Clear Pan");
    }

    public override void TransferIngredients(Dish dishToBeTransferred) {
        if (dishToBeTransferred.HasAnyIngredientOnTop) {
            if (!isFull) {
                bool ingredientsMatched = true;
                foreach (Ingredient ingredientInDishToBeTransferred in dishToBeTransferred.CurrentIngredients) {
                    ingredientsMatched = CanAddIngredient(ingredientInDishToBeTransferred);
                    if (!ingredientsMatched) {
                        break;
                    }
                }

                if (ingredientsMatched) {
                    foreach (Ingredient ingredientInDishToBeTransferred in dishToBeTransferred.CurrentIngredients) {
                        AddIngredient(ingredientInDishToBeTransferred);
                    }

                    dishToBeTransferred.ClearCurrentIngredients();
                }
                else {
                    Debug.Log("Recipe uyuþmuyor, transfer gerçekleþtirelemedi");
                    return;
                }
            }
        }

        else {
            foreach (Ingredient ingredientInDish in currentIngredients) {
                dishToBeTransferred.AddIngredient(ingredientInDish);
            }

            ClearCurrentIngredients();
        }
    }

    public Ingredient GetIngredientOnTop() {
        Ingredient ingredientOnTop = currentIngredients[0];
        return ingredientOnTop;
    }

    public IFryable GetFryableOnTop() {
        Ingredient ingredientOnTop = GetIngredientOnTop();
        IFryable fryableOnTop = ingredientOnTop as IFryable;
        return fryableOnTop;
    }

    public IEnumerator FryingTimer(float ingredientFryingTimer) {
        Debug.Log("Frying start");

        while (currentFryingTime < ingredientFryingTimer) {
            currentFryingTime += Time.deltaTime;
            yield return null;
        }

        IFryable fryableOnTop = GetFryableOnTop();
        fryableOnTop.FriedUp();
        Debug.Log("Fried");
        
        StartCoroutine(BurningTimer(fryableOnTop.BurningTimerMax));
    }

    public IEnumerator BurningTimer(float ingredientBurningTimer) {
        Debug.Log("Burning start");

        while (currentBurningTime < ingredientBurningTimer) {
            currentBurningTime += Time.deltaTime;
            yield return null;
        }

        IFryable fryableOnTop = GetFryableOnTop();
        fryableOnTop.BurnedUp();
        Debug.Log("Burned");
    }

    public override void ClearTimers() {
        currentFryingTime = 0;
        currentBurningTime = 0;
    }
}
