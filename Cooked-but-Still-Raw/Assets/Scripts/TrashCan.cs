using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashCan : Furniture {

    public override bool CanSetItemOnTop(Item droppedItem) {
        return true;
    }

    public override void SetItemOnTop(Item droppedItem) {
        droppedItem.ThrowInTheGarbage();
    }
}
