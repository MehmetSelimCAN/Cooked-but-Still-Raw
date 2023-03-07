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

        BurgerStove burgerStoveUnder = transform.GetComponentInParent<BurgerStove>();
        //If the pan is on top of the burger stove when we add an ingredient.
        if (burgerStoveUnder != null) {
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
        IFryable fryableOnTop = GetFryableOnTop();
        float ingredientBurningTimer = fryableOnTop.BurningTime;

        while (currentBurningTime < ingredientBurningTimer) {
            currentBurningTime += Time.deltaTime;
            timerFillImage.fillAmount = currentBurningTime / ingredientBurningTimer;
            yield return null;
        }

        fryableOnTop.BurnedUp();

        timerUI.gameObject.SetActive(false);
    }
}
