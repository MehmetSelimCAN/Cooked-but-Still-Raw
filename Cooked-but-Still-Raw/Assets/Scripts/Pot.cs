using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Pot : Dish {

    private float soupSnappingOffSet;

    private float currentCookingTime = 0;
    private float currentBurningTime = 0;
    private float currentIngredientsTimer = 0;

    [SerializeField] protected Transform timerUI;
    [SerializeField] protected Image timerFillImage;

    private void Awake() {
        soupSnappingOffSet = 20;
        ingredientCapacity = 3;
    }

    public override bool CanAddIngredient(Item droppedItem) {
        if (CurrentIngredientQuantity >= ingredientCapacity) return false;
        if (!(droppedItem is Ingredient)) return false;

        Ingredient droppedIngredient = droppedItem as Ingredient;

        if (droppedIngredient.IngredientStatus != IngredientStatus.Processed) return false;
        if (!(droppedIngredient is ICookable)) return false;

        return true;
    }

    public override void AddIngredient(Ingredient droppedIngredient) {
        currentIngredients.Add(droppedIngredient);
        CurrentIngredientQuantity++;

        ICookable cookableIngredient = droppedIngredient as ICookable;
        cookableIngredient.Liquize();
        droppedIngredient.transform.SetParent(ingredientSlot);
        droppedIngredient.transform.localPosition = Vector3.up * soupSnappingOffSet * (CurrentIngredientQuantity - 1);
        droppedIngredient.transform.localScale = Vector3.one;

        AddIngredientUI(droppedIngredient);

        ICookable droppedCookableIngredient = droppedIngredient as ICookable;
        currentIngredientsTimer += droppedCookableIngredient.CookingTimerMax;
        currentBurningTime = 0;
        timerFillImage.fillAmount = currentCookingTime / currentIngredientsTimer;

        PotStove potStoveUnder = transform.GetComponentInParent<PotStove>();
        //E�er ingredient eklendi�inde pot; pot stove'un �st�ndeyse cooking timer ba�lat.
        if (potStoveUnder != null) {
            StopAllCoroutines();
            StartCoroutine(CookingTimer());
        }
    }

    public override void AddIngredientUI(Ingredient droppedIngredient) {
        droppedIngredient.HideUI();

        if (CurrentIngredientQuantity == 1) {
            ingredientUICanvasArea.gameObject.SetActive(true);
        }

        ingredientUICanvasArea.transform.GetChild(CurrentIngredientQuantity - 1).gameObject.SetActive(true);
        ingredientUICanvasArea.transform.GetChild(CurrentIngredientQuantity - 1).GetComponent<Image>().sprite = droppedIngredient.IngredientSprite;
    }

    public override void ClearCurrentIngredients() {
        CurrentIngredientQuantity = 0;
        currentIngredients.Clear();
        Debug.Log("Clear Pot");
        ClearTimers();
        ClearIngredientUI();
    }

    public override void ClearIngredientUI() {
        foreach (Transform ingredientUI in ingredientUICanvasArea) {
            ingredientUI.gameObject.SetActive(false);
        }

        ingredientUICanvasArea.gameObject.SetActive(false);
        timerUI.gameObject.SetActive(false);
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
                    Debug.Log("Recipe uyu�muyor, transfer ger�ekle�tirelemedi");
                    return;
                }
            }
            else {
                bool ingredientsMatched = true;
                foreach (Ingredient ingredientInDish in CurrentIngredients) {
                    ingredientsMatched = dishToBeTransferred.CanAddIngredient(ingredientInDish);
                    if (!ingredientsMatched) {
                        break;
                    }
                }

                if (ingredientsMatched) {
                    foreach (Ingredient ingredientInDish in currentIngredients) {
                        dishToBeTransferred.AddIngredient(ingredientInDish);
                    }

                    ClearCurrentIngredients();
                }
            }
        }

        else {
            bool ingredientsMatched = true;
            foreach (Ingredient ingredientInDish in CurrentIngredients) {
                ingredientsMatched = dishToBeTransferred.CanAddIngredient(ingredientInDish);
                if (!ingredientsMatched) {
                    break;
                }
            }

            if (ingredientsMatched) {
                foreach (Ingredient ingredientInDish in currentIngredients) {
                    dishToBeTransferred.AddIngredient(ingredientInDish);
                }

                ClearCurrentIngredients();
            }
        }
    }

    public Ingredient GetIngredientOnTop() {
        Ingredient ingredientOnTop = currentIngredients.Last();
        return ingredientOnTop;
    }

    public ICookable GetCookableOnTop() {
        Ingredient ingredientOnTop = GetIngredientOnTop();
        ICookable cookableOnTop = ingredientOnTop as ICookable;
        return cookableOnTop;
    }

    public IEnumerator CookingTimer() {
        timerUI.gameObject.SetActive(true);

        Debug.Log("Cooking start");

        while (currentCookingTime < currentIngredientsTimer) {
            currentCookingTime += Time.deltaTime;
            timerFillImage.fillAmount = currentCookingTime / currentIngredientsTimer;
            yield return null;
        }

        ICookable cookableOnTop = GetCookableOnTop();
        cookableOnTop.CookedUp();
        Debug.Log("Cooked");

        StartCoroutine(BurningTimer());
    }

    public IEnumerator BurningTimer() {
        ICookable cookableOnTop = GetCookableOnTop();
        float ingredientBurningTimer = cookableOnTop.BurningTimerMax;
        Debug.Log("Burning start");

        while (currentBurningTime < ingredientBurningTimer) {
            currentBurningTime += Time.deltaTime;
            timerFillImage.fillAmount = currentBurningTime / ingredientBurningTimer;
            yield return null;
        }

        cookableOnTop.BurnedUp();
        Debug.Log("Burned");

        timerUI.gameObject.SetActive(false);
    }

    public override void ClearTimers() {
        currentCookingTime = 0;
        currentBurningTime = 0;
    }
}