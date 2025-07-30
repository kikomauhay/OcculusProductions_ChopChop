using UnityEngine;

public class Trash : MonoBehaviour
{
    #region Unity

    private void OnTriggerEnter(Collider other)
    {
        GameObject obj = other.gameObject;

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
        
        switch(obj.GetComponent<Trashable>().TrashTypes)
        {
            case TrashableType.INGREDIENT:
                DestroyIngredient(obj.GetComponent<Ingredient>());
                break;

            case TrashableType.FOOD:
                DestroyFood(obj.GetComponent<UPD_Food>());
                break;

            case TrashableType.EQUIPMENT:
                RepositionEquipment(obj.GetComponent<Equipment>());
                break;

            default: break;
        }
    }

    #endregion

    #region Collision

    private void DestroyIngredient(Ingredient ing)
    {
        if (ing == null)
        {
            Debug.LogError($"{ing.name} is not an ingredient");
            return;
        }

        ing.Trashed();
        Destroy(ing.gameObject);
        SoundManager.Instance.PlaySound("dispose food");
    }
    private void DestroyFood(UPD_Food food)
    {
        if (food == null)
        {
            Debug.LogError($"{food.name} is not a food");
            return;
        }

        Destroy(food.gameObject);
        SoundManager.Instance.PlaySound("dispose food");
    }
    private void RepositionEquipment(Equipment eq)
    {
        if (eq == null)
        {
            Debug.LogError($"{eq.name} is not an equipment");
            return;
        }

        eq.Trashed();
        eq.GetComponent<Rigidbody>().velocity = Vector3.zero;
    }

    #endregion
}
