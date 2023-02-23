using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ingridient : Item {

    protected MeshFilter ingridientMeshFilter;
    [SerializeField] private Mesh rawMesh;

    [SerializeField] protected IngridientStatus ingridientStatus;

    public IngridientStatus IngridientStatus { get { return ingridientStatus; } }

    private void Awake() {
        ingridientMeshFilter = GetComponentInChildren<MeshFilter>();
    }

    private void ResetAttributes() {
        ingridientStatus = IngridientStatus.Raw;
        ingridientMeshFilter.mesh = rawMesh;
    }

    protected void ChangeStatus(IngridientStatus newStatus) {
        ingridientStatus = newStatus;
    }

    public virtual void ChangeMesh(IngridientStatus newStatus) { }
}
