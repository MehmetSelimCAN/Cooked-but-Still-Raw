using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Onion : Ingridient, ICuttable, IFryable {

    [SerializeField] private Mesh processedMesh;
    [SerializeField] private Mesh cookedMesh;
    [SerializeField] private Mesh burnedMesh;

    [SerializeField] private float processCountMax;
    public float ProcessCountMax { get { return processCountMax; } }

    [SerializeField] private float fryingTimerMax;
    public float FryingTimerMax { get { return fryingTimerMax; } }

    public void SlicedUp() {
        ChangeStatus(IngridientStatus.Processed);
        ChangeMesh(IngridientStatus.Processed);
    }

    public void FriedUp() {
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
            case IngridientStatus.Cooked:
                ingridientMeshFilter.mesh = cookedMesh;
                break;
            case IngridientStatus.Burned:
                ingridientMeshFilter.mesh = burnedMesh;
                break;
        }
    }
}
