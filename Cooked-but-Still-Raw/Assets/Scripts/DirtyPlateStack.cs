using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirtyPlateStack : Item {

    [HideInInspector] public int dirtyPlateCount;

    private void Awake() {
        ShowUI();
    }

    public override void ThrowInTheGarbage() {
        return;
    }
}
