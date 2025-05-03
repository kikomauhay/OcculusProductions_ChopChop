using System.Collections;
using UnityEngine;
using System;
using UnityEngine.Experimental.Rendering;

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

    public GameObject ActiveDish => _activeDish;
    public DishState State => _dishState;
    public DishType Type => _dishType;
    public bool IsPlated => _isPlated;
    public float Score => _dishScore;

#endregion 

#region Private

    [Tooltip("0 = N. Salmon, 1 = N. Tuna, 2 = S. Salmon, 3 = S. Tuna")]
    [SerializeField] private GameObject[] _foodItems;

    [Header("Dish Attributes")]
    [SerializeField, Range(0f, 100f)] private float _dishScore;
    [SerializeField] private DishType _dishType;
    [SerializeField] private DishState _dishState;
    [SerializeField] private bool _isPlated;

    [Header("Onboarding")]
    [SerializeField] private Material _highlightOSM;

    [Header("Debugging")]
    [SerializeField] private bool _isDevloperMode;

    private GameObject _activeDish;
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

        Debug.Log($"{gameObject.name} developer mode: {_isDevloperMode}");
    }
    private void Start() 
    {
        _dishType = DishType.DEFAULT;
        _dishState = DishState.CLEAN;

        _collider.enabled = true; 
        _isPlated = false;
        
        foreach (GameObject item in _foodItems)
            item.SetActive(false);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (_isPlated)
        {
            Debug.LogError("Dish is already plated!");
            return;
        }

        Ingredient ing = other.gameObject.GetComponent<Ingredient>();
        Food food = other.gameObject.GetComponent<Food>();

        if (ing != null) // sashimi creation
        {
            DoIngredientCollision(ing);
            return;
        }
        if (food != null) // nigiri creation
        {
            DoFoodCollision(food);
            return;
        }
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
            SetActiveDish(DishType.NIGIRI_SALMON);
        
        if (Input.GetKeyDown(KeyCode.Alpha2))
            SetActiveDish(DishType.NIGIRI_TUNA);
        
        if (Input.GetKeyDown(KeyCode.Alpha3))
            SetActiveDish(DishType.SASHIMI_SALMON);

        if (Input.GetKeyDown(KeyCode.Alpha4))
            SetActiveDish(DishType.SASHIMI_TUNA);

        if (Input.GetKeyDown(KeyCode.Delete)) 
            DisableDish();
    }

#endregion

#region Enumerators

    private IEnumerator Expire()
    { 
        yield return new WaitForSeconds(DECAY_TIME);

        if (_dishState != DishState.MOLDY)
        {
            _dishState = DishState.ROTTEN;
            _plate.Contaminate();
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
    public void SetState(DishState chosenState)
    {
        // guard clauses
        if (!_isPlated)
        {
            Debug.LogError("Dish is not plated!");
            return;
        }
        if (_dishState != DishState.CLEAN)    
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
            case DishState.CLEAN:  platter.ResetMaterial(); break;
            case DishState.ROTTEN: platter.SetRotten();     break;
            case DishState.MOLDY:  platter.SetMoldy();      break;
        }        
    }

#endregion

#region Helpers

    private void SetActiveDish(DishType dishInput)
    {
        // guard clauses
        if (_isPlated)
        {
            Debug.LogError("Dish is already plated!");
            return;
        }
        if (dishInput == DishType.DEFAULT)
        {
            Debug.LogError("Default mode chosen!");
            return;
        }

        // sets the active dish  
        _dishType = dishInput;
        _foodItems[(int)dishInput].SetActive(true);
        
        _isPlated = true;
        _collider.enabled = false;        
        Debug.LogWarning($"{dishInput} is visible");

        if (!_isDevloperMode) 
            StartCoroutine(Expire());
    }
    

#region Collision

    private void DoFoodCollision(Food food)
    {
        _isPlated = true;
        SetActiveDish(food.FoodType);

        if (food.IsContaminated)
        {
            Debug.LogWarning($"{food.gameObject.name} is moldy!");
            _dishScore = 0f;
            
            SetState(DishState.MOLDY);
            return;
        }
        if (food.IsExpired)
        {
            Debug.LogError($"{food.gameObject.name} is rotten!");
            _dishScore = 0f;

            SetState(DishState.ROTTEN);
            return;
        }

        // no need to set it to CLEAN anymore since it's CLEAN by default
        _dishScore = food.FoodScore;
    }
    private void DoIngredientCollision(Ingredient ing)
    {
        _isPlated = true;
        // do a similar thing to food collision

        
    }

#endregion
#endregion
}   

#region Structs

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

#region Enums

    public enum DishState
    {
        CLEAN  = 0, 
        ROTTEN = 1, 
        MOLDY  = 2
    }
    public enum DishType 
    { 
        DEFAULT       = -1, // nothing is on the plate
        NIGIRI_SALMON  = 0, 
        NIGIRI_TUNA    = 1, 
        SASHIMI_SALMON = 2, 
        SASHIMI_TUNA   = 3 
    }

#endregion
