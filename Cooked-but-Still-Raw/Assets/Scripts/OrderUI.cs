using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OrderUI : MonoBehaviour {

    private Recipe orderRecipe;
    [SerializeField] private Transform ingredientUI_Icons;
    [SerializeField] private Image orderTimerUI;
    [SerializeField] private Gradient timerGradient;

    private float maxOrderTime;
    private float remainingOrderTime;

    public void SetRecipe(Recipe recipe) {
        orderRecipe = recipe;
        maxOrderTime = orderRecipe.recipePrepareTime;

        ShowIngredientsUI();
        StartCoroutine(OrderTimer());
    }

    //Display the ingridients of the ordered recipe.
    public void ShowIngredientsUI() {
        for (int i = 0; i < orderRecipe.ingredientInformations.Count; i++) {
            IngredientType ingredientType = orderRecipe.ingredientInformations[i].ingredientType;
            Sprite ingredientSprite = SpriteProvider.Instance.GetIngredientSprite(ingredientType);

            Transform ingredientUI_Icon = ingredientUI_Icons.GetChild(i);
            ingredientUI_Icon.GetComponent<Image>().sprite = ingredientSprite;
            ingredientUI_Icon.gameObject.SetActive(true);

            IngredientStatus ingredientStatus = orderRecipe.ingredientInformations[i].ingredientStatus;
            Sprite statusIndicatorSprite = SpriteProvider.Instance.GetDishSpriteForOrderUI(ingredientStatus);
            //If the desired status of an ingredient is reachable via a cooking process
            if (statusIndicatorSprite != null) {
                Transform statusIndicatorIcon = ingredientUI_Icon.GetChild(0);
                statusIndicatorIcon.GetComponent<Image>().sprite = statusIndicatorSprite;
                statusIndicatorIcon.gameObject.SetActive(true);
            }
        }
    }

    private IEnumerator OrderTimer() {
        remainingOrderTime = maxOrderTime;

        while (remainingOrderTime > 0) {
            remainingOrderTime -= Time.deltaTime;
            orderTimerUI.fillAmount = remainingOrderTime / maxOrderTime;
            orderTimerUI.color = timerGradient.Evaluate(orderTimerUI.fillAmount);
            yield return null;
        }

        OrderManager.Instance.MissOrder();
    }
}
