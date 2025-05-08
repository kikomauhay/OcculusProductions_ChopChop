using UnityEngine;

public class Water : MonoBehaviour
{
#region Members
 
    [SerializeField] private bool _isTutorial;
    [SerializeField] private float _cooldownTimer, _cooldownInterval;
    [SerializeField] private bool _hasExitedAndPlayed;

#endregion

#region Unity

    private void OnEnable() => SoundManager.Instance.PlaySound("tap water", SoundGroup.APPLIANCES);
    private void OnDisable() => SoundManager.Instance.SoundSource.Stop();
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<Sponge>() != null)
            other.gameObject.GetComponent<Sponge>().SetWet();
            
        if (other.gameObject.GetComponent<HandWashing>() != null)
        {
            HandWashing handWash = other.gameObject.GetComponent<HandWashing>();

            if (!handWash.IsWet)
                handWash.ToggleWet();
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (!_isTutorial) return;

        if (other.gameObject.GetComponent<HandWashing>() == null) return;

        // spanwning bubbles at an interval
        _cooldownTimer -= Time.deltaTime;
        if (_cooldownTimer <= 0f)
        {
            SpawnManager.Instance.SpawnVFX(VFXType.BUBBLE, transform, 2f);
            _cooldownTimer = _cooldownInterval;
        }
    }

#endregion
}
