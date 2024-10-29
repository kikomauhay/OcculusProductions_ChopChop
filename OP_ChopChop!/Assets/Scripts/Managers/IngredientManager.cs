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
    public static List<GameObject> DisposedFoods { get; private set; }
    public static List<GameObject> OutsideIngredients { get; private set; }

    void Start()
    {
        DisposedFoods = new List<GameObject>();
        OutsideIngredients = new List<GameObject>();
    }

    public void Reset() 
    { 
        if (DisposedFoods.Count > 0) 
        {
            foreach (GameObject food in DisposedFoods) 
                Destroy(food);
        
            DisposedFoods.Clear();
        }
        
        if (OutsideIngredients.Count > 0)
        {
            foreach (GameObject food in OutsideIngredients)
                Destroy(food);

            OutsideIngredients.Clear();
        }
    }
}
