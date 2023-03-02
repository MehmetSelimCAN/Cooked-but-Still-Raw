using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bread : Ingredient {

    public override void Awake() {
        base.Awake();
        ingredientType = IngredientType.Bread;
    }
}