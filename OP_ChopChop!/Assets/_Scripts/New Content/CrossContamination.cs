using Unity.VisualScripting;
using UnityEngine;

public class CrossContamination : MonoBehaviour 
{
#region Members

    public TrashableType ContaminationType => _contaminationType; 
    [SerializeField] private TrashableType _contaminationType;


#endregion 

#region Unity

    private void OnCollisionEnter(Collision other)
    { 
        CrossContamination cc = other.gameObject.GetComponent<CrossContamination>();

        if (cc == null)
        {
            Debug.LogError($"{other.gameObject.name} cannot be cross-contaminated!");
            return;
        }        

    }

#endregion

#region Collision

    private void ContaminateIngredient(Ingredient ing)
    {
        if (ing == null)
        {
            Debug.LogError($"{ing.name} is not an ingredient");
            return;
        }

        if (ing.IngredientState == IngredientState.DEFAULT)
            ing.SetMoldy();
    }
    private void ContaminateFood(UPD_Food food)
    {
        if (food == null)
        {
            Debug.LogError($"{food.name} is not a food");
            return;
        }

        if (food.Condition == FoodCondition.CLEAN)
            food.SetMoldy();
    }
    private void ContaminateEquipment(Equipment eq)
    {
        if (eq == null)
        {
            Debug.LogError($"{eq.name} is not an equipment");
            return;
        }

            
    }


#endregion
}