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
                obj.GetComponent<Ingredient>().Contaminate();
                break;

            case TrashableType.FOOD:
                obj.GetComponent<Food>().Contaminate();
                break;

            case TrashableType.DISH:
                obj.GetComponent<Dish>().Contaminate();
                break;

            case TrashableType.EQUIPMENT:
                DoEquipmentLogic(obj.GetComponent<Equipment>());
                break;

            default: break;
        }
    }

    void DoEquipmentLogic(Equipment eq)
    {
        eq.ResetPosition();
        eq.GetComponent<Rigidbody>().velocity = Vector3.zero;
        
        // additional logic for equipment child classes
        if (eq.GetComponent<Plate>() != null)
        {
            if (eq.GetComponent<Plate>().IsClean)
                eq.ToggleClean();

            SoundManager.Instance.PlaySound("plate dropped", SoundGroup.EQUIPMENT);
        }

        if (eq.GetComponent<Knife>() != null)
        {
            // ternary operator syntax -> condition ? val_if_true : val_if_false
            SoundManager.Instance.PlaySound(Random.value > 0.5f ? "knife dropped 01" : "knife dropped 02",
                                            SoundGroup.EQUIPMENT);
        }
    } 
}
