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
    List<GameObject> _disposedFoods, _outsideIngredients;

    public List<GameObject> TrashCan { get => _disposedFoods; }
    public List<GameObject> Ingredients { get => _outsideIngredients; }

    void Start()
    {
        _disposedFoods = new List<GameObject>();
        _outsideIngredients = new List<GameObject>();
    }

    public void AddIngredient(GameObject food)
    {
        _outsideIngredients.Add(food);
    }

    public void ThrowFood(GameObject food) 
    {
        _disposedFoods.Add(food);
    } 

    public void Reset() // used for restarting the game 
    { 
        foreach (GameObject food in _disposedFoods) 
            Destroy(food);

        foreach (GameObject food in _outsideIngredients) 
            Destroy(food);
        
        _disposedFoods.Clear();
        _outsideIngredients.Clear();
    }
}
