using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecipeManager : MonoBehaviour {

    public static RecipeManager Instance { get; private set; }

    [SerializeField] private List<Recipe> recipes;
    public List<Recipe> Recipes { get { return recipes; } }

    private void Awake() {
        Instance = this;
    }
}
