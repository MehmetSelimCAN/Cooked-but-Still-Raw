using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Pot : Dish {

    private float soupSnappingOffSet = 0.3f;

    private float currentCookingTime = 0;
    private float currentBurningTime = 0;
    private float currentIngredientsTotalCookingTime = 0;

    [SerializeField] protected Transform timerUI;
    [SerializeField] protected Image timerFillImage;

    [SerializeField] private Transform fireWarning;
    private float fireWarningTime = 3f;

    private PotStove potStoveUnder;

    private void Awake() {
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

        if (CurrentIngredientQuantity > 0) {
        Ingredient ingredientOnTop = GetIngredientOnTop();
            if (ingredientOnTop.IngredientStatus == IngredientStatus.Burned) {
                return false;
            }
        }

        return true;
    }

    //Adds the ingridient to the pot and updates itself accordingly.
    public override void AddIngredient(Ingredient droppedIngredient) {
        HandleDroppedIngredientPosition(droppedIngredient);

        CurrentIngredientQuantity++;
        currentIngredients.Add(droppedIngredient);

        AddIngredientUI(droppedIngredient);

        ICookable cookableIngredient = droppedIngredient as ICookable;
        cookableIngredient.Liquize();

        currentIngredientsTotalCookingTime += cookableIngredient.CookingTime;
        timerFillImage.fillAmount = currentCookingTime / currentIngredientsTotalCookingTime;
        currentBurningTime = 0;
        fireWarning.gameObject.SetActive(false);

        StopAllCoroutines();
        potStoveUnder = transform.GetComponentInParent<PotStove>();
        //If the pot is on top of the stove when we add an ingredient.
        if (potStoveUnder != null) {
            StartCoroutine(CookingTimer());
            potStoveUnder.TurnOn();
        }
    }

    //Place the added ingredient into the pot.
    public override void HandleDroppedIngredientPosition(Ingredient droppedIngredient) {
        droppedIngredient.transform.SetParent(ingredientSlot);
        droppedIngredient.transform.localPosition = Vector3.up * soupSnappingOffSet * (CurrentIngredientQuantity);
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
        currentIngredientsTotalCookingTime = 0;
        StopAllCoroutines();
    }

    public override void ClearIngredientUI() {
        base.ClearIngredientUI();
        timerUI.gameObject.SetActive(false);
        fireWarning.gameObject.SetActive(false);

        potStoveUnder = transform.GetComponentInParent<PotStove>();
        if (potStoveUnder != null) {
            potStoveUnder.TurnOff();
        }
    }

    public Ingredient GetIngredientOnTop() {
        return currentIngredients.Last();
    }

    public IEnumerator CookingTimer() {
        timerUI.gameObject.SetActive(true);
        fireWarning.gameObject.SetActive(false);

        potStoveUnder = transform.GetComponentInParent<PotStove>();
        potStoveUnder.TurnOn();

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
        potStoveUnder = transform.GetComponentInParent<PotStove>();

        Ingredient ingredientOnTop = GetIngredientOnTop();
        ICookable cookableOnTop = ingredientOnTop as ICookable;
        float ingredientBurningTime = cookableOnTop.BurningTime;

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

        cookableOnTop.BurnedUp();
        fireWarning.gameObject.SetActive(false);
        timerUI.gameObject.SetActive(false);
        potStoveUnder.TurnOff();
    }
}