using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Singleton class that stores the recipes
//Fully customizable via the Unity editor.
public class RecipeManager : Singleton<RecipeManager> {
    
    [SerializeField] private List<Recipe> recipes;
    public List<Recipe> Recipes { get { return recipes; } }

}
