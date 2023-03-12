using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrderManager : Singleton<OrderManager> {

    private List<Recipe> availableRecipesInLevel;
    [SerializeField] private List<Transform> availableOrderUIs = new List<Transform>();

    [SerializeField] private List<OrderUI> currentOrderUIsList = new List<OrderUI>();

    private float minimumOrderSpawnDelay = 3f;
    private float maximumOrderSpawnDelay = 7f;

    private int correctDeliveredOrderCount = 0;
    public int CorrectDeliveredOrderCount { get { return correctDeliveredOrderCount; } }
    private int wrongDeliveredOrderCount = 0;
    public int WrongDeliveredOrderCount { get { return wrongDeliveredOrderCount; } }
    private int missDeliveredOrderCount = 0;
    public int MissDeliveredOrderCount { get { return missDeliveredOrderCount; } }

    private void Start() {
        availableRecipesInLevel = RecipeManager.Instance.Recipes.FindAll(x => x.isAvailableOnThisLevel);
    }

    public void StartGame() {
        StartCoroutine(SpawnFirstOrders());
    }

    private IEnumerator SpawnFirstOrders() {
        SpawnOrder();
        yield return new WaitForSeconds(Random.Range(1, 3));
        SpawnOrder();
        yield return new WaitForSeconds(Random.Range(1, 3));
        SpawnOrder();
        yield return new WaitForSeconds(Random.Range(1, 3));
        SpawnOrder();
    }

    //Places an order and displays it on UI.
    private void SpawnOrder() {
        if (GameController.Instance.IsGamePlaying) {
            //Pick a recipe along the available recipes at the current level.
            int randomNumber = Random.Range(0, availableRecipesInLevel.Count);
            Recipe randomRecipe = availableRecipesInLevel[randomNumber];

            Transform orderUI_Transform = availableOrderUIs[0];
            orderUI_Transform.SetAsLastSibling();
            orderUI_Transform.parent.gameObject.SetActive(true);
            availableOrderUIs.Remove(availableOrderUIs[0]);

            OrderUI orderUI = orderUI_Transform.GetComponent<OrderUI>();
            orderUI.SetRecipe(randomRecipe);
            orderUI.FadeIn();

            currentOrderUIsList.Add(orderUI);
        }
    }

    //Decides on whether the delivered plate matches with any order or not.
    public void CheckOrder(Plate deliveredPlate) {
        List<IngredientInformation> ingredientInformationList = new List<IngredientInformation>();
        foreach (Ingredient ingredient in deliveredPlate.CurrentIngredients) {
            IngredientInformation ingredientInformation = new IngredientInformation(ingredient);
            ingredientInformationList.Add(ingredientInformation);
        }

        bool ingredientsMatches = false;
        OrderUI deliveredOrderUI = null;
        foreach (OrderUI currentOrderUI in currentOrderUIsList) {
            Recipe recipeInCurrentOrders = currentOrderUI.OrderRecipe;
            if (deliveredPlate.CurrentIngredientQuantity != recipeInCurrentOrders.ingredientInformations.Count) continue;
            deliveredOrderUI = currentOrderUI;
            ingredientsMatches = true;
            foreach (IngredientInformation ingredientInformation in ingredientInformationList) {
                if (!recipeInCurrentOrders.ingredientInformations.Contains(ingredientInformation)) {
                    ingredientsMatches = false;
                    break;
                }
            }
        }

        if (ingredientsMatches) {
            CorrectDelivery(deliveredOrderUI);
        }
        else {
            wrongDeliveredOrderCount++;
            GameController.Instance.DeliveryPenalty();
        }
    }

    public void CorrectDelivery(OrderUI deliveredOrderUI) {
        correctDeliveredOrderCount++;

        currentOrderUIsList.Remove(deliveredOrderUI);
        deliveredOrderUI.FadeOut();
        availableOrderUIs.Add(deliveredOrderUI.transform);
        GameController.Instance.CorrectDelivery(deliveredOrderUI);
        StartCoroutine(SpawnOrderRandomly());
    }


    public void MissOrder(OrderUI missOrderUI, Transform AvailableOrderUI_Transform) {
        missDeliveredOrderCount++;

        currentOrderUIsList.Remove(missOrderUI);
        availableOrderUIs.Add(AvailableOrderUI_Transform);
        StartCoroutine(SpawnOrderRandomly());

        GameController.Instance.DeliveryPenalty();
    }

    private IEnumerator SpawnOrderRandomly() {
        float delay = Random.Range(minimumOrderSpawnDelay, maximumOrderSpawnDelay);
        yield return new WaitForSeconds(delay);
        SpawnOrder();
    }
}
