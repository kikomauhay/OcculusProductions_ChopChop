using UnityEngine;

public class Water : MonoBehaviour
{
#region Members
 
    [SerializeField] private bool _isTutorial;
    [SerializeField] private float _cooldownTimer, _cooldownInterval;
    [SerializeField] private bool _hasExitedAndPlayed;

#endregion

#region Unity

    private void OnEnable() => SoundManager.Instance.PlaySound("tap water");
    private void OnDisable() => SoundManager.Instance.StopSound();
    private void OnTriggerEnter(Collider other)
    {
        HandWashing handWash = other.gameObject.GetComponent<HandWashing>();

        if (other.gameObject.GetComponent<Sponge>() != null)
            other.gameObject.GetComponent<Sponge>().SetWet();
            
        if (handWash != null)
        {
            if (!handWash.IsWet)
                handWash.Wet();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        HandWashing handWash = other.gameObject.GetComponent<HandWashing>();

        if (handWash != null)
        {
            handWash.Dry();
        }
    }

    #endregion
}
