using System.Collections;
using UnityEngine;
using System;

/// <summary>
/// 
/// - Acts as the reworked version of Plate.cs
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

    [Header("Debug Mode")]
    [SerializeField] private bool _debugging;

    [Header("Food Components"), Space(5f)]
    [Tooltip("0 = N. Salmon, 1 = N. Tuna, 2 = S. Salmon, 3 = S. Tuna")]
    [SerializeField] private GameObject[] _foodItems;
    [SerializeField] private FoodLooks _foodLooks;
    
    [Header("Dish Attributes"), Space(5f)]
    [SerializeField, Range(0f, 100f)] private float _dishScore;
    [SerializeField] private DishType _dishType;
    [SerializeField] private DishState _dishState;
    [SerializeField] private bool _isPlated;

    private Collider _collider;
    private const float DECAY_TIME = 30f;

#endregion

#region Unity

    private void Awake()
    {
        _collider = GetComponent<Collider>();
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
        {
            DoIngredientCollision(ing);
            return;
        }

        // case for nigiris
        if (food != null)
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
        if (!_debugging) return;

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
            throw new ArgumentException("Dish is already plated!", nameof(_isPlated));
        
        if (dishInput == DishType.DEFAULT)
            throw new ArgumentException("Default mode chosen!", nameof(dishInput));

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
        if (_dishState == chosenState)
            throw new ArgumentException("You cannot set the same state again!");

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
        // disables all food in the array
        if (_foodItems.Length > 0)
            foreach (GameObject item in _foodItems)
                item.SetActive(false); 
    }
    private void ResetDish()
    {
        DisableFoodItems();

        _dishScore = 0f;
        _isPlated = false;
        _collider.enabled = true;
    }

    // ON TRIGGER ENTER
    private void DoFoodCollision(Food food)
    {
        if (food.IsContaminated || food.IsExpired)
        {
            Debug.LogError($"{food.gameObject.name} is dirty!");
            // set contaminated
            return;
        }
    }

    private void DoIngredientCollision(Ingredient ing)
    {

    }

#endregion
}   

public enum DishState
{
    CLEAN  = 0, 
    ROTTEN = 1, 
    MOLDY  = 2
}

[Serializable]
public struct FoodLooks
{
#region Properties

    public readonly Material CleanMat => _materials[0];
    public readonly Material RottenMat => _materials[1];
    public readonly Material MoldyMat => _materials[2];

#endregion

#region Private

    [Tooltip("0 = clean, 1 = rotten, 2 = moldy")]
    [SerializeField] private Material[] _materials;
    [SerializeField] private Material _outlineMat;

#endregion
}