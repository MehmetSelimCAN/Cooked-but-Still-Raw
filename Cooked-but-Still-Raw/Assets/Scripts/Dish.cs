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

    public virtual bool CanAddIngredient(Item droppedItem) { return false; }
    public virtual void AddIngredient(Ingredient droppedIngredient) { }
    public virtual void AddIngredientUI(Ingredient droppedIngredient) {
        droppedIngredient.HideUI();

        if (!ingredientUI_Icons.gameObject.activeInHierarchy) {
            ingredientUI_Icons.gameObject.SetActive(true);
        }

        Sprite ingredientSprite = SpriteProvider.Instance.GetIngredientSprite(droppedIngredient.IngredientType);
        Transform ingredientUI_Icon = ingredientUI_Icons.transform.GetChild(CurrentIngredientQuantity - 1);
        ingredientUI_Icon.gameObject.SetActive(true);
        ingredientUI_Icon.GetComponent<Image>().sprite = ingredientSprite;
    }

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
