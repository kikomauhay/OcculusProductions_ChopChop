using System.Collections;
using UnityEngine;

public class HandWashing : MonoBehaviour
{
    public bool IsWet { get; private set; }
    public Collider _handWashCollider;

    [SerializeField] bool _isDirty;
    private float _timer;

    void Start()
    { 
        _isDirty = true;
        IsWet = false;
        _timer = 20F;

        // Debug.Log($"Hand Dirty is {_isDirty}");
    }

    private void FixedUpdate()
    {
        if (KitchenCleaningManager.Instance.HandUsageCounter < 15)
        {
            KitchenCleaningManager.Instance.ToggleHandWashColliders();
        }
        else if (KitchenCleaningManager.Instance.HandUsageCounter <= 0)
        {
            _isDirty = true;
        }
        else _isDirty = false;    
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<Ingredient>() != null)
        {
            if (_isDirty)
            {
                other.gameObject.GetComponent<Ingredient>().Contaminate();
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.GetComponent<HandWashing>() != null
            && IsWet)
        {
            SpawnManager.Instance.SpawnVFX(VFXType.BUBBLE, 
                                                   _handWashCollider.transform, 
                                                   3F);
            _timer -= Time.deltaTime;

            if (_timer <= 0)
            {
                KitchenCleaningManager.Instance.ResetHandUsage();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        _timer = 20F;

        if (IsWet)
            StartCoroutine(WetToggle());
    }

    public void ToggleWet()
    {
        IsWet = true;
    }



    IEnumerator WetToggle()
    {
        yield return new WaitForSeconds(5F);
        IsWet = false;
    }
}
