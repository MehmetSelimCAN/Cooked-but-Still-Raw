using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Meat : Ingredient, ICuttable, IFryable {

    [SerializeField] private Mesh processedMesh;
    [SerializeField] private Mesh cookedMesh;
    [SerializeField] private Mesh burnedMesh;

    [SerializeField] private float cuttingProcessCountMax;
    public float CuttingProcessCountMax { get { return cuttingProcessCountMax; } }

    [SerializeField] private float fryingTimerMax;
    public float FryingTimerMax { get { return fryingTimerMax; } }

    [SerializeField] private float burningTimerMax;
    public float BurningTimerMax { get { return burningTimerMax; } }

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
