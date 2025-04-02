using UnityEngine;
using System;

public class RiceIngredient : Ingredient
{
    [Header("Finished Dishes"), Tooltip("0 = salmon nigiri, 1 = tuna nigiri, 2 = salmon sashimi, 3 = tuna sashimi")]
    [SerializeField] GameObject[] _foodPrefabs; 

    [Header("Molding Attributes")]
    [SerializeField] MoldType _moldType;
    public Action<int> OnRiceMolded; // check with Moldable.cs

    protected override void Start() 
    {
        base.Start();
        OnRiceMolded += ChangeRiceMold;
    }
    protected override void OnDestroy()
    {
        base.OnDestroy();
        OnRiceMolded -= ChangeRiceMold;
    }

    void OnTriggerEnter(Collider other) // combination of the food
    {
        if (other.gameObject.name == name || 
            _moldType != MoldType.PERFECT) 
        {
            return;
        }

        Ingredient ing = other.GetComponent<Ingredient>();
        GameObject foodToSpawn = null;
        Food food = null;

        if (!ing.IsFresh) return;

        // sashimi spawning
        if (ing.SliceIndex == 3)
        {
            Dish dish = null;

            if (ing.IngredientType == IngredientType.SALMON)
            {
                Destroy(gameObject);
                Destroy(other.gameObject);

                SpawnManager.Instance.SpawnVFX(VFXType.SMOKE, transform, 1f);
                SoundManager.Instance.PlaySound("poof", SoundGroup.VFX);

                foodToSpawn = SpawnManager.Instance.SpawnObject(_foodPrefabs[2],
                                                                transform,
                                                                SpawnObjectType.DISH);
                dish = foodToSpawn.GetComponent<Dish>();
                dish.OrderDishType = DishType.SASHIMI_SALMON;
            }
            else if (ing.IngredientType == IngredientType.TUNA)
            {
                Destroy(gameObject);
                Destroy(other.gameObject);

                SpawnManager.Instance.SpawnVFX(VFXType.SMOKE, transform, 1f);
                SoundManager.Instance.PlaySound("poof", SoundGroup.VFX);

                foodToSpawn = SpawnManager.Instance.SpawnObject(_foodPrefabs[3],
                                                                transform,
                                                                SpawnObjectType.DISH);
               dish = foodToSpawn.GetComponent<Dish>();
               dish.OrderDishType = DishType.SASHIMI_TUNA;
            }

            // sets up the dish's score
            dish.DishScore = (FreshnessRate + ing.FreshnessRate) / 2f;
            return;
        }

        if (ing.SliceIndex != 4) return;

        // nigiri spawning
        if (ing.IngredientType == IngredientType.SALMON)
        {            
            Destroy(gameObject);
            Destroy(other.gameObject);

            SpawnManager.Instance.SpawnVFX(VFXType.SMOKE, transform, 1f);
            SoundManager.Instance.PlaySound("poof", SoundGroup.VFX);

            foodToSpawn = SpawnManager.Instance.SpawnObject(_foodPrefabs[0],
                                                            transform,
                                                            SpawnObjectType.FOOD);
            food = foodToSpawn.GetComponent<Food>();
            food.FoodType = DishType.NIGIRI_SALMON;
        }
        else if (ing.IngredientType == IngredientType.TUNA)
        {
            SpawnManager.Instance.SpawnVFX(VFXType.SMOKE, transform, 1f);
            SoundManager.Instance.PlaySound("poof", SoundGroup.VFX);

            Destroy(gameObject);
            Destroy(other.gameObject);
            
            foodToSpawn = SpawnManager.Instance.SpawnObject(_foodPrefabs[1],
                                                            transform,
                                                            SpawnObjectType.FOOD);
            food = foodToSpawn.GetComponent<Food>();
            food.FoodType = DishType.NIGIRI_TUNA;
        }
        else return;

        // sets up the food's score
        food.FoodScore = (FreshnessRate + ing.FreshnessRate) / 2f;
    }

    void ChangeRiceMold(int moldIndex) => _moldType = (MoldType)moldIndex;
}