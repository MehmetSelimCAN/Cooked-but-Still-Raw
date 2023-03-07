using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Stores the type and statu information of each ingredient in a recipe.
[System.Serializable]
public struct IngredientInformation {
    public IngredientType ingredientType;
    public IngredientStatus ingredientStatus;

    public IngredientInformation(Ingredient ingredient) {
        this.ingredientType = ingredient.IngredientType;
        this.ingredientStatus = ingredient.IngredientStatus;
    }
}

//Stores everyt data related to Recipe object.
[System.Serializable]
public class Recipe {

    public string recipeName;
    public float recipePrepareTime;
    public bool isAvailableOnThisLevel;
    public List<IngredientInformation> ingredientInformations;

}
