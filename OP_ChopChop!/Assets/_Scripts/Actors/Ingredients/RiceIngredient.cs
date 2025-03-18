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
        if (other.gameObject.name == name) return;

        if (_moldType != MoldType.PERFECT) return;

        Ingredient ing = other.GetComponent<Ingredient>();
        Vector3 pos = transform.position;
        Quaternion rot = transform.rotation;

        // gets the freshness rates of both ingredients before deleting them
        
        // only nigiris for now (makis will be added on SPARK)
        if (ing.GetComponent<FishIngredient>().SliceType == SliceType.THIN)
        {
            GameObject foodToSpawn;
            Food food = null;

            if (ing.IngredientType == IngredientType.SALMON)
            {
                SpawnManager.Instance.SpawnVFX(VFXType.SMOKE, transform);
                Destroy(gameObject);
                Destroy(other.gameObject);
                foodToSpawn = Instantiate(_foodPrefabs[0], pos, rot);
            }
            
            else if (ing.IngredientType == IngredientType.TUNA)
            {
                SpawnManager.Instance.SpawnVFX(VFXType.SMOKE, transform);
                Destroy(gameObject);
                Destroy(other.gameObject);
                foodToSpawn = Instantiate(_foodPrefabs[1], pos, rot);
            }

            else return;

            // sets up the food's score
            food = foodToSpawn.GetComponent<Food>();
            food.FoodScore = (FreshnessRate + ing.FreshnessRate) / 2f;
            food.FoodType = DishType.NIGIRI_SALMON; // only salmon for now (will add tuna later)
        }
    }

    void ChangeRiceMold(int moldIndex) => _moldType = (MoldType)moldIndex;
    // change to incrementing index so you don't need to get a parameter
}