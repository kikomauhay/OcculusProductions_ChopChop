using UnityEngine;

public class Water : MonoBehaviour
{
    [SerializeField] private bool _isTutorial;

    private void OnEnable() => SoundManager.Instance.PlaySound("tap water", SoundGroup.APPLIANCES);
    private void OnDisable() => SoundManager.Instance.SoundSource.Stop();    

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<Sponge>() != null)
            other.gameObject.GetComponent<Sponge>().SetWet();
            
        if (other.gameObject.GetComponent<HandWashing>() != null)
        {
            if (_isTutorial)
            {
                transform.parent.GetComponentInChildren<OutlineMaterial>().DisableHighlight();
                OnBoardingHandler.Instance.CallOnboarding(1);
                return;
            }
            
            HandWashing handWash = other.gameObject.GetComponent<HandWashing>();

            if (!handWash.IsWet)
                handWash.ToggleWet();
        }
    }
}
