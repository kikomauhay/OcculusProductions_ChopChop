using System.Collections;
using UnityEngine;
using System;

/// <summary>
/// 
/// - Acts as the reworked version of Plate.cs
/// - This script enables/disables the food present in the plate
/// - 
/// 
/// </summary>

public class NEW_Dish : MonoBehaviour
{
#region Properties

    public DishType Type => _dishType;
    public DishState State => _dishState;
    public float Score => _dishScore;
    public bool IsPlated => _isPlated;

#endregion 

#region Private

    [Header("Food Components"), Tooltip("0 = N. Salmon, 1 = N. Tuna, 2 = S. Salmon, 3 = S. Tuna")]
    [SerializeField] private GameObject[] _foodItems;
    [SerializeField] private NEW_Platter[] _platters;
    
    [Header("Dish Attributes")]
    [SerializeField, Range(0f, 100f)] private float _dishScore;
    [SerializeField] private DishType _dishType;
    [SerializeField] private DishState _dishState;
    [SerializeField] private bool _isPlated;

    [Header("Onboarding")]
    [SerializeField] private Material _highlightOSM;

    [Header("Debugging")]
    [SerializeField] private bool _isDevloperMode;

    private Collider _collider;
    private Renderer _rend;
    private const float DECAY_TIME = 30f;

#endregion

#region Unity

    private void Awake()
    {
        _collider = GetComponent<Collider>();

        if (_foodItems.Length < 4)
            Debug.LogWarning($"Missing elements in {_foodItems}");

        if (_platters.Length < 4)
            Debug.LogWarning($"Missing elements in {_platters}");
    }
    private void Start() 
    {
        _dishType = DishType.DEFAULT;
        _dishState = DishState.CLEAN;

        _collider.enabled = true; 
        _isPlated = false;
        
        DisableFoodItems();
    }
    private void OnTriggerEnter(Collider other)
    {
        Ingredient ing = other.gameObject.GetComponent<Ingredient>();
        Food food = other.gameObject.GetComponent<Food>();

        // case for sashimis
        if (ing != null)
            DoIngredientCollision(ing);

        // case for nigiris
        if (food != null)
            DoFoodCollision(food);
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
            SetPlatter(DishType.NIGIRI_SALMON);
        
        if (Input.GetKeyDown(KeyCode.Alpha2))
            SetPlatter(DishType.NIGIRI_TUNA);
        
        if (Input.GetKeyDown(KeyCode.Alpha3))
            SetPlatter(DishType.SASHIMI_SALMON);

        if (Input.GetKeyDown(KeyCode.Alpha4))
            SetPlatter(DishType.SASHIMI_TUNA);

        if (Input.GetKeyDown(KeyCode.Delete)) 
            ResetDish();
    }

#endregion

#region Enumerators

    private IEnumerator Expire()
    { 
        yield return new WaitForSeconds(DECAY_TIME);

        if (_dishState != DishState.MOLDY)
            _dishState = DishState.ROTTEN;
    }

#endregion

#endregion

#region Public

    public void SetPlatter(DishType dishInput)
    {
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

        // sets the dish 
        _dishType = dishInput;
        _foodItems[(int)dishInput].SetActive(true);
        _dishScore = 100f; // test
        
        _isPlated = true;
        _collider.enabled = false;

        StartCoroutine(Expire());
    }
    public void SetState(DishState chosenState)
    {
        if (_dishState != DishState.CLEAN)    
        {
            Debug.LogError("Dish is already bad!");
            return;
        }
        if (_dishState == chosenState)
        {
            Debug.LogError("You cannot set the same state again!");
            return;
        }

        _dishState = chosenState;

        switch (chosenState)
        {
            case DishState.CLEAN:
                
            break;

            case DishState.ROTTEN:
            break;

            case DishState.MOLDY: 
            
            break;
        } 
        
    }

#endregion

#region Helpers

    private void DisableFoodItems()
    {
        for (int i = 0; i < _foodItems.Length; i++)
        {
            _foodItems[i].SetActive(false);
            _platters[i].ResetMaterial();
        }            
    }
    private void ResetDish()
    {
        DisableFoodItems();

        _dishScore = 0f;
        _isPlated = false;
        _collider.enabled = true;

        Debug.LogWarning($"{gameObject.name} is an empty plate again!");
    }
    private void ChangeMaterial()
    {
          

    }

#region Collision
    private void DoFoodCollision(Food food)
    {
        if (food.IsContaminated || food.IsExpired)
        {
            Debug.LogError($"{food.gameObject.name} is already dirty!");
            // set contaminated
            return;
        }
    }
    private void DoIngredientCollision(Ingredient ing)
    {

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
        DEFAULT       = -1,
        NIGIRI_SALMON  = 0, 
        NIGIRI_TUNA    = 1, 
        SASHIMI_SALMON = 2, 
        SASHIMI_TUNA   = 3 
    }

#endregion

