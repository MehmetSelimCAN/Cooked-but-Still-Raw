using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tomato : Ingredient, ICuttable {

    [SerializeField] private Mesh processedMesh;

    [SerializeField] private float cuttingProcessCount;
    public float CuttingProcessCount { get { return cuttingProcessCount; } }

    public void SlicedUp() {
        ChangeStatus(IngredientStatus.Processed);
        ChangeMesh(IngredientStatus.Processed);
    }

    public override void ChangeMesh(IngredientStatus newStatus) {
        switch (newStatus) {
            case IngredientStatus.Processed:
                //ingredientMeshFilter.mesh = processedMesh;
                break;
        }
    }
}
