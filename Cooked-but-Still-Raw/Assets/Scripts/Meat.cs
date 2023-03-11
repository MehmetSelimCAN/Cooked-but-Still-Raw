using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Meat : Ingredient, ICuttable, IFryable {

    [SerializeField] private GameObject processedModel;
    [SerializeField] private GameObject friedModel;
    [SerializeField] private GameObject burnedModel;

    [SerializeField] private float cuttingProcessCount;
    public float CuttingProcessCount { get { return cuttingProcessCount; } }

    [SerializeField] private float fryingTime;
    public float FryingTime { get { return fryingTime; } }

    [SerializeField] private float burningTime;
    public float BurningTime { get { return burningTime; } }

    public void Awake() {
        ingredientType = IngredientType.Meat;
    }

    public void SlicedUp() {
        ChangeStatus(IngredientStatus.Processed);
        ChangeMesh(IngredientStatus.Processed);
    }

    public void FriedUp() {
        ChangeStatus(IngredientStatus.Fried);
        ChangeMesh(IngredientStatus.Fried);
    }

    public void BurnedUp() {
        ChangeStatus(IngredientStatus.Burned);
        ChangeMesh(IngredientStatus.Burned);
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
            case IngredientStatus.Fried:
                friedModel.SetActive(true);
                currentModel = friedModel;
                break;
            case IngredientStatus.Burned:
                burnedModel.SetActive(true);
                currentModel = burnedModel;
                break;
        }
    }
}
