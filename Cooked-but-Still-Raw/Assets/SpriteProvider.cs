using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteProvider : MonoBehaviour {

    public static SpriteProvider Instance { get; private set; }

    public Sprite meatSprite;
    public Sprite cabbageSprite;
    public Sprite breadSprite;
    public Sprite onionSprite;
    public Sprite tomatoSprite;
    public Sprite cheeseSprite;
    public Sprite panSprite;
    public Sprite potSprite;

    private void Awake() {
        Instance = this;
    }

    public Sprite GetIngredientSprite(IngredientType ingredientType) {
        Sprite returnSprite = null;
        switch (ingredientType) {
            case IngredientType.Cheese:
                returnSprite = cheeseSprite;
                break;
            case IngredientType.Cabbage:
                returnSprite = cabbageSprite;
                break;
            case IngredientType.Onion:
                returnSprite = onionSprite;
                break;
            case IngredientType.Meat:
                returnSprite = meatSprite;
                break;
            case IngredientType.Bread:
                returnSprite = breadSprite;
                break;
        }

        return returnSprite;
    }

    public Sprite GetDishSpriteForOrderUI(IngredientStatus ingredientStatus) {
        Sprite returnSprite = null;
        switch (ingredientStatus) {
            case IngredientStatus.Cooked:
                returnSprite = potSprite;
                break;
            case IngredientStatus.Fried:
                returnSprite = panSprite;
                break;
        }

        return returnSprite;
    }
}
