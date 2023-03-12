using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Ingredient : Item {

    [SerializeField] protected IngredientType ingredientType;
    [SerializeField] protected IngredientStatus ingredientStatus;

    protected IngredientStatus defaultIngredientStatus;

    [SerializeField] protected GameObject defaultModel;
    public IngredientType IngredientType { get { return ingredientType; } }
    public IngredientStatus IngredientStatus { get { return ingredientStatus; } }

    [SerializeField] protected GameObject currentModel;

    private void Awake() {
        defaultIngredientStatus = ingredientStatus;
    }

    protected void ChangeStatus(IngredientStatus newStatus) {
        ingredientStatus = newStatus;
    }

    public virtual void ChangeMesh(IngredientStatus newStatus) { }

    public override void ThrowInTheGarbage() {
        ResetIngredient();
        PoolingManager.Instance.DeActivateToPool(gameObject);
    }

    public virtual void ResetIngredient() {
        ChangeStatus(defaultIngredientStatus);
        ChangeMesh(defaultIngredientStatus);
    }
}
