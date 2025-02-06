using UnityEngine;
using System;
using UnityEngine.Rendering.Universal;

public enum MoldType { UNMOLDED, GOOD, PERFECT, BAD }

public class RiceIngredient : Ingredient
{
    [Header("Finished Dishes"), Tooltip("Possible dishes the rice can combine with.")]
    [SerializeField] GameObject[] _foodPrefabs; // 0 = salmon, 1 = tuna

    [Header("VFX Settings")]
    [SerializeField] GameObject _smokeVFX;
    [SerializeField] float _vfxDestroyTime; // was initially at 2f  

    [Header("Molding Attributes")]
    [SerializeField] MoldType _moldType;
    public Action<int> OnRiceMolded;

    protected override void Start() 
    {
        base.Start();
        OnRiceMolded += ChangeRiceMold;
        _moldType = MoldType.UNMOLDED;
    }

    void Reset() => OnRiceMolded -= ChangeRiceMold;

    void OnTriggerEnter(Collider other) // combination of the food
    {
        // prevents mixing the same ingredients together
        if (other.gameObject.name == name) return;

        // only perfectly-molded rice is allowed
        if (_moldType != MoldType.PERFECT) return;

        Ingredient ing = other.GetComponent<Ingredient>();
        Vector3 pos = transform.position;
        Quaternion rot = transform.rotation;

        // gets the freshness rates of both ingredients before deleting them
        Destroy(gameObject);
        Destroy(other.gameObject);
        GameManager.Instance.OnDishCreated?.Invoke(FreshnessRate, ing.FreshnessRate);
        SpawnManager.Instance.OnSpawnVFX?.Invoke(VFXType.SMOKE, pos, rot);
        
        // sets up the food's appearance & value
        if (ing.GetComponent<FishIngredient>().SliceType == SliceType.THIN)
        {
            GameObject foodToSpawn;
            Food food = null;
            
            if (ing.Type == IngredientType.SALMON)
            {
                foodToSpawn = Instantiate(_foodPrefabs[0], pos, rot);
                food = foodToSpawn.GetComponent<Food>();

                // sets the food stats in the Food that the Dish will use
                //food.OrderType = OrderType.Nigiri_Salmon;
            }
            else if (ing.Type == IngredientType.TUNA)
            {
                foodToSpawn = Instantiate(_foodPrefabs[1], pos, rot);
                food = foodToSpawn.GetComponent<Food>();

                // sets the food stats in the Food that the Dish will use
                //food.OrderType = OrderType.Nigiri_Salmon; // change to tuna later
            }

            food.FoodScore = (FreshnessRate + ing.FreshnessRate) / 2f;
        }

        //// different types of nigiri can be made
        //if (ing.Type == IngredientType.SALMON &&
        //    ing.GetComponent<SalmonIngredient>().SliceType == SliceType.THIN)
        //{
        //    food = Instantiate(_foodPrefabs[0], pos, rot); // salmon nigiri
        //    nigiri = food.GetComponent<NigiriFood>();



        //}
        //else if (ing.Type == IngredientType.TUNA &&
        //         ing.GetComponent<TunaIngredient>().SliceType == SliceType.THIN)
        //{

        //    food = Instantiate(_foodPrefabs[1], pos, rot); // tuna nigiri
        //    nigiri = food.GetComponent<NigiriFood>();
        //}

        //// gets the average of the two ingredients and adds them as the food score
        //nigiri.FoodScore = (FreshnessRate + ing.FreshnessRate) / 2f;
    }

    void ChangeRiceMold(int moldIndex) => _moldType = (MoldType)moldIndex;
}