using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PoolingManager : Singleton<PoolingManager>
{
    [SerializeField] private GameObject[] gameObjectsToBePooled;
    private Dictionary<IngredientType, int> ingredientTypeIndexPair;
    
    private List<List<GameObject>> gameObjectPool;
    private int poolSize = 3;


    public override void Awake() {
        base.Awake();
        InitializePool();
    }

    private void InitializePool() {
        gameObjectPool = new List<List<GameObject>>();
        ingredientTypeIndexPair = new Dictionary<IngredientType, int>();

        for (int i = 0; i < gameObjectsToBePooled.Length; i++) {
            List<GameObject> tempObjectPool = new List<GameObject>();
            for (int j = 0; j < poolSize; j++) {
                GameObject tempGameObject = Instantiate(gameObjectsToBePooled[i], transform);
                tempGameObject.SetActive(false);
                tempObjectPool.Add(tempGameObject);
            }
            gameObjectPool.Add(tempObjectPool);
            ingredientTypeIndexPair.Add(gameObjectPool[i][0].GetComponent<Ingredient>().IngredientType, i);
        }
    }

    public Item ActivateFromPool(Item itemToBeActivated) {
        IngredientType ingredientType = itemToBeActivated.GetComponent<Ingredient>().IngredientType;

        int index = ingredientTypeIndexPair[ingredientType];
        Item RefItemFromPool = null;
        foreach (GameObject gameObject in gameObjectPool[index]) {
            if (!gameObject.activeInHierarchy) {
                gameObject.SetActive(true);
                RefItemFromPool = gameObject.GetComponent<Item>();
                break;
            }
        }

        if (RefItemFromPool != null) {
            return RefItemFromPool;
        }
        else {
            GameObject newAddedGameObject = Instantiate(gameObjectsToBePooled[index]);
            Item newAddedItem = newAddedGameObject.GetComponent<Item>();
            gameObjectPool[ingredientTypeIndexPair[newAddedItem.GetComponent<Ingredient>().IngredientType]].Add(newAddedGameObject);
            return newAddedItem;
        }
    }

    public void DeActivateToPool(GameObject gameObject) {
        gameObject.transform.SetParent(transform);
        gameObject.SetActive(false);
    }
}
