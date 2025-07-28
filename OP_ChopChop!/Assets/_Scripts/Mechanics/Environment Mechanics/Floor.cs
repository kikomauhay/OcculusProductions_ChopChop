using UnityEngine;

public class Floor : MonoBehaviour
{
    #region Unity 

    private void OnCollisionEnter(Collision other)
    {
        GameObject obj = other.gameObject;

        if (obj.GetComponent<Sponge>() != null)
        {
            obj.GetComponent<Sponge>().HitTheGround();
            return;
        }
        if (obj.GetComponent<Trashable>() == null) 
        {
            Debug.LogError($"{obj.name} is not a trashable object!");
            return;
        }

        if (obj.GetComponent<Sponge>() != null)
        {
            obj.GetComponent<Sponge>().HitTheGround();
            return;
        }

        switch (obj.GetComponent<Trashable>().TrashTypes)
        {
            case TrashableType.INGREDIENT:
                DoIngredientLogic(obj.GetComponent<Ingredient>());
                break;

            case TrashableType.FOOD:
                DoFoodLogic(obj.GetComponent<UPD_Food>());
                break;

            case TrashableType.EQUIPMENT:
                DoEquipmentLogic(obj.GetComponent<Equipment>());
                break;

            default: break;
        }
    }

    #endregion

    #region Helpers

    private void DoIngredientLogic(Ingredient ing)
    {
        if (ing == null)
        {
            Debug.LogError($"{ing.name} is not an ingredient");
            return;
        }

        ing.SetMoldy();
        
        if (ing.IngredientType != IngredientType.RICE)
            SoundManager.Instance.PlaySound("fish dropped");

        Debug.LogWarning($"{ing.name} landed on the floor!");
    }
    private void DoFoodLogic(UPD_Food food)
    {
        if (food == null)
        {
            Debug.LogError($"{food.name} is not a food");
            return;
        }

        food.SetMoldy();
        SoundManager.Instance.PlaySound("fish dropped");
        Debug.LogWarning($"{food.name} landed on the floor!");
    }
    private void DoEquipmentLogic(Equipment eq)
    {
        if (eq == null)
        {
            Debug.LogError($"{eq.name} is not an equipment");
            return;
        }

        eq.HitTheGround();
        eq.GetComponent<Rigidbody>().velocity = Vector3.zero;
        // Debug.LogWarning($"{eq.name} landed on the floor!");
    } 

    #endregion
}
