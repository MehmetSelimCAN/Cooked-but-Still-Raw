using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Pan : Dish {

    private float currentFryingTime = 0;
    private float currentBurningTime = 0;

    [SerializeField] protected Transform timerUI;
    [SerializeField] protected Image timerFillImage;

    [SerializeField] private Transform fireWarning;
    private float fireWarningTime = 3f;

    private PanStove panStoveUnder;

    private void Awake() {
        ingredientCapacity = 1;
    }

    //Returns a boolean showing that whether an ingredient can be added into the pan or not.
    public override bool CanAddIngredient(Item droppedItem) {
        if (CurrentIngredientQuantity >= ingredientCapacity) return false;
        if (!(droppedItem is Ingredient)) return false;

        Ingredient droppedIngredient = droppedItem as Ingredient;

        if (droppedIngredient.IngredientStatus != IngredientStatus.Processed) return false;
        if (!(droppedIngredient is IFryable)) return false;

        return true;
    }

    //Adds the ingridient to the pan and updates itself accordingly.
    public override void AddIngredient(Ingredient droppedIngredient) {
        HandleDroppedIngredientPosition(droppedIngredient);

        CurrentIngredientQuantity++;
        currentIngredients.Add(droppedIngredient);

        AddIngredientUI(droppedIngredient);

        panStoveUnder = transform.GetComponentInParent<PanStove>();
        //If the pan is on top of the burger stove when we add an ingredient.
        if (panStoveUnder != null) {
            StartCoroutine(FryingTimer());
        }
    }

    //Reset pan.
    public override void ClearCurrentIngredients() {
        CurrentIngredientQuantity = 0;
        currentIngredients.Clear();

        ClearIngredientUI();
        ClearTimers();
    }

    public override void ClearTimers() {
        currentFryingTime = 0;
        currentBurningTime = 0;
        StopAllCoroutines();
    }

    public override void ClearIngredientUI() {
        base.ClearIngredientUI();
        timerUI.gameObject.SetActive(false);
        fireWarning.gameObject.SetActive(false);

        panStoveUnder = transform.GetComponentInParent<PanStove>();
        if (panStoveUnder != null) {
            panStoveUnder.TurnOff();
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

    public IEnumerator FryingTimer() {
        timerUI.gameObject.SetActive(true);
        panStoveUnder = transform.GetComponentInParent<PanStove>();
        panStoveUnder.TurnOn();

        IFryable fryableOnTop = GetFryableOnTop();
        float ingredientFryingTimer = fryableOnTop.FryingTime;

        while (currentFryingTime < ingredientFryingTimer) {
            currentFryingTime += Time.deltaTime;
            timerFillImage.fillAmount = currentFryingTime / ingredientFryingTimer;
            yield return null;
        }

        fryableOnTop.FriedUp();
        StartCoroutine(BurningTimer());
    }

    public IEnumerator BurningTimer() {
        panStoveUnder = transform.GetComponentInParent<PanStove>();

        IFryable fryableOnTop = GetFryableOnTop();
        float ingredientBurningTime = fryableOnTop.BurningTime;

        while (currentBurningTime < ingredientBurningTime) {
            currentBurningTime += Time.deltaTime;
            timerFillImage.fillAmount = currentBurningTime / ingredientBurningTime;

            if (ingredientBurningTime - currentBurningTime < fireWarningTime) {
                if (!fireWarning.gameObject.activeInHierarchy) {
                    fireWarning.gameObject.SetActive(true);
                }
            }

            yield return null;
        }


        fryableOnTop.BurnedUp();
        fireWarning.gameObject.SetActive(false);
        timerUI.gameObject.SetActive(false);
        panStoveUnder.TurnOff();
    }
}
