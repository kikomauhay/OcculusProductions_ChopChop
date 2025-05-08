using UnityEngine;

public class Trash : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        GameObject obj = other.gameObject;

        if (obj.GetComponent<Trashable>() == null) return;

        if (obj.GetComponent<Sponge>() != null)
        {
            obj.GetComponent<Sponge>().ResetPosition();
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
                DoEquipmentLogic(obj.GetComponent<Equipment>());
                break;

            default: break;
        }
    }

#region Helpers

    private void DestroyIngredient(Ingredient ing)
    {
        ing.Trashed();
        Destroy(ing.gameObject);
        SoundManager.Instance.PlaySound("dispose food", SoundGroup.FOOD);
    }

    private void DestroyFood(UPD_Food food)
    {
        Destroy(food.gameObject);
        SoundManager.Instance.PlaySound("dispose food", SoundGroup.FOOD);
    }
    private void DoEquipmentLogic(Equipment eq)
    {
        eq.Trashed();
        eq.GetComponent<Rigidbody>().velocity = Vector3.zero;
    }

#endregion
}
