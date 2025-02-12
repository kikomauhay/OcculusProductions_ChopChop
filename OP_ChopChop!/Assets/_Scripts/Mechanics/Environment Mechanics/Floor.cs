using UnityEngine;

public class Floor : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<Plate>() != null)
        {
            other.gameObject.GetComponent<Plate>().SetContaminated();
            return;
        }
        if (other.gameObject.GetComponent<Trashable>() == null) return;
        
        switch (other.gameObject.GetComponent<Trashable>()._trashTypes)
        {
            case TrashableType.INGREDIENT:
                other.gameObject.GetComponent<Ingredient>().ContaminateFood();
                break;
            case TrashableType.FOOD:
                other.gameObject.GetComponent<Ingredient>().ContaminateFood();
                break;
            case TrashableType.EQUIPMENT:
                Debug.LogWarning("Reset Equipment");
                break;
            default: break;
        }
    }
}
