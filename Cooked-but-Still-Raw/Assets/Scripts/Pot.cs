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
        CurrentIngredientQuantity++;
        currentIngredients.Add(droppedIngredient);
        AddIngredientUI(droppedIngredient);
        HandleDroppedIngredientPosition(droppedIngredient);

        ICookable cookableIngredient = droppedIngredient as ICookable;
        cookableIngredient.Liquize();

        currentIngredientsTimer += cookableIngredient.CookingTime;
        timerFillImage.fillAmount = currentCookingTime / currentIngredientsTimer;
        currentBurningTime = 0;

        PotStove potStoveUnder = transform.GetComponentInParent<PotStove>();
        //Eðer ingredient eklendiðinde pot; pot stove'un üstündeyse cooking timer baþlat.
        if (potStoveUnder != null) {
            StartCoroutine(CookingTimer());
        }
    }

    public override void HandleDroppedIngredientPosition(Ingredient droppedIngredient) {
        droppedIngredient.transform.SetParent(ingredientSlot);
        droppedIngredient.transform.localPosition = Vector3.up * soupSnappingOffSet * (CurrentIngredientQuantity - 1);
        droppedIngredient.transform.localScale = Vector3.one;
    }

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

        while (currentCookingTime < currentIngredientsTimer) {
            currentCookingTime += Time.deltaTime;
            timerFillImage.fillAmount = currentCookingTime / currentIngredientsTimer;
            yield return null;
        }

        ICookable cookableOnTop = GetCookableOnTop();
        cookableOnTop.CookedUp();
        StartCoroutine(BurningTimer());
    }

    public IEnumerator BurningTimer() {
        ICookable cookableOnTop = GetCookableOnTop();
        float ingredientBurningTimer = cookableOnTop.BurningTime;

        while (currentBurningTime < ingredientBurningTimer) {
            currentBurningTime += Time.deltaTime;
            timerFillImage.fillAmount = currentBurningTime / ingredientBurningTimer;
            yield return null;
        }

        cookableOnTop.BurnedUp();

        timerUI.gameObject.SetActive(false);
    }
}