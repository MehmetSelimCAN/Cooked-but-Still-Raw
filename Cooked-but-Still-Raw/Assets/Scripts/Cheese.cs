using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cheese : Ingredient, ICuttable {

    [SerializeField] private GameObject processedModel;

    [SerializeField] private float cuttingProcessCount;
    public float CuttingProcessCount { get { return cuttingProcessCount; } }

    public void SlicedUp() {
        ChangeStatus(IngredientStatus.Processed);
        ChangeMesh(IngredientStatus.Processed);
    }

    public override void ChangeMesh(IngredientStatus newStatus) {
        currentModel.SetActive(false);

        switch (newStatus) {
            case IngredientStatus.Raw:
                HideUI();
                defaultModel.SetActive(true);
                currentModel = defaultModel;
                break;
            case IngredientStatus.Processed:
                ShowUI();
                processedModel.SetActive(true);
                currentModel = processedModel;
                break;
        }
    }
}
