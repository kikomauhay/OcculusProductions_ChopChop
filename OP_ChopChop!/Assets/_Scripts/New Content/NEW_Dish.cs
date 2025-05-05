using System.Collections;
using UnityEngine;
using System;
using UnityEditor;

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

public class NEW_Dish : MonoBehaviour
{
#region Properties

    public FoodCondition State => _dishState;
    public DishOrder Type => _dishType;
    public bool IsPlated => _isPlated;
    public float Score => _dishScore;

#endregion 

#region Private

    [Tooltip("0 = N. Salmon, 1 = N. Tuna, 2 = S. Salmon, 3 = S. Tuna")]
    [SerializeField] private GameObject[] _foodItems;

    [Header("Dish Attributes")]
    [SerializeField, Range(0f, 100f)] private float _dishScore;
    [SerializeField] private FoodCondition _dishState;
    [SerializeField] private DishOrder _dishType;
    [SerializeField] private bool _isPlated;

    [Header("Onboarding")]
    [SerializeField] private Material _highlightOSM;

    [Header("Debugging")]
    [SerializeField] private bool _isDevloperMode;

    private Collider _collider;
    private NEW_Plate _plate;
    private const float DECAY_TIME = 30f;

#endregion

#region Unity

    private void Awake()
    {
        _collider = GetComponent<Collider>();
        _plate = GetComponent<NEW_Plate>();

        if (_foodItems.Length < 4)
            Debug.LogWarning($"Missing elements in {_foodItems}");

        // Debug.Log($"{gameObject.name} developer mode: {_isDevloperMode}");
    }
    private void Start() 
    {
        _dishType = DishOrder.EMPTY;
        _dishState = FoodCondition.CLEAN;

        _collider.enabled = true; 
        _isPlated = false;
        
        foreach (GameObject item in _foodItems)
            item.SetActive(false);

        Debug.Log($"Dish plated: {_isPlated}");
    }
    private void OnTriggerEnter(Collider other)
    {
        if (_isPlated)
        {
            Debug.LogError($"{gameObject.name} is already plated!");
            return;
        }

        Ingredient ing = other.gameObject.GetComponent<Ingredient>();
        UPD_Food food = other.gameObject.GetComponent<UPD_Food>();

        // set the proper state of the plate
        _isPlated = true;
        _collider.enabled = false; 

        // sashimi creation
        if (ing != null) 
            DoIngredientCollision(ing);
        
        // nigiri creation
        else if (food != null)
            DoFoodCollision(food);

        // finishing effects for the dish
        Destroy(other.gameObject);
        SpawnManager.Instance.SpawnVFX(VFXType.SMOKE, other.transform, 1f);
        SoundManager.Instance.PlaySound("poof", SoundGroup.VFX);
    }

#region Testing

    private void Update() 
    {
        test();
    }
    void test()
    {
        if (!_isDevloperMode) 
        {
            Debug.LogError("Not in developer mode!");            
            return;
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
            SetActiveDish(DishOrder.NIGIRI_SALMON);
        
        if (Input.GetKeyDown(KeyCode.Alpha2))
            SetActiveDish(DishOrder.NIGIRI_TUNA);
        
        if (Input.GetKeyDown(KeyCode.Alpha3))
            SetActiveDish(DishOrder.SASHIMI_SALMON);

        if (Input.GetKeyDown(KeyCode.Alpha4))
            SetActiveDish(DishOrder.SASHIMI_TUNA);

        if (Input.GetKeyDown(KeyCode.Delete)) 
            DisableDish();
    }

#endregion

#region Enumerators

    private IEnumerator Expire()
    { 
        yield return new WaitForSeconds(DECAY_TIME);

        if (_dishState != FoodCondition.MOLDY)
        {
            _dishState = FoodCondition.ROTTEN;
            _plate.SetDirty();
        }
    }

#endregion

#endregion

#region Public

    public void DisableDish()
    {
        _dishScore = 0f;
        _isPlated = false;
        _collider.enabled = true;

        _foodItems[(int)_dishType].GetComponent<NEW_Platter>().ResetMaterial();
        _foodItems[(int)_dishType].SetActive(false);

        Debug.LogWarning($"{gameObject.name} is an empty plate again!");
    }
    public void SetCondition(FoodCondition chosenState)
    {
        // guard clauses
        if (!_isPlated)
        {
            Debug.LogError("Dish is not plated!");
            return;
        }
        if (_dishState != FoodCondition.CLEAN)    
        {
            Debug.LogError("Dish is already bad!");
            return;
        }
        if (_dishState == chosenState)
        {
            Debug.LogError("You cannot set it to the same state again!");
            return;
        }

        _dishState = chosenState;

        // change material based on the state
        NEW_Platter platter = _foodItems[(int)_dishType].GetComponent<NEW_Platter>();
        switch (chosenState)
        {
            case FoodCondition.CLEAN:  platter.ResetMaterial(); break;
            case FoodCondition.ROTTEN: platter.SetRotten();     break;
            case FoodCondition.MOLDY:  platter.SetMoldy();      break;
        }        
    }

#endregion

#region Helpers

    private void SetActiveDish(DishOrder dishInput)
    {
        if (dishInput == DishOrder.EMPTY)
        {
            _isPlated = false;
            _collider.enabled = true;
            Debug.LogError("Default mode chosen!");
            return;
        }

        // enables the dish  
        _dishType = dishInput;
        _foodItems[(int)dishInput].SetActive(true);
               
        // testing
        Debug.LogWarning($"{dishInput} is visible");
        Debug.Log($"{gameObject.name} score: {_dishScore}");

        if (!_isDevloperMode) 
        {
            StartCoroutine(Expire());
            Debug.LogWarning($"{gameObject.name} is expiring!");    
        }
    }
    

#region Collision

    private void DoFoodCollision(UPD_Food food)
    {
        SetActiveDish(food.OrderType);

        switch (food.Condition)
        {
            case FoodCondition.MOLDY:
                _dishScore = 0f;
                _plate.SetDirty();
                SetCondition(FoodCondition.MOLDY);
                Debug.LogWarning($"{food.gameObject.name} is moldy!");
                break;
            
            case FoodCondition.ROTTEN:
                _dishScore = 0f;
                _plate.SetDirty();
                SetCondition(FoodCondition.ROTTEN);
                Debug.LogError($"{food.gameObject.name} is rotten!");
                break;

            case FoodCondition.CLEAN:
                _dishScore = food.Score;
                Debug.LogWarning($"{food.gameObject.name} has been plated to {gameObject.name}!");
                break;

            default: break;
        }
    }
    private void DoIngredientCollision(Ingredient ing)
    {
        _isPlated = true;
        // do a similar thing to food collision

        
    }

#endregion
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
    public enum DishOrder 
    { 
        EMPTY         = -1,
        NIGIRI_SALMON  = 0, 
        NIGIRI_TUNA    = 1, 
        SASHIMI_SALMON = 2, 
        SASHIMI_TUNA   = 3 
    }

#endregion
