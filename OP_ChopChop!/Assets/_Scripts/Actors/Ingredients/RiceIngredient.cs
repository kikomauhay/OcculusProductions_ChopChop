using UnityEngine;
using System;

public class RiceIngredient : Ingredient
{
    [Header("Finished Dishes"), Tooltip("0 = salmon nigiri, 1 = tuna nigiri")]
    [SerializeField] GameObject[] _foodPrefabs; 

    [Header("Molding Attributes")]
    [SerializeField] MoldType _moldType;
    public Action<int> OnRiceMolded; // check with Moldable.cs

    protected override void Start() 
    {

        base.Start();
        OnRiceMolded += ChangeRiceMold;
        //_moldType = MoldType.UNMOLDED;
        //_ingredientType = IngredientType.RICE;
    }
    protected override void Reset()
    {
        base.Reset();
        OnRiceMolded -= ChangeRiceMold;
    }

    void OnTriggerEnter(Collider other) // combination of the food
    {
        /* -COLLISION RULES-
            * rice can't combine with another rice
            * food spawning only occurs when the ingredient is still FRESH
        */

        if (other.gameObject.name == name || 
            _moldType != MoldType.PERFECT) 
        {
            return;
        }

        Ingredient ing = other.GetComponent<Ingredient>();
        GameObject foodToSpawn = null;
        Food food = null;

        if (!ing.IsFresh) return;

        // gets the freshness rates of both ingredients before deleting them
        
        if (ing.IngredientType == IngredientType.SALMON)
        {
            SpawnManager.Instance.SpawnVFX(VFXType.SMOKE, transform, 1f);
            Destroy(gameObject);
            Destroy(other.gameObject);
            foodToSpawn = SpawnManager.Instance.SpawnObject(_foodPrefabs[0],
                                                            transform,
                                                            SpawnObjectType.FOOD);
        }
        else if (ing.IngredientType == IngredientType.TUNA)
        {
            SpawnManager.Instance.SpawnVFX(VFXType.SMOKE, transform, 1f);
            Destroy(gameObject);
            Destroy(other.gameObject);
            foodToSpawn = SpawnManager.Instance.SpawnObject(_foodPrefabs[1],
                                                            transform,
                                                            SpawnObjectType.FOOD);
        }
        else return;

        // sets up the food's score
        food = foodToSpawn.GetComponent<Food>();
        food.FoodScore = (FreshnessRate + ing.FreshnessRate) / 2f;
        food.FoodType = DishType.NIGIRI_SALMON; // only salmon for now (will add tuna later)
    }

    void ChangeRiceMold(int moldIndex) => _moldType = (MoldType)moldIndex;
}