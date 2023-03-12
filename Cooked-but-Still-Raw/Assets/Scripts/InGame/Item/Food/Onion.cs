using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Onion : Ingredient, ICuttable, ICookable {

    [SerializeField] private GameObject processedModel;
    [SerializeField] private GameObject liquidModel;
    [SerializeField] private GameObject cookedModel;
    [SerializeField] private GameObject burnedModel;

    [SerializeField] private float cuttingProcessCount;
    public float CuttingProcessCount { get { return cuttingProcessCount; } }

    [SerializeField] private float cookingTime;
    public float CookingTime { get { return cookingTime; } }

    [SerializeField] private float burningTime;
    public float BurningTime { get { return burningTime; } }

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
            case IngredientStatus.Liquid:
                liquidModel.SetActive(true);
                currentModel = liquidModel;
                break;
            case IngredientStatus.Cooked:
                cookedModel.SetActive(true);
                currentModel = cookedModel;
                break;
            case IngredientStatus.Burned:
                burnedModel.SetActive(true);
                currentModel = burnedModel;
                break;
        }
    }
}
