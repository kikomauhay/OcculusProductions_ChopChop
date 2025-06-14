using System.Collections;
using UnityEngine;
using System;

/// <summary>
/// 
/// - Acts as the reworked version of Plate.cs.
/// - Rather than spawning new dishes each time, this just enables the right dish.
/// 
/// WHAT THIS SCRIPT SHOULD DO: 
///     - Enabling/disabling the type of food present in the plate.
///     - Changes the state of the dish when interacting with the environment.
///     - Contains the score needed for the Customer SR.
/// 
/// </summary>

[RequireComponent(typeof(BoxCollider))]
public class NEW_Dish : MonoBehaviour
{
#region Members

#region Properties

    public FoodCondition FoodCondition => _foodCondition;
    public DishPlatter DishPlatter => _dishPlatter;
    public bool HasFood => _hasFood;
    public float Score => _dishScore;

#endregion 
#region Private

    [Tooltip("0 = N. Salmon, 1 = N. Tuna, 2 = S. Salmon, 3 = S. Tuna")]
    [SerializeField] private GameObject[] _foodItems;

    [Header("Dish Attributes")]
    [SerializeField, Range(0f, 100f)] private float _dishScore;
    [SerializeField] private FoodCondition _foodCondition;
    [SerializeField] private DishPlatter _dishPlatter;
    [SerializeField] private bool _hasFood;

    [Header("Onboarding")]
    [SerializeField] private Material _highlightOSM;

    [Header("Debugging")]
    [SerializeField] private bool _isDevloperMode;

    private BoxCollider _collider;
    private NEW_Plate _plate;
    private const float DECAY_TIME = 30f;
    
#endregion

#endregion

#region Methods

#region Unity

    private void Awake()
    {
        _collider = GetComponent<BoxCollider>();
        _plate = GetComponent<NEW_Plate>();

        if (_foodItems.Length < 4)
            Debug.LogWarning($"Missing elements in {_foodItems}");

        foreach (GameObject item in _foodItems)
            item.SetActive(false);
    }
    private void Start() 
    {
        _dishPlatter = DishPlatter.EMPTY;
        _foodCondition = FoodCondition.CLEAN;

        _collider.enabled = true; 
        _hasFood = false;

        // Debug.Log($"{name} plated: {_isPlated}");
        Debug.Log($"{this} developer mode: {_isDevloperMode}");
    }
    private void OnTriggerEnter(Collider other)
    {
        if (_hasFood)
        {
            // Debug.LogError($"{name} already has food on the plate!");
            return;
        }

        Ingredient ing = other.gameObject.GetComponent<Ingredient>();
        UPD_Food food = other.gameObject.GetComponent<UPD_Food>();
        
        // nigiri creation
        if (food != null)
        {
            DoFoodCollision(food);
            Destroy(food.gameObject);
            SpawnManager.Instance.SpawnVFX(VFXType.SMOKE, other.transform, 1f);
            SoundManager.Instance.PlaySound("poof");
            return;
        }

        // sashimi creation
        if (ing != null) 
        {
            if (ing.IngredientType == IngredientType.RICE)
            {
                Debug.LogError("There is no option for onigiri yet!");
                return;
            }
            if (ing.SliceIndex != 3)
            {
                Debug.LogError("Not the proper slice!");
                return;
            }

            DoIngredientCollision(ing);
            Destroy(ing.gameObject);
            SpawnManager.Instance.SpawnVFX(VFXType.SMOKE, other.transform, 1f);
            SoundManager.Instance.PlaySound("poof");
        }         
    }

#region Testing

    private void Test()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1) && _isDevloperMode)
            SetActiveDish(DishPlatter.NIGIRI_SALMON);
        
        if (Input.GetKeyDown(KeyCode.Alpha2) && _isDevloperMode)
            SetActiveDish(DishPlatter.NIGIRI_TUNA);
        
        if (Input.GetKeyDown(KeyCode.Alpha3) && _isDevloperMode)
            SetActiveDish(DishPlatter.SASHIMI_SALMON);

        if (Input.GetKeyDown(KeyCode.Alpha4) && _isDevloperMode)
            SetActiveDish(DishPlatter.SASHIMI_TUNA);

        if (Input.GetKeyDown(KeyCode.Delete) && _isDevloperMode)
            DisableDish();
    }
    private void Update() => Test();

#endregion

#endregion

#region Public

    public void DisableDish()
    {
        if (!_hasFood)
        {
            Debug.LogWarning("There's no food currently!");
            return;
        }

        _dishScore = 0f;
        _hasFood = false;
        _collider.enabled = true;

        _foodItems[(int)_dishPlatter].GetComponent<NEW_Platter>().ResetMaterial();
        _foodItems[(int)_dishPlatter].SetActive(false);

        Debug.LogWarning($"{gameObject.name} is an empty plate again!");
    }
    public void SetFoodCondition(FoodCondition chosenState)
    {
        // guard clauses
        if (_foodCondition != FoodCondition.CLEAN)    
        {
            Debug.LogError("Dish is already bad!");
            return;
        }
        if (_foodCondition == chosenState)
        {
            Debug.LogError("You cannot set it to the same state again!");
            return;
        }

        _foodCondition = chosenState;

        // change material based on the state
        NEW_Platter platter = _foodItems[(int)_dishPlatter].GetComponent<NEW_Platter>();
        switch (chosenState)
        {
            case FoodCondition.CLEAN:  platter.ResetMaterial(); break;
            case FoodCondition.ROTTEN: platter.SetRotten();     break;
            case FoodCondition.MOLDY:  platter.SetMoldy();      break;
        }        
        Debug.LogWarning($"{name} has was set to {chosenState}");
    }

#endregion
#region Collision

    private void DoFoodCollision(UPD_Food food)
    {
        SetActiveDish(food.OrderType);

        switch (food.Condition)
        {
            case FoodCondition.MOLDY:
                _dishScore = 0f;
                _plate.SetDirty();
                SetFoodCondition(FoodCondition.MOLDY);
                // Debug.LogWarning($"{food.gameObject.name} is moldy!");
                break;
            
            case FoodCondition.ROTTEN:
                _dishScore = 0f;
                _plate.SetDirty();
                SetFoodCondition(FoodCondition.ROTTEN);
                // Debug.LogError($"{food.gameObject.name} is rotten!");
                SetFoodCondition(FoodCondition.ROTTEN);
                break;

            case FoodCondition.CLEAN:
                _dishScore = food.Score;
                // Debug.LogWarning($"{food.name} has been plated to {name}!");
                break;

            default: break;
        }   

        _hasFood = true;
        _collider.enabled = false;
        Debug.Log($"{name} has food: {_hasFood}");
    }
    private void DoIngredientCollision(Ingredient ing)
    { 
        switch (ing.IngredientType)
        {
            case IngredientType.SALMON:
                SetActiveDish(DishPlatter.SASHIMI_SALMON);
                break;

            case IngredientType.TUNA:
                SetActiveDish(DishPlatter.SASHIMI_TUNA);
                break;

            default: break;
        }

        // checks if the ingredient still fresh
        if (!ing.IsFresh)
        {
            _dishScore = 0f;
            _plate.SetDirty();
            SetFoodCondition(FoodCondition.MOLDY);
            // Debug.LogWarning($"{ing.name} is moldy!");
        }
        else 
        {
            _dishScore = ing.FreshnessRate;
            // Debug.LogWarning($"{ing.name} has been plated to {name}!");
        }

        _hasFood = true;
        _collider.enabled = false;
        // Debug.Log($"{name} has food: {_hasFood}");
    }

#endregion
#region Helpers

    private void SetActiveDish(DishPlatter activeDishChosen)
    {
        if (activeDishChosen == DishPlatter.EMPTY)
        {
            _hasFood = false;
            _collider.enabled = true;
            Debug.LogError("Default mode chosen!");
            return;
        }

        // enables the dish  
        _dishPlatter = activeDishChosen;
        _foodItems[(int)activeDishChosen].SetActive(true);
               
        // testing
        // Debug.LogWarning($"{activeDishChosen} is visible");
        // Debug.LogWarning($"{name} score: {_dishScore}");

        if (!_isDevloperMode) 
        {
            StartCoroutine(CO_StartRotting());
            // Debug.LogWarning($"{gameObject.name} is expiring!");    
        }
    }

#endregion

#endregion

#region Enumerators

    private IEnumerator CO_StartRotting()
    { 
        yield return new WaitForSeconds(DECAY_TIME);

        if (_foodCondition != FoodCondition.MOLDY)
        {
            SetFoodCondition(FoodCondition.ROTTEN);
            _plate.SetDirty();
        }
    }

#endregion
}   

#region Structure

[Serializable]
public struct FoodMaterials
{
    public readonly Material CleanMat => _materials[0];
    public readonly Material RottenMat => _materials[1];
    public readonly Material MoldyMat => _materials[2];

    [Tooltip("0 = clean, 1 = rotten, 2 = moldy")]
    [SerializeField] private Material[] _materials;
}

#endregion

#region Enumerations

    public enum FoodCondition
    {
        CLEAN  = 0, 
        ROTTEN = 1, 
        MOLDY  = 2
    }
    public enum DishPlatter 
    { 
        EMPTY         = -1,
        NIGIRI_SALMON  = 0, 
        NIGIRI_TUNA    = 1, 
        SASHIMI_SALMON = 2, 
        SASHIMI_TUNA   = 3 
    }

#endregion
