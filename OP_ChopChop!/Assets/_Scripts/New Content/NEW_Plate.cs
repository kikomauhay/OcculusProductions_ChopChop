using System.Collections;
using UnityEngine;
using System;

/// <summary>
/// 
/// - Acts as the reworked version of Plate.cs
/// 
/// </summary>

public class NEW_Plate : Equipment
{
#region Properties

    public bool IsPlated => _isPlated;
    public float Score 
    {
        get => _dishScore;
        set => _dishScore = value;
    }
    

#endregion 

#region Private


    [Header("Food Components"), Tooltip("0 = N. Salmon, 1 = N. Tuna, 2 = S. Salmon, 3 = S. Tuna")]
    [SerializeField] private GameObject[] _foodItems;
    
    [Header("Plate Attributes")]
    [SerializeField] private Platter _platterServed;
    [SerializeField] private bool _isPlated;
    [SerializeField, Range(0f, 100f)] private float _dishScore;
    

    [Header("Debugging Section")]
    [SerializeField] private bool _debugging;

    private Collider _collider;

#endregion

#region Unity

    private void Awake()
    {
        _collider = GetComponent<Collider>();
    }
    protected override void Start() 
    {
        _collider.enabled = false; // true;
        _platterServed = Platter.DEFAULT;
        
        DisableFoodItems();
    }

    protected override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);
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
            SetPlatter(Platter.NIGIRI_SALMON);
        
        if (Input.GetKeyDown(KeyCode.Alpha2))
            SetPlatter(Platter.NIGIRI_TUNA);
        
        if (Input.GetKeyDown(KeyCode.Alpha3))
            SetPlatter(Platter.SASHIMI_SALMON);

        if (Input.GetKeyDown(KeyCode.Alpha4))
            SetPlatter(Platter.SASHIMI_TUNA);

        if (Input.GetKeyDown(KeyCode.Delete)) 
            ResetDish();
    }

#endregion

#endregion

#region Helpers

    public void SetPlatter(Platter platterInput)
    {
        if (_isPlated)
            throw new ArgumentException("Dish is already plated!", nameof(_isPlated));
        
        if (platterInput == Platter.DEFAULT)
            throw new ArgumentException("Default mode chosen!", nameof(platterInput));

        // sets the dish 
        _platterServed = platterInput;
        _foodItems[(int)platterInput].SetActive(true);
        _isPlated = true;
        _dishScore = 100f; // test

        Debug.LogWarning($"Dish has a {_platterServed}!");
        Debug.LogWarning($"Is plated: {_isPlated}");
    }

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
                
        Debug.LogWarning("Dish has been reset!");
        Debug.LogWarning($"Is plated: {_isPlated}");
    }

#endregion
}

public enum Platter
{
    DEFAULT       = -1,
    NIGIRI_SALMON  = 0,
    NIGIRI_TUNA    = 1,
    SASHIMI_SALMON = 2,
    SASHIMI_TUNA   = 3
}