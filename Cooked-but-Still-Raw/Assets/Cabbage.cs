using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cabbage : Ingredient, ICuttable {

    [SerializeField] private Mesh processedMesh;

    [SerializeField] private float cuttingProcessCountMax;
    public float CuttingProcessCountMax { get { return cuttingProcessCountMax; } }

    public override void Awake() {
        base.Awake();
        ingredientType = IngredientType.Cabbage;
    }

    public void SlicedUp() {
        ChangeStatus(IngredientStatus.Processed);
        ChangeMesh(IngredientStatus.Processed);
    }

    public override void ChangeMesh(IngredientStatus newStatus) {
        switch (newStatus) {
            case IngredientStatus.Processed:
                ingredientMeshFilter.mesh = processedMesh;
                break;
        }
    }
}
