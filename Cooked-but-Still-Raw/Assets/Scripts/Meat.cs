using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Meat : Ingredient, ICuttable, IFryable {

    [SerializeField] private Mesh processedMesh;
    [SerializeField] private Mesh cookedMesh;
    [SerializeField] private Mesh burnedMesh;

    [SerializeField] private float cuttingProcessCount;
    public float CuttingProcessCount { get { return cuttingProcessCount; } }

    [SerializeField] private float fryingTime;
    public float FryingTime { get { return fryingTime; } }

    [SerializeField] private float burningTime;
    public float BurningTime { get { return burningTime; } }

    public override void Awake() {
        base.Awake();
        ingredientType = IngredientType.Meat;
    }

    public void SlicedUp() {
        ChangeStatus(IngredientStatus.Processed);
        ChangeMesh(IngredientStatus.Processed);
    }

    public void FriedUp() {
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
            case IngredientStatus.Cooked:
                ingredientMeshFilter.mesh = cookedMesh;
                break;
            case IngredientStatus.Burned:
                ingredientMeshFilter.mesh = burnedMesh;
                break;
        }
    }
}
