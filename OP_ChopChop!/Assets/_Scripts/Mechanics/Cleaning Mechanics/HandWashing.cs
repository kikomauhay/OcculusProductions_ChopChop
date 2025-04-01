using System.Collections;
using UnityEngine;

public class HandWashing : MonoBehaviour
{
    public bool IsWet { get; private set; }

    [SerializeField] Collider _handWashCollider;
    [SerializeField] bool _isDirty;

    private float _timer;
    private int _handUsage;

    void Start()
    { 
        _handWashCollider.enabled = false;
        _isDirty = true;
        IsWet = false;
        _timer = 20F;
        _handUsage = 20;

        // Debug.Log($"Hand Dirty is {_isDirty}");
    }

    private void FixedUpdate()
    {
        if (_handUsage < 10)
        {
            _handWashCollider.enabled = true;
        }
        else if (_handUsage <= 0)
        {
            _isDirty = true;
            SpawnManager.Instance.SpawnVFX(VFXType.STINKY, _handWashCollider.transform, 5F);
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
        else if(other.gameObject.GetComponent<HandWashing>() != null)
        {
            if(other.gameObject.GetComponent<HandWashing>()._isDirty)
            {
                _isDirty = true;
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
                _handUsage = 20;
                _handWashCollider.enabled = false;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        _timer = 20F;

        if (IsWet)
            StartCoroutine(WetToggle());
    }

#region Functions

    public void ToggleWet()
    {
        IsWet = true;
    }

    public void DecrementUsage()
    {
        _handUsage--;
    }

    IEnumerator WetToggle()
    {
        yield return new WaitForSeconds(5F);
        IsWet = false;
    }
#endregion
}
