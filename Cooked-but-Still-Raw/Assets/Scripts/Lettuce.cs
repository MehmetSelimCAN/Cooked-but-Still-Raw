using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lettuce : Ingredient, ICuttable {

    [SerializeField] private GameObject processedModel;

    [SerializeField] private float cuttingProcessCount;
    public float CuttingProcessCount { get { return cuttingProcessCount; } }

    public void Awake() {
        ingredientType = IngredientType.Lettuce;
    }

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
                processedModel.SetActive(true);
                currentModel = processedModel;
                ShowUI();
                break;
        }
    }
}
