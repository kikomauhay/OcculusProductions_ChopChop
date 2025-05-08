using UnityEngine;

public class Floor : MonoBehaviour
{
    private void OnCollisionEnter(Collision other)
    {
        GameObject obj = other.gameObject;

        if (obj.GetComponent<Trashable>() == null) return;

        switch (obj.GetComponent<Trashable>().TrashTypes)
        {
            case TrashableType.INGREDIENT:
                DoIngredientLogic(obj.GetComponent<Ingredient>());
                break;

            case TrashableType.FOOD:
                obj.GetComponent<UPD_Food>().SetRotten();
                SoundManager.Instance.PlaySound("fish dropped");
                break;

            case TrashableType.DISH:
                obj.GetComponent<Dish>().HitTheFloor();
                SoundManager.Instance.PlaySound(Random.value > 0.5f ? 
                                                "plate placed 01" : 
                                                "plate placed 02");
                break;

            case TrashableType.EQUIPMENT:
                DoEquipmentLogic(obj.GetComponent<Equipment>());
                break;

            default: break;
        }
    }

    private void DoIngredientLogic(Ingredient ing)
    {
        ing.Contaminate();
        
        if (ing.IngredientType == IngredientType.SALMON ||
            ing.IngredientType == IngredientType.TUNA)
        {
            SoundManager.Instance.PlaySound("fish dropped");
        }
    }

    private void DoEquipmentLogic(Equipment eq)
    {                
        eq.HitTheGround();
        eq.GetComponent<Rigidbody>().velocity = Vector3.zero;
        Debug.LogWarning($"{eq.name} landed on the floor!");
    } 
}
