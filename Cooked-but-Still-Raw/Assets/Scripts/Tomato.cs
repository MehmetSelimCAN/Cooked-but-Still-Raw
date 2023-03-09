using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tomato : Ingredient, ICuttable {

    [SerializeField] private GameObject processedModel;

    [SerializeField] private float cuttingProcessCount;
    public float CuttingProcessCount { get { return cuttingProcessCount; } }

    private void Awake() {
        ingredientType = IngredientType.Tomato;
    }

    public void SlicedUp() {
        ChangeStatus(IngredientStatus.Processed);
        ChangeMesh(IngredientStatus.Processed);
    }

    public override void ChangeMesh(IngredientStatus newStatus) {
        currentModel.SetActive(false);

        switch (newStatus) {
            case IngredientStatus.Processed:
                processedModel.SetActive(true);
                currentModel = processedModel;
                ShowUI();
                break;
        }
    }
}
