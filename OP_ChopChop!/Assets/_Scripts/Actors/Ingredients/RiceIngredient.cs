using UnityEngine;
using System;

public class RiceIngredient : Ingredient
{
    [Header("Finished Dishes"), Tooltip("0 = salmon nigiri, 1 = tuna nigiri, 2 = salmon sashimi, 3 = tuna sashimi")]
    [SerializeField] private GameObject[] _foodPrefabs; 

    [Header("Molding Attributes")]
    [SerializeField] private MoldType _moldType;
    public Action<int> OnRiceMolded; // check with Moldable.cs
    private static bool TutorialDone { get; set; } 

    protected override void Start() 
    {
        base.Start();
        OnRiceMolded += ChangeRiceMold;

        if (!TutorialDone && _moldType == MoldType.PERFECT)
        {
            StartCoroutine(OnBoardingHandler.Instance.Onboarding06());
            TutorialDone = true;
        }
    }
    protected override void OnDestroy()
    {
        base.OnDestroy();
        OnRiceMolded -= ChangeRiceMold;
    }

    protected override void OnTriggerEnter(Collider other) // combination of the food
    {
        if (other.gameObject.name == name) return;
        
        if (_moldType != MoldType.PERFECT) return;

        Ingredient ing = other.gameObject.GetComponent<Ingredient>();

        if (ing != null)
        {
            // thinnest slice possible
            if (ing.SliceIndex == 4)
            {
                Destroy(gameObject);

                SpawnManager.Instance.SpawnVFX(VFXType.SMOKE, transform, 1f);   
                SoundManager.Instance.PlaySound("poof", SoundGroup.VFX);

                GameObject foodToSpawn = SpawnManager.Instance.SpawnObject(ing.IngredientType == IngredientType.SALMON ?
                                                                        _foodPrefabs[0] : _foodPrefabs[1],
                                                                        transform, SpawnObjectType.FOOD);
                // sets up the food's score
                Food food = foodToSpawn.GetComponent<Food>();
                food.FoodType = ing.IngredientType == IngredientType.SALMON ? DishType.NIGIRI_SALMON : DishType.NIGIRI_TUNA;
                food.FoodScore = (FreshnessRate + ing.FreshnessRate) / 2f;
                
                Destroy(other.gameObject);
            }
        }        
    }
    protected override void OnCollisionEnter(Collision other) => base.OnCollisionEnter(other);

    void ChangeRiceMold(int moldIndex) => _moldType = (MoldType)moldIndex;
}