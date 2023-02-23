using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tomato : Ingridient, ICuttable {

    [SerializeField] private Mesh processedMesh;

    [SerializeField] private float processCountMax;
    public float ProcessCountMax { get { return processCountMax; } }

    public void SlicedUp() {
        ChangeStatus(IngridientStatus.Processed);
        ChangeMesh(IngridientStatus.Processed);
    }

    public override void ChangeMesh(IngridientStatus newStatus) {
        switch (newStatus) {
            case IngridientStatus.Processed:
                ingridientMeshFilter.mesh = processedMesh;
                break;
        }
    }
}
