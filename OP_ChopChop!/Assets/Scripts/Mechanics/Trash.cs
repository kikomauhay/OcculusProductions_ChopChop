using UnityEngine;

public class Trash : MonoBehaviour
{

    // needs an explanation
    // Kiko will make fixes to this
    // "Reparent the physics" - Sir G

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<Destructable>() != null)
        {
            Destroy(other.gameObject);
            SoundManager.Instance.PlaySound("dispose food");
        }
    }
}
