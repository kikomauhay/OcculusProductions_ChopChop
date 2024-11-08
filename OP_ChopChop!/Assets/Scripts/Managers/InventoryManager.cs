using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public enum StorageType { CHILLER, FREEZER } 

public class InventoryManager : Singleton<InventoryManager>
{    
    public List<GameObject> Fridge => _fridge;
    
    List<GameObject> _fridge = new List<GameObject>();

    protected override void Awake() { base.Awake(); }
    void Reset() 
    {         
        if (_fridge.Count > 0)
        {
            foreach (GameObject food in _fridge)
                Destroy(food);

            _fridge.Clear();
        }
    }  

    public void TakeOut(GameObject food) 
    {
        Ingredient ingredient = food.GetComponent<Ingredient>();

        if (ingredient == null) return;

        // transfers the food outside
        IngredientManager.Instance.Ingredients.Add(food);
        _fridge.Remove(food);    
        
        // grace period before the food rots faster again
        StartCoroutine(TookOut(ingredient));
    }
    public void StoreIn(StorageType mode, GameObject food) 
    {
        Ingredient ingredient = food.GetComponent<Ingredient>();

        if (ingredient == null) return;

        // prevents contaminated food from entering the fridge
        if (ingredient.Stats.FreshnessRate == 0) 
        {
            IngredientManager.Instance.TrashIngredient(food);
            Debug.LogError($"{ingredient.Stats.name} is already expired! It's unsafe to put in any storage!");
            return;
        }

        // transfers food inside
        IngredientManager.Instance.Ingredients.Remove(food);
        _fridge.Add(food);    
        
        if (ingredient.Stats.StorageType == mode)
            ingredient.ToggleProperStorage();
    }

    protected override void OnApplicationQuit() { base.OnApplicationQuit(); }

    IEnumerator TookOut(Ingredient food) 
    {
        yield return new WaitForSecondsRealtime(10f);
        food.ToggleProperStorage();
    }
}