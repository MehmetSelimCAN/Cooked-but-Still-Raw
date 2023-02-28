using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tomato : Ingredient, ICuttable {

    [SerializeField] private Mesh processedMesh;

    [SerializeField] private float cuttingProcessCountMax;
    public float CuttingProcessCountMax { get { return cuttingProcessCountMax; } }

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
