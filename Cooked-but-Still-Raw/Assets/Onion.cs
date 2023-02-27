using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Onion : Ingridient, ICuttable, ICookable {

    [SerializeField] private Mesh processedMesh;
    [SerializeField] private Mesh liquidMesh;
    [SerializeField] private Mesh cookedMesh;
    [SerializeField] private Mesh burnedMesh;

    [SerializeField] private float processCountMax;
    public float ProcessCountMax { get { return processCountMax; } }

    [SerializeField] private float cookingTimerMax;
    public float CookingTimerMax { get { return cookingTimerMax; } }

    public void SlicedUp() {
        ChangeStatus(IngridientStatus.Processed);
        ChangeMesh(IngridientStatus.Processed);
    }

    public void Liquize() {
        ChangeStatus(IngridientStatus.Liquid);
        ChangeMesh(IngridientStatus.Liquid);
    }

    public void CookedUp() {
        ChangeStatus(IngridientStatus.Cooked);
        ChangeMesh(IngridientStatus.Cooked);
    }

    public void BurnedUp() {
        ChangeStatus(IngridientStatus.Burned);
        ChangeMesh(IngridientStatus.Burned);
    }

    public override void ChangeMesh(IngridientStatus newStatus) {
        switch (newStatus) {
            case IngridientStatus.Processed:
                ingridientMeshFilter.mesh = processedMesh;
                break;
            case IngridientStatus.Liquid:
                ingridientMeshFilter.mesh = liquidMesh;
                break;
            case IngridientStatus.Cooked:
                ingridientMeshFilter.mesh = cookedMesh;
                break;
            case IngridientStatus.Burned:
                ingridientMeshFilter.mesh = burnedMesh;
                break;
        }
    }
}
