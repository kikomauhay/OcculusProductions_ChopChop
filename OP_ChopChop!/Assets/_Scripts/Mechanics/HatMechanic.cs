using UnityEngine;

public class HatMechanic : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<ChefHat>() != null)
        {
            if (GameManager.Instance.CurrentShift != GameShift.TRAINING ||
               !OnBoardingHandler.Instance.TutorialDone) 
            {
                return;
            }

            other.gameObject.GetComponent<ChefHat>().StartService();
        }        
    }
}
