using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OrderUI : MonoBehaviour {

    private Recipe orderRecipe;
    [SerializeField] private Transform ingredientUI_Icons;

    public void SetRecipe(Recipe recipe) {
        orderRecipe = recipe;
    }

    public void ShowIngredientsUI() {
        for (int i = 0; i < orderRecipe.ingredientInformations.Count; i++) {
            IngredientType ingredientType = orderRecipe.ingredientInformations[i].ingredientType;
            Sprite ingredientSprite = SpriteProvider.Instance.GetIngredientSprite(ingredientType);

            Transform ingredientUI_Icon = ingredientUI_Icons.GetChild(i);
            ingredientUI_Icon.GetComponent<Image>().sprite = ingredientSprite;
            ingredientUI_Icon.gameObject.SetActive(true);

            IngredientStatus ingredientStatus = orderRecipe.ingredientInformations[i].ingredientStatus;
            Sprite statusIndicatorSprite = SpriteProvider.Instance.GetDishSpriteForOrderUI(ingredientStatus);
            //Eðer sipariþteki malzemenin istenilen status'una piþirme tarzý bir iþlemle ulaþabiliyorsak
            //UI'da bunu gösteriyoruz.
            if (statusIndicatorSprite != null) {
                Transform statusIndicatorIcon = ingredientUI_Icon.GetChild(0);
                statusIndicatorIcon.GetComponent<Image>().sprite = statusIndicatorSprite;
                statusIndicatorIcon.gameObject.SetActive(true);
            }
        }
    }
}
