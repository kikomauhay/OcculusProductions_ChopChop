using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : Singleton<InventoryManager>
{
    public static List<GameObject> Chiller { get; private set; }
    public static List<GameObject> Freezer { get; private set; }

    protected override void Awake() { base.Awake(); }
    void Start()
    {
        Chiller = new List<GameObject>();
        Freezer = new List<GameObject>();
    }

    public void TakeOutIngredientFrom(StorageType mode, GameObject food) {
        // takes out ingredient (will do the mode selection later)

        IngredientManager.OutsideIngredients.Add(food);
    }

    public void StoreIngredientIn(StorageType mode, GameObject food) 
    {
        if (mode == StorageType.CHILLER) 
        {
            Chiller.Add(food);
            return;
        }

        Freezer.Add(food);
    }

    public void Reset() 
    { 
        if (Chiller.Count > 0) 
        {
            foreach (GameObject food in Chiller) 
                Destroy(food);
        
            Chiller.Clear();
        }
        
        if (Freezer.Count > 0)
        {
            foreach (GameObject food in Freezer)
                Destroy(food);

            Freezer.Clear();
        }
    }    
    protected override void OnApplicationQuit() { base.OnApplicationQuit(); }
}

public enum StorageType { CHILLER, FREEZER } 