using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Dish : Item {

    [SerializeField] protected Transform ingredientSlot;

    protected int ingredientCapacity;
    private int currentIngredientQuantity = 0;
    public int CurrentIngredientQuantity { get { return currentIngredientQuantity; } set { currentIngredientQuantity = Mathf.Clamp(value, 0, 6); } }
    public bool HasAnyIngredientOnTop { get { return currentIngredientQuantity > 0; } }
    public bool isFull { get { return currentIngredientQuantity >= ingredientCapacity; } }

    protected List<Ingredient> currentIngredients = new List<Ingredient>();
    public List<Ingredient> CurrentIngredients { get { return currentIngredients; } }

    //Returns a boolean showing whether an ingredient can be added into the specific dish or not.
    public virtual bool CanAddIngredient(Item droppedItem) { return false; }
    //Adds the ingridient to the dish.
    public virtual void AddIngredient(Ingredient droppedIngredient) { }
    //Adds the ingridient to the ui of the dish and displays it.
    public virtual void AddIngredientUI(Ingredient droppedIngredient) {
        droppedIngredient.HideUI();

        if (!ingredientUI_Icons.gameObject.activeInHierarchy) {
            ingredientUI_Icons.gameObject.SetActive(true);
        }

        Sprite ingredientSprite = SpriteProvider.Instance.GetIngredientSprite(droppedIngredient.IngredientType);
        Transform ingredientUI_Icon = ingredientUI_Icons.transform.GetChild(CurrentIngredientQuantity - 1).GetChild(1);
        ingredientUI_Icon.parent.gameObject.SetActive(true);
        ingredientUI_Icon.GetComponent<Image>().sprite = ingredientSprite;
    }

    //Returns a boolean showing whether the ingredients can be transfered between dishes.
    public virtual bool TryToTransferIngredients(Dish dishToBeTransferred) {
        bool ingredientsMatched = CheckIngredientMatches(dishToBeTransferred);
        if (ingredientsMatched) {
            TransferIngredients(dishToBeTransferred);
            return true;
        }
        else {
            return false;
        }
    }

    //Returns a boolean showing whether the ingredients are acceptable by the target dish.
    public virtual bool CheckIngredientMatches(Dish dishToBeTransferred) {
        bool ingredientsMatched = false;

        foreach (Ingredient ingredientInDish in CurrentIngredients) {
            ingredientsMatched = dishToBeTransferred.CanAddIngredient(ingredientInDish);
            if (!ingredientsMatched) {
                return ingredientsMatched;
            }
        }

        return ingredientsMatched;
    }

    //Transfer the ingredients between dishes.
    public virtual void TransferIngredients(Dish dishToBeTransferred) {
        foreach (Ingredient ingredientInDish in CurrentIngredients) {
            dishToBeTransferred.AddIngredient(ingredientInDish);
        }

        ClearCurrentIngredients();
    }

    public virtual void ClearCurrentIngredients() { }
    public virtual void ClearTimers() { }

    public virtual void ClearIngredientUI() {
        foreach (Transform ingredientUI in ingredientUI_Icons) {
            ingredientUI.gameObject.SetActive(false);
        }

        ingredientUI_Icons.gameObject.SetActive(false);
    }

    public virtual void HandleDroppedIngredientPosition(Ingredient droppedIngredient) {
        droppedIngredient.transform.SetParent(ingredientSlot);
        droppedIngredient.transform.localPosition = Vector3.zero;
    }

    public override void ThrowInTheGarbage() {
        foreach (Transform ingredient in ingredientSlot) {
            Destroy(ingredient.gameObject);
        }

        ClearCurrentIngredients();
        ClearTimers();
    }
}
