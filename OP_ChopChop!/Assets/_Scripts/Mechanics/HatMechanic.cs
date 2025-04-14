using UnityEngine;

public class HatMechanic : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<ChefHat>() == null) return;

        if (GameManager.Instance.CurrentShift != GameShift.TRAINING) return;

        other.gameObject.GetComponent<ChefHat>().StartService();
    }
}
