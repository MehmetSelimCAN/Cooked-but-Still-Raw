using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ingredient : Item {

    protected IngredientType ingredientType;
    [SerializeField] protected IngredientStatus ingredientStatus;
    public IngredientType IngredientType { get { return ingredientType; } }
    public IngredientStatus IngredientStatus { get { return ingredientStatus; } }

    protected MeshFilter ingredientMeshFilter;
    [SerializeField] private Mesh rawMesh;

    public virtual void Awake() {
        ingredientMeshFilter = GetComponentInChildren<MeshFilter>();
    }

    private void ResetAttributes() {
        ingredientStatus = IngredientStatus.Raw;
        ingredientMeshFilter.mesh = rawMesh;
    }

    protected void ChangeStatus(IngredientStatus newStatus) {
        ingredientStatus = newStatus;
    }

    public virtual void ChangeMesh(IngredientStatus newStatus) { }
}
