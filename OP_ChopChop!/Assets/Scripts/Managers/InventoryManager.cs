using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// InventoryManager is only for foods that are not currenty being used
/// Think of this script as a refrigerator
/// </summary>

public class InventoryManager : Singleton<InventoryManager>
{
    List<GameObject> _chilledFoods, _frozenFoods;

    public List<GameObject> Chiller { get => _chilledFoods; }
    public List<GameObject> Freezer { get => _frozenFoods; }

    protected override void Awake() { base.Awake(); }
    void Start()
    {
        _chilledFoods = new List<GameObject>();
        _frozenFoods = new List<GameObject>();
    }

    public void TakeOutIngredient(StorageType mode, GameObject food) {
        // takes out ingredient (will do the mode selection later)

        IngredientManager.Instance.AddIngredient(food);
    }

    public void StoreIngredientIn(StorageType mode, GameObject food) 
    {
        if (mode == StorageType.CHILLER) 
        {
            _chilledFoods.Add(food);
            return;
        }

        _frozenFoods.Add(food);
    }

    public void Reset() 
    { 
        foreach (GameObject food in _chilledFoods) 
            Destroy(food);
            
        foreach (GameObject food in _frozenFoods) 
            Destroy(food);
        
        _chilledFoods.Clear();
        _frozenFoods.Clear();
    }

    
    protected override void OnApplicationQuit() { base.OnApplicationQuit(); }
}

public enum StorageType { CHILLER, FREEZER } 