using UnityEngine;
using System;

public enum MoldType { UNMOLDED, GOOD, PERFECT, BAD }

public class RiceIngredient : Ingredient
{
    [Header("Finished Dishes"), Tooltip("Possible dishes the rice can combine with.")]
    [SerializeField] GameObject[] _foodPrefabs; // 0 = salmon nigiri, 1 = tuna nigiri

    [Header("VFX Settings")]
    [SerializeField] float _vfxDestroyTime; // was initially at 2f  

    [Header("Molding Attributes")]
    [SerializeField] MoldType _moldType;
    public Action<int> OnRiceMolded; // check with Moldable.cs

    protected override void Start() 
    {
        base.Start();

        OnRiceMolded += ChangeRiceMold;
        _moldType = MoldType.UNMOLDED;
        _ingredientType = IngredientType.RICE;
    }

    void Reset() => OnRiceMolded -= ChangeRiceMold;

    void OnTriggerEnter(Collider other) // combination of the food
    {
        if (other.gameObject.name == name) return;

        if (_moldType != MoldType.PERFECT) return;

        Ingredient ing = other.GetComponent<Ingredient>();
        Vector3 pos = transform.position;
        Quaternion rot = transform.rotation;

        // gets the freshness rates of both ingredients before deleting them
        Destroy(gameObject);
        Destroy(other.gameObject);
        SpawnManager.Instance.OnSpawnVFX?.Invoke(VFXType.SMOKE, pos, rot);
        
        // only nigiris for now (makis will be added after midterms)
        if (ing.GetComponent<FishIngredient>().SliceType == SliceType.THIN)
        {
            GameObject foodToSpawn;
            Food food = null;

            if (ing.IngredientType == IngredientType.SALMON)
                foodToSpawn = Instantiate(_foodPrefabs[0], pos, rot);
            
            else if (ing.IngredientType == IngredientType.TUNA)
                foodToSpawn = Instantiate(_foodPrefabs[1], pos, rot);
            
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