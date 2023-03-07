using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirtyPlateStack : Item {

    private void Awake() {
        ShowUI();
    }

    public override void ThrowInTheGarbage() {
        return;
    }
}
