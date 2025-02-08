using System.Collections.Generic;
using UnityEngine;

/// <summary> -WHAT DOES THIS SCRIPT DO-
/// 
/// IngredientManager is basically a List with extra features
///
/// -FOOD NAMING SCHEME-
///     Ingredient -> combines with another ingredient to make the unplated food
///     Food -> UNPLATED prefab that has the average score of the 2 ingredients
///     Dish -> PLATED prefab that also has the score and needs to be served to the customer  
/// 
/// The player has 3 options with an ingredient:
///     1. use it
///     2. dispose it (if it's expireds)
///     3. put it back in the fridge (as long as it's not contaminated) 
///     
/// </summary>

public class IngredientManager : Singleton<IngredientManager>
{
    public List<GameObject> Ingredients => _ingredients;
    public int GarbageCount => _trashCan.Count;
    
    List<GameObject> _trashCan, _ingredients;

    protected override void Awake() => base.Awake(); 
    protected override void OnApplicationQuit() => base.OnApplicationQuit(); 

    void Start()
    {
        _ingredients = new List<GameObject>();
        _trashCan = new List<GameObject>();
    }
 
    public void TrashIngredient(GameObject food) {
        _trashCan.Add(food);
        food.GetComponent<Ingredient>().ThrowInTrash();
    }
}
