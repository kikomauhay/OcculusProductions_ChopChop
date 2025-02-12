using UnityEngine;

public class Trash : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.GetComponent<Trashable>() != null)
        {
            if (other.gameObject.GetComponent<Trashable>()._trashTypes == TrashableType.INGREDIENT)
            {
                Destroy(other.gameObject);
                SoundManager.Instance.PlaySound("dispose food");
            }
            if (other.gameObject.GetComponent<Trashable>()._trashTypes == TrashableType.FOOD)
            {
                // Reinstantiate plate prefab
            }
            if (other.gameObject.GetComponent<Trashable>()._trashTypes == TrashableType.EQUIPMENT)
            {
                //Reset Equipment here
                //Set Reset Points
            }
        }
    }
}
