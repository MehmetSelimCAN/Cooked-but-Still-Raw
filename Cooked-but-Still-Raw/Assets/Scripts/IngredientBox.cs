using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngredientBox : Furniture {

    public override Item GetItemOnTop() {
        var ingredient = Instantiate(itemOnTop, itemSlot);
        return ingredient;
    }
}
