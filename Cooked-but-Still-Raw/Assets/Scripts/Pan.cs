using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Pan : Dish {

    private float currentFryingTime = 0;
    private float currentBurningTime = 0;

    [SerializeField] protected Transform timerUI;
    [SerializeField] protected Image timerFillImage;

    private void Awake() {
        ingredientCapacity = 1;
    }

    public override bool CanAddIngredient(Item droppedItem) {
        if (CurrentIngredientQuantity >= ingredientCapacity) return false;
        if (!(droppedItem is Ingredient)) return false;

        Ingredient droppedIngredient = droppedItem as Ingredient;

        if (droppedIngredient.IngredientStatus != IngredientStatus.Processed) return false;
        if (!(droppedIngredient is IFryable)) return false;

        return true;
    }

    public override void AddIngredient(Ingredient droppedIngredient) {
        droppedIngredient.transform.SetParent(ingredientSlot);
        droppedIngredient.transform.localPosition = Vector3.zero;
        CurrentIngredientQuantity++;
        currentIngredients.Add(droppedIngredient);

        AddIngredientUI(droppedIngredient);

        BurgerStove burgerStoveUnder = transform.GetComponentInParent<BurgerStove>();
        //Eðer ingredient eklendiðinde pan; burger stove'un üstündeyse frying timer baþlat.
        if (burgerStoveUnder != null) {
            IFryable droppedFryableIngredient = droppedIngredient as IFryable;
            StartCoroutine(FryingTimer(droppedFryableIngredient.FryingTimerMax));
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
        Debug.Log("Clear Pan");

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
                    Debug.Log("Recipe uyuþmuyor, transfer gerçekleþtirelemedi");
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
        Ingredient ingredientOnTop = currentIngredients[0];
        return ingredientOnTop;
    }

    public IFryable GetFryableOnTop() {
        Ingredient ingredientOnTop = GetIngredientOnTop();
        IFryable fryableOnTop = ingredientOnTop as IFryable;
        return fryableOnTop;
    }

    public IEnumerator FryingTimer(float ingredientFryingTimer) {
        timerUI.gameObject.SetActive(true);

        Debug.Log("Frying start");
        while (currentFryingTime < ingredientFryingTimer) {
            currentFryingTime += Time.deltaTime;
            timerFillImage.fillAmount = currentFryingTime / ingredientFryingTimer;
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
            timerFillImage.fillAmount = currentBurningTime / ingredientBurningTimer;
            yield return null;
        }

        IFryable fryableOnTop = GetFryableOnTop();
        fryableOnTop.BurnedUp();
        Debug.Log("Burned");

        timerUI.gameObject.SetActive(false);
    }

    public override void ClearTimers() {
        currentFryingTime = 0;
        currentBurningTime = 0;
    }
}
