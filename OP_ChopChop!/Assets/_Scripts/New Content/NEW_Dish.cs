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

    [Header("Food Components"), Tooltip("0 = N. Salmon, 1 = N. Tuna, 2 = S. Salmon, 3 = S. Tuna")]
    [SerializeField, Space(5f)] private GameObject[] _foodItems;
    
    [Header("Dish Attributes")]
    [SerializeField, Space(5f)] private DishType _dishType;
    [SerializeField] private DishState _dishState;
    [SerializeField, Range(0f, 100f)] private float _dishScore;
    [SerializeField] private bool _isPlated;
    
    [Header("Debug Mode")]
    [SerializeField] private bool _debugging;

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

    public void Contaminate() 
    {
        _dishState = DishState.MOLDY;

        // change to moldy material
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

#endregion
}


public enum DishState
{
    CLEAN  = 0, 
    ROTTEN = 1, 
    MOLDY  = 2
}