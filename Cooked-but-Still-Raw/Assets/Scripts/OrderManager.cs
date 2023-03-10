using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrderManager : MonoBehaviour {

    public static OrderManager Instance { get; private set; }

    private List<Recipe> availableRecipesInLevel;
    [SerializeField] protected Transform OrdersUI;

    [SerializeField] private List<Recipe> currentOrdersRecipeList = new List<Recipe>();

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

    //Places an order and displays it on UI.
    private void SpawnOrder() {
        //Pick a recipe along the available recipes at the current level.
        int randomNumber = Random.Range(0, availableRecipesInLevel.Count);
        Recipe randomRecipe = availableRecipesInLevel[randomNumber];

        currentOrdersRecipeList.Add(randomRecipe);

        Transform orderUI_Transform = OrdersUI.GetChild(currentOrdersRecipeList.Count - 1).transform;
        orderUI_Transform.gameObject.SetActive(true);

        OrderUI orderUI = orderUI_Transform.GetComponent<OrderUI>();
        orderUI.SetRecipe(randomRecipe);
    }

    //Decides on whether the delivered plate matches with any order or not.
    public void CheckOrder(Plate deliveredPlate) {
        List<IngredientInformation> ingredientInformationList = new List<IngredientInformation>();
        foreach (Ingredient ingredient in deliveredPlate.CurrentIngredients) {
            IngredientInformation ingredientInformation = new IngredientInformation(ingredient);
            ingredientInformationList.Add(ingredientInformation);
        }

        bool ingredientsMatches = false;
        Recipe deliveredRecipe = new Recipe();
        foreach (Recipe recipeInCurrentOrders in currentOrdersRecipeList) {
            if (deliveredPlate.CurrentIngredientQuantity != recipeInCurrentOrders.ingredientInformations.Count) continue;
            deliveredRecipe = recipeInCurrentOrders;
            ingredientsMatches = true;
            foreach (IngredientInformation ingredientInformation in ingredientInformationList) {
                if (!recipeInCurrentOrders.ingredientInformations.Contains(ingredientInformation)) {
                    ingredientsMatches = false;
                    break;
                }
            }
        }

        if (ingredientsMatches) {
            CorrectDelivery(deliveredRecipe);
        }
        else {
            WrongDelivery();
        }
    }

    private void CorrectDelivery(Recipe deliveredRecipe) {
        Debug.Log(deliveredRecipe.recipePrize);
    }

    private void WrongDelivery() {

    }


    public void MissOrder(Recipe missedRecipe) {

    }
}
