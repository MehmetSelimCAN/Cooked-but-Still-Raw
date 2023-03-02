using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public struct IngredientInformation {
    public IngredientType ingredientType;
    public IngredientStatus ingredientStatus;
}

[System.Serializable]
public class Recipe {

    public float recipePrepareTime;
    public List<IngredientInformation> ingredientInformations;

}
