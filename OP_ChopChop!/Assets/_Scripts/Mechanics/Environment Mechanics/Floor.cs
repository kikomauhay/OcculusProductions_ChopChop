using UnityEngine;

public class Floor : MonoBehaviour
{
    void OnCollisionEnter(Collision  other)
    {
        GameObject obj = other.gameObject;

        if (obj.GetComponent<Trashable>() == null) return;

        switch (obj.GetComponent<Trashable>().TrashTypes)
        {
            case TrashableType.INGREDIENT:
                DoIngredientLogic(obj.GetComponent<Ingredient>());
                break;

            case TrashableType.FOOD:
                obj.GetComponent<Food>().Contaminate();
                SoundManager.Instance.PlaySound("fish dropped", SoundGroup.FOOD);
                break;

            case TrashableType.DISH:
                obj.GetComponent<Dish>().HitTheFloor();
                SoundManager.Instance.PlaySound(Random.value > 0.5f ? 
                                                "plate placed 01" : 
                                                "plate placed 02", 
                                                SoundGroup.EQUIPMENT);
                break;

            case TrashableType.EQUIPMENT:
                DoEquipmentLogic(obj.GetComponent<Equipment>());
                break;

            default: break;
        }
    }

    void DoIngredientLogic(Ingredient ing)
    {
        ing.Contaminate();
        
        if (ing.IngredientType == IngredientType.SALMON ||
            ing.IngredientType == IngredientType.TUNA)
        {
            SoundManager.Instance.PlaySound("fish dropped", SoundGroup.FOOD);
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

            SoundManager.Instance.PlaySound(Random.value > 0.5f ? "plate placed 01" : "plate placed 02", 
                                            SoundGroup.EQUIPMENT);
        }

        if (eq.GetComponent<Knife>() != null)
        {
            // ternary operator syntax -> condition ? val_if_true : val_if_false
            SoundManager.Instance.PlaySound(Random.value > 0.5f ? "knife dropped 01" : "knife dropped 02",
                                            SoundGroup.EQUIPMENT);
        }
    } 
}
