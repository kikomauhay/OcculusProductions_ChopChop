using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// IngredientManager is for foods that are OUTSIDE the fridge
/// 
/// The player has 3 options with the ingredient:
///     - use it
///     - dispose it (if it's rotten or something)
///     - put it back in the fridge 
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
    void Reset() 
    { 
        if (_trashCan.Count > 0) 
        {
            foreach (GameObject food in _trashCan) 
                Destroy(food);
        
            _trashCan.Clear();
        }
        
        if (Ingredients.Count > 0)
        {
            foreach (GameObject food in Ingredients)
                Destroy(food);

            Ingredients.Clear();
        }
    }
 
    public void TrashIngredient(GameObject food) {
        _trashCan.Add(food);
        food.GetComponent<Ingredient>().ThrowInTrash();
    }
}
