using UnityEngine;

public class Trash : MonoBehaviour
{

    // needs an explanation
    // Kiko will make fixes to this
    // "Reparent the physics" - Sir G

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<Trashable>()._trashTypes == TrashTypes.Ingredients
            && other.gameObject.GetComponent<Trashable>() != null)
        {
            Destroy(other.gameObject);
            SoundManager.Instance.PlaySound("dispose food");
        }
        if (other.gameObject.GetComponent<Trashable>()._trashTypes == TrashTypes.Food
            && other.gameObject.GetComponent<Trashable>() != null)
        {
            // Reinstantiate plate prefab
        }
        if (other.gameObject.GetComponent<Trashable>()._trashTypes == TrashTypes.Equipment
            && other.gameObject.GetComponent<Trashable>() != null)
        {
            //Reset Equipment here
        }
    }
}
