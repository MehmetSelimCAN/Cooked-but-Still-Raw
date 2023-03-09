using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Pot : Dish {

    private float soupSnappingOffSet;

    private float currentCookingTime = 0;
    private float currentBurningTime = 0;
    private float currentIngredientsTotalCookingTime = 0;

    [SerializeField] protected Transform timerUI;
    [SerializeField] protected Image timerFillImage;

    private void Awake() {
        soupSnappingOffSet = 20;
        ingredientCapacity = 3;
    }

    //Returns a boolean showing that whether an ingredient can be added into the pot or not.
    public override bool CanAddIngredient(Item droppedItem) {
        //If there is no available room in the pot.
        if (CurrentIngredientQuantity >= ingredientCapacity) return false;
        //If we are trying to put something else other than an ingredient inside of the pot
        if (!(droppedItem is Ingredient)) return false;

        Ingredient droppedIngredient = droppedItem as Ingredient;

        if (droppedIngredient.IngredientStatus != IngredientStatus.Processed) return false;

        if (!(droppedIngredient is ICookable)) return false;

        return true;
    }

    //Adds the ingridient to the pot and updates itself accordingly.
    public override void AddIngredient(Ingredient droppedIngredient) {
        CurrentIngredientQuantity++;
        currentIngredients.Add(droppedIngredient);
        AddIngredientUI(droppedIngredient);
        HandleDroppedIngredientPosition(droppedIngredient);

        ICookable cookableIngredient = droppedIngredient as ICookable;
        cookableIngredient.Liquize();

        currentIngredientsTotalCookingTime += cookableIngredient.CookingTime;
        timerFillImage.fillAmount = currentCookingTime / currentIngredientsTotalCookingTime;
        currentBurningTime = 0;

        StopAllCoroutines();
        PotStove potStoveUnder = transform.GetComponentInParent<PotStove>();
        //If the pot is on top of the stove when we add an ingredient.
        if (potStoveUnder != null) {
            StartCoroutine(CookingTimer());
        }
    }

    //Place the added ingredient into the pot.
    public override void HandleDroppedIngredientPosition(Ingredient droppedIngredient) {
        droppedIngredient.transform.SetParent(ingredientSlot);
        droppedIngredient.transform.localPosition = Vector3.up * soupSnappingOffSet * (CurrentIngredientQuantity - 1);
        droppedIngredient.transform.localScale = Vector3.one;
    }

    //Reset pot.
    public override void ClearCurrentIngredients() {
        CurrentIngredientQuantity = 0;
        currentIngredients.Clear();

        ClearIngredientUI();
        ClearTimers();
    }

    public override void ClearTimers() {
        currentCookingTime = 0;
        currentBurningTime = 0;
        StopAllCoroutines();
    }

    public override void ClearIngredientUI() {
        base.ClearIngredientUI();
        timerUI.gameObject.SetActive(false);
    }

    public Ingredient GetIngredientOnTop() {
        return currentIngredients.Last();
    }

    public IEnumerator CookingTimer() {
        timerUI.gameObject.SetActive(true);

        while (currentCookingTime < currentIngredientsTotalCookingTime) {
            currentCookingTime += Time.deltaTime;
            timerFillImage.fillAmount = currentCookingTime / currentIngredientsTotalCookingTime;
            yield return null;
        }

        foreach (Ingredient ingredient in currentIngredients) {
            ICookable cookableOnTop = ingredient as ICookable;
            cookableOnTop.CookedUp();
        }

        StartCoroutine(BurningTimer());
    }

    public IEnumerator BurningTimer() {
        Ingredient ingredientOnTop = GetIngredientOnTop();
        ICookable cookableOnTop = ingredientOnTop as ICookable;
        float ingredientBurningTime = cookableOnTop.BurningTime;

        while (currentBurningTime < ingredientBurningTime) {
            currentBurningTime += Time.deltaTime;
            timerFillImage.fillAmount = currentBurningTime / ingredientBurningTime;
            yield return null;
        }

        cookableOnTop.BurnedUp();

        timerUI.gameObject.SetActive(false);
    }
}