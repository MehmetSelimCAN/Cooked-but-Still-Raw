using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngridientBox : Furniture {

    public override Item GetItemOnTop() {
        var ingridient = Instantiate(itemOnTop, itemSlot);
        return ingridient;
    }
}
