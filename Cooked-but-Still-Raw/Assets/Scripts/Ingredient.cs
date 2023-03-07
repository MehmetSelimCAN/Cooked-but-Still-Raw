using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Ingredient : Item {

    protected IngredientType ingredientType;
    [SerializeField] protected IngredientStatus ingredientStatus;
    public IngredientType IngredientType { get { return ingredientType; } }
    public IngredientStatus IngredientStatus { get { return ingredientStatus; } }

    [SerializeField] protected GameObject currentModel;

    protected void ChangeStatus(IngredientStatus newStatus) {
        ingredientStatus = newStatus;
    }

    public virtual void ChangeMesh(IngredientStatus newStatus) { }

    public override void ThrowInTheGarbage() {
        Destroy(gameObject);
    }
}
