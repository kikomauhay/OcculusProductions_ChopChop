using UnityEngine;

public class HatMechanic : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<ChefHat>() == null) return;

        if (GameManager.Instance.CurrentShift == GameShift.Training) 
        {
            OnBoardingHandler.Instance.Disable();
            SoundManager.Instance.StopAllAudio(); // in case there is any ongoing tutorial lines
            other.gameObject.GetComponent<ChefHat>().StartService();
        }
    }
}
