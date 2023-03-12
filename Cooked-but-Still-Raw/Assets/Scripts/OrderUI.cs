using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OrderUI : MonoBehaviour {

    private Recipe orderRecipe;
    public Recipe OrderRecipe { get { return orderRecipe; } }
    [SerializeField] private Image orderImage;
    [SerializeField] private Transform ingredientUI_Icons;
    [SerializeField] private List<Transform> statusIndicatorUI_Icons;
    [SerializeField] private Image orderTimerUI;
    [SerializeField] private Gradient timerGradient;
    [SerializeField] private Animator orderAnimator;

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
        orderImage.sprite = orderRecipe.recipeSprite;

        for (int i = 0; i < orderRecipe.ingredientInformations.Count; i++) {
            IngredientType ingredientType = orderRecipe.ingredientInformations[i].ingredientType;
            Sprite ingredientSprite = SpriteProvider.Instance.GetIngredientSprite(ingredientType);

            Transform ingredientUI_Icon = ingredientUI_Icons.GetChild(i);
            ingredientUI_Icon.GetChild(0).GetComponent<Image>().sprite = ingredientSprite;
            ingredientUI_Icon.gameObject.SetActive(true);

            IngredientStatus ingredientStatus = orderRecipe.ingredientInformations[i].ingredientStatus;
            Sprite statusIndicatorSprite = SpriteProvider.Instance.GetDishSpriteForOrderUI(ingredientStatus);
            //If the desired status of an ingredient is reachable via a cooking process
            if (statusIndicatorSprite != null) {
                Transform statusIndicatorIcon = statusIndicatorUI_Icons[i];
                statusIndicatorIcon.GetChild(1).GetComponent<Image>().sprite = statusIndicatorSprite;
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

        FadeOut();
        OrderManager.Instance.MissOrder(this, transform);
    }

    public void FadeIn() {
        orderAnimator.Play("OrderFadeIn");
    }

    public void FadeOut() {
        orderAnimator.Play("OrderFadeOut");
    }

    private void ResetUI() {
        foreach (Transform ingredientUI_Icon in ingredientUI_Icons) {
            ingredientUI_Icon.gameObject.SetActive(false);
        }

        foreach (Transform statusIndicatorUI_Icon in statusIndicatorUI_Icons) {
            statusIndicatorUI_Icon.gameObject.SetActive(false);
        }

        transform.parent.gameObject.SetActive(false);
    }
}
