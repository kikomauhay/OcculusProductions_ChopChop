using UnityEngine;

public class Water : MonoBehaviour
{
#region Members
 
    [SerializeField] private bool _isTutorial;
    [SerializeField] private float _cooldownTimer, _cooldownInterval; 
    [SerializeField] private GameObject _bubbleVFX;

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
            SpawnBubbles(other.gameObject.transform);
            _cooldownTimer = _cooldownInterval;
        }
    }    
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.GetComponent<HandWashing>() == null) return;

        if (_isTutorial)
        {
            transform.parent.GetComponentInChildren<OutlineMaterial>().DisableHighlight();
            OnBoardingHandler.Instance.CallOnboarding(1);
        }
    }

#endregion

#region Helpers

    private void SpawnBubbles(Transform t)
    {
        GameObject newVFX = Instantiate(_bubbleVFX, 
                                        t.transform.position,
                                        t.transform.rotation,
                                        transform);

        Destroy(newVFX, _cooldownInterval);
    }

#endregion
}
