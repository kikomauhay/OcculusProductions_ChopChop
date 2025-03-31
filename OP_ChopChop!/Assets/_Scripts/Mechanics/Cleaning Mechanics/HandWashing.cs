using System.Collections;
using UnityEngine;

public class HandWashing : MonoBehaviour
{
    public bool IsWet { get; private set; }

    [SerializeField] bool _isDirty;
    [SerializeField] Collider _handWashCollider;
    [SerializeField] float _timer;

    void Start()
    { 
        _isDirty = true;
        IsWet = false;

        // Debug.Log($"Hand Dirty is {_isDirty}");
    }

    private void FixedUpdate()
    {
        KitchenCleaningManager.Instance.ToggleHandWashColliders();
        if (KitchenCleaningManager.Instance.HandUsageCounter <= 0)
            _isDirty = true;
            
        else _isDirty = false;

        if (IsWet)
            StartCoroutine(WetToggle());

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

        if (other.gameObject.GetComponent<HandWashing>() != null
            && IsWet)
        {
            SpawnManager.Instance.SpawnVFX(VFXType.BUBBLE, 
                                                   transform, 
                                                   3F);

            //Increment Clean rate by random float
        }
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
