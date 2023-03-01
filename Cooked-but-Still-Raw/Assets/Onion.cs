using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Onion : Ingredient, ICuttable, ICookable {

    [SerializeField] private Mesh processedMesh;
    [SerializeField] private Mesh liquidMesh;
    [SerializeField] private Mesh cookedMesh;
    [SerializeField] private Mesh burnedMesh;

    [SerializeField] private float cuttingProcessCountMax;
    public float CuttingProcessCountMax { get { return cuttingProcessCountMax; } }

    [SerializeField] private float cookingTimerMax;
    public float CookingTimerMax { get { return cookingTimerMax; } }

    public override void Awake() {
        base.Awake();
        ingredientType = IngredientType.Onion;
    }

    public void SlicedUp() {
        ChangeStatus(IngredientStatus.Processed);
        ChangeMesh(IngredientStatus.Processed);
    }

    public void Liquize() {
        ChangeStatus(IngredientStatus.Liquid);
        ChangeMesh(IngredientStatus.Liquid);
    }

    public void CookedUp() {
        ChangeStatus(IngredientStatus.Cooked);
        ChangeMesh(IngredientStatus.Cooked);
    }

    public void BurnedUp() {
        ChangeStatus(IngredientStatus.Burned);
        ChangeMesh(IngredientStatus.Burned);
    }

    public override void ChangeMesh(IngredientStatus newStatus) {
        switch (newStatus) {
            case IngredientStatus.Processed:
                ShowUI();
                ingredientMeshFilter.mesh = processedMesh;
                break;
            case IngredientStatus.Liquid:
                ingredientMeshFilter.mesh = liquidMesh;
                break;
            case IngredientStatus.Cooked:
                ingredientMeshFilter.mesh = cookedMesh;
                break;
            case IngredientStatus.Burned:
                ingredientMeshFilter.mesh = burnedMesh;
                break;
        }
    }
}
