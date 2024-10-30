using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public enum StorageType { CHILLER, FREEZER } 

public class InventoryManager : Singleton<InventoryManager>
{    
    List<GameObject> _chiller, _freezer;
    Ingredient ingredient;

    protected override void Awake() { base.Awake(); }
    void Start()
    {
        _chiller = new List<GameObject>();
        _freezer = new List<GameObject>();

        ingredient = null;
    }
    void Reset() 
    { 
        if (_chiller.Count > 0) 
        {
            foreach (GameObject food in _chiller) 
                Destroy(food);
        
            _chiller.Clear();
        }
        
        if (_freezer.Count > 0)
        {
            foreach (GameObject food in _freezer)
                Destroy(food);

            _freezer.Clear();
        }
    }  

    public void TakeOut(StorageType mode, GameObject food) 
    {
        ingredient = food.GetComponent<Ingredient>();

        if (ingredient.Stats.FreshnessRate == 0) 
        {
            ingredient = null;
            Debug.LogError($"{ingredient.Stats.name} is unsafe to put in any storage!");
            return;
        }   

        IngredientManager.Ingredients.Add(food);
        if (mode == StorageType.CHILLER) _chiller.Remove(food);
        else                             _freezer.Remove(food);        

        if (ingredient.Stats.StorageType == mode)
            ingredient.ToggleProperStorage();

        StartCoroutine(TookOut(ingredient));
        ingredient = null;
    }
    public void StoreIn(StorageType mode, GameObject food) 
    {
        ingredient = food.GetComponent<Ingredient>();

        if (ingredient.Stats.FreshnessRate == 0) 
        {
            ingredient = null;
            Debug.LogError($"{ingredient.Stats.name} is unsafe to put in any storage!");
            return;
        }

        IngredientManager.Ingredients.Remove(food);
        if (mode == StorageType.CHILLER) _chiller.Add(food);
        else                             _freezer.Add(food);
        
        if (ingredient.Stats.StorageType == mode)
            ingredient.ToggleProperStorage();

        ingredient = null;
    }

    protected override void OnApplicationQuit() { base.OnApplicationQuit(); }

    IEnumerator TookOut(Ingredient food) 
    {
        yield return new WaitForSecondsRealtime(3f);
        food.ToggleProperStorage();
    }
}