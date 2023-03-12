using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

//Stores every data related to Recipe object.
[System.Serializable]
public class Recipe {

    public string recipeName;
    public Sprite recipeSprite;
    public float recipePrepareTime;
    public int recipePrize;
    public bool isAvailableOnThisLevel;
    public List<IngredientInformation> ingredientInformations;

}
