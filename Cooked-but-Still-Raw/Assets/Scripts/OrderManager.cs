using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrderManager : MonoBehaviour {

    public static OrderManager Instance { get; private set; }

    private List<Recipe> availableRecipesInLevel;
    [SerializeField] protected Transform OrdersUI;

    private List<Recipe> currentOrdersRecipeList = new List<Recipe>();

    private void Awake() {
        Instance = this;
    }

    private void Start() {
        availableRecipesInLevel = RecipeManager.Instance.Recipes.FindAll(x => x.isAvailableOnThisLevel);
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.T)) {
            SpawnOrder();
        }
    }

    private void SpawnOrder() {
        int randomNumber = Random.Range(0, availableRecipesInLevel.Count);
        Recipe randomRecipe = availableRecipesInLevel[randomNumber];

        currentOrdersRecipeList.Add(randomRecipe);

        Transform orderUI_Transform = OrdersUI.GetChild(currentOrdersRecipeList.Count - 1).transform;
        orderUI_Transform.gameObject.SetActive(true);

        OrderUI orderUI = orderUI_Transform.GetComponent<OrderUI>();
        orderUI.SetRecipe(randomRecipe);
        orderUI.ShowIngredientsUI();
    }

    public void CheckOrder(Plate deliveredPlate) {
        List<IngredientInformation> ingredientInformationList = new List<IngredientInformation>();
        foreach (Ingredient ingredient in deliveredPlate.CurrentIngredients) {
            IngredientInformation ingredientInformation = new IngredientInformation(ingredient);
            ingredientInformationList.Add(ingredientInformation);
        }

        bool ingredientsMatches = false;
        foreach (Recipe recipeInCurrentOrders in currentOrdersRecipeList) {
            if (deliveredPlate.CurrentIngredientQuantity != recipeInCurrentOrders.ingredientInformations.Count) continue;

            ingredientsMatches = true;
            foreach (IngredientInformation ingredientInformation in ingredientInformationList) {
                if (!recipeInCurrentOrders.ingredientInformations.Contains(ingredientInformation)) {
                    ingredientsMatches = false;
                    break;
                }
            }
        }

        if (ingredientsMatches) {
            Debug.Log("correct plate");
        }
        else {
            Debug.Log("wrong plate");
        }
    }
}
