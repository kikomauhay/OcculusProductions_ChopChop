using UnityEngine;

public class Floor : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        GameObject obj = other.gameObject;

        if (obj.GetComponent<Trashable>() == null) return;

        switch (obj.GetComponent<Trashable>().TrashTypes)
        {
            case TrashableType.INGREDIENT:
                DoIngredientLogic(obj.GetComponent<Ingredient>());
                break;

            case TrashableType.FOOD:
                DoFoodLogic(obj.GetComponent<Food>());
                break;

            case TrashableType.EQUIPMENT:
                DoEquipmentLogic(obj.GetComponent<Equipment>());
                break;

            default: break;
        }
    }

#region Collision_Logic

    void DoIngredientLogic(Ingredient ing)
    {
        ing.ContaminateIngredient();
        Debug.LogWarning($"{ing.gameObject.name} has been contaminated!");
    }

    void DoFoodLogic(Food food)
    {
        food.SetContaminated();
        Debug.LogWarning($"{food.gameObject.name} has been contaminated!");
    }

    void DoEquipmentLogic(Equipment eq)
    {
        eq.ResetPosition();
        eq.GetComponent<Rigidbody>().velocity = Vector3.zero;
        
        // other logic for equipment child classes
        
        if (eq.GetComponent<Plate>().IsClean)
        {
            eq.ToggleClean();

            Debug.LogWarning($"{eq.gameObject.name} has gotten dirty!");
        }  
    } 

#endregion

}
