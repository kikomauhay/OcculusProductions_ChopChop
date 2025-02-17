using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public enum StorageType { CHILLER, FREEZER } 

public class InventoryManager : Singleton<InventoryManager>
{
    List<GameObject> _fridge = new List<GameObject>();

#region Methods

    protected override void Awake() => base.Awake();
    protected override void OnApplicationQuit() => base.OnApplicationQuit(); 
    void Reset() // removes all the food after testing 
    {         
        if (_fridge.Count > 0)
        {
            foreach (GameObject food in _fridge)
                Destroy(food);

            _fridge.Clear();
        }
    }  

#endregion endregion

    public void TakeOut(GameObject food) 
    {
        Ingredient ingredient = food?.GetComponent<Ingredient>();

        if (ingredient == null) return;

        // transfers the food outside
        IngredientManager.Instance.Ingredients.Add(food);
        _fridge.Remove(food);    
        
        
        StartCoroutine(TookOut(ingredient));
    }
    public void StoreIn(StorageType mode, GameObject food) 
    {
        Ingredient ingredient = food.GetComponent<Ingredient>();

        if (ingredient == null) return;

        // prevents contaminated food from entering the fridge
        if (ingredient.FreshnessRate == 0) 
        {
            IngredientManager.Instance.TrashIngredient(food);
            Debug.LogError($"{ingredient.IngredientStats.name} is already expired! It's unsafe to put in any storage!");
            return;
        }

        // transfers food inside
        IngredientManager.Instance.Ingredients.Remove(food);
        _fridge.Add(food);    
        
        if (ingredient.IngredientStats.StorageType == mode)
            ingredient.IsProperlyStored = true;
    }
    
    IEnumerator TookOut(Ingredient food) // grace period before the food starts decaying
    {
        yield return new WaitForSecondsRealtime(10f);
        food.IsProperlyStored = false;
    }
}