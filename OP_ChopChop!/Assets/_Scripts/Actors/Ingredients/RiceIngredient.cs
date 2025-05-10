using UnityEngine;
using System;

public class RiceIngredient : Ingredient
{
#region Members

    public Action<int> OnRiceMolded;

#region SerializeField

    [Header("Food Prefabs"), Tooltip("0 = salmon nigiri, 1 = tuna nigiri")]
    [SerializeField] private GameObject[] _foodPrefabs; // might add more dishes soon 

    [Header("Molding Attributes")]
    [SerializeField] private MoldType _moldType;

#endregion

    private static bool _tutorialDone;

#endregion

#region Methods

    protected override void Start() 
    {
        base.Start();
        OnRiceMolded += ChangeRiceMold;

        if (!_tutorialDone && _moldType == MoldType.PERFECT)
        {
            StartCoroutine(OnBoardingHandler.Instance.Onboarding06());
            _tutorialDone = true;
        }
    }
    protected override void OnDestroy()
    {
        base.OnDestroy();
        OnRiceMolded -= ChangeRiceMold;
    }
    protected override void OnTriggerEnter(Collider other)
    {
        Ingredient ing = other.gameObject.GetComponent<Ingredient>();

        // guard clauses
        if (other.gameObject.name == name) 
        {
            Debug.LogError("Cannot combine with the same ingredient!");
            return;
        }
        if (_moldType != MoldType.PERFECT) 
        {
            Debug.LogError($"Mold type of {name} is not perfect!");
            return;
        }       
        if (ing == null)
        {
            Debug.LogError($"{other.name} is not an ingredient!");
            return;
        }
        if (ing.SliceIndex != 4)
        {
            Debug.LogError($"{other.name} is not the correct slice!");
            return;
        }
        if (ing.IngredientType == IngredientType.RICE)
        {
            Debug.LogError($"{other.name} is a rice ingredient!");
            return;
        }

        // UX to make food combination more appealing 
        SpawnManager.Instance.SpawnVFX(VFXType.SMOKE, transform, 1f);
        SoundManager.Instance.PlaySound("poof");

        // more readable way which prefab is being spawned
        GameObject chosenFood = ing.IngredientType == IngredientType.SALMON ? 
                                _foodPrefabs[0] : _foodPrefabs[1];

        // spawns the proper food based on the collided object's ingredient type 
        GameObject foodToSpawn = SpawnManager.Instance.SpawnObject(chosenFood,transform, 
                                                                   SpawnObjectType.FOOD);
        // sets up the food's score
        foodToSpawn.GetComponent<UPD_Food>().SetFoodScore((FreshnessRate + ing.FreshnessRate) / 2f);
        
        Destroy(gameObject);
        Destroy(other.gameObject);
    }

    protected override void ChangeMaterial()
    {
        _rend.materials = IngredientState == IngredientState.DEFAULT ? 
                          new Material[] { _materials[1], _dirtyOSM } : 
                          new Material[] { _materials[0] }; 
  
    }

    private void ChangeRiceMold(int moldIndex) => _moldType = (MoldType)moldIndex;

#endregion 
}