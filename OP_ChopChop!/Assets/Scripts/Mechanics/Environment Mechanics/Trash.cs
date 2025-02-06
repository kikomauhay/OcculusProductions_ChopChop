using UnityEngine;

public class Trash : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.GetComponent<Trashable>() != null)
        {
            if (other.gameObject.GetComponent<Trashable>()._trashTypes == TrashTypes.Ingredients)
            {
                Destroy(other.gameObject);
                SoundManager.Instance.PlaySound("dispose food");
            }
            if (other.gameObject.GetComponent<Trashable>()._trashTypes == TrashTypes.Food)
            {
                // Reinstantiate plate prefab
            }
            if (other.gameObject.GetComponent<Trashable>()._trashTypes == TrashTypes.Equipment)
            {
                //Reset Equipment here
                //Set Reset Points
            }
        }
    }
}
