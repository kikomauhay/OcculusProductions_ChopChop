using System;
using System.Collections;
using UnityEngine;

public class HandWashing : MonoBehaviour
{
#region Members

    public bool IsWet { get; private set; }
    public static Action<int> OnHandCleaned;

    [SerializeField] private bool _isDirty;
    [SerializeField] public Collider HandWashCollider;
    [SerializeField] private Material _handMaterial, _outlineTexture, _warningOutlineTexture;

    private float _timer;
    private bool _washLogicRunning;

#endregion

#region Methods

    private void Start()
    { 
        IsWet = false;
        HandWashCollider.enabled = false;
        
        _timer = 5f;
        _isDirty = false;
        _washLogicRunning = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        HandWashing otherHand = other.gameObject.GetComponent<HandWashing>();

        if (other.gameObject.GetComponent<Ingredient>() != null)
        {
            if (_isDirty)
            {
                other.gameObject.GetComponent<Ingredient>().SetMoldy();
            }
        }

        if(otherHand != null)
        {
            if (!IsWet || !otherHand.IsWet) return;
            
            if(!_washLogicRunning)
            {
                HandManager.Instance.ToggleBubblesOn();
                StartCoroutine(WashLogic());
                StartCoroutine(WetToggle());
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.GetComponent<HandWashing>() != null)
        {
            if (!IsWet) return;

            _washLogicRunning = false ;
            StopCoroutine(WashLogic());
        }
    }

#endregion

#region Helpers

    public void Dirtify()
    {
        _isDirty = true;
        // Debug.LogWarning("Dirtified");

        if (_isDirty)
        {
            SkinnedMeshRenderer r = GetComponentInChildren<SkinnedMeshRenderer>();
            
            if (r != null)
            {
                r.materials = new Material[] { _handMaterial, _outlineTexture };
            }
        }
    }

    public void WarningIndicator()
    {
        SkinnedMeshRenderer r = GetComponentInChildren<SkinnedMeshRenderer>();
        
        if (r != null)
        {
            r.materials = new Material[] { _handMaterial, _warningOutlineTexture };
        }
    }

    public void Cleaned()
    {
        SkinnedMeshRenderer r = GetComponentInChildren<SkinnedMeshRenderer>();
        _isDirty = false;

        if (r != null && !_isDirty)
        {
            r.materials = new Material[] { _handMaterial};
        }

    }

    public void Wet() => IsWet = true;

    public void Dry() => StartCoroutine(WetToggle());
    
#endregion

#region Enuerators

    private IEnumerator WetToggle()
    {
        yield return new WaitForSeconds(7f);
        IsWet = false;
        HandManager.Instance.ToggleBubblesOff();
    }

    private IEnumerator WashLogic()
    {
        _washLogicRunning = true;
        yield return new WaitForSeconds(_timer);
        OnHandCleaned?.Invoke(20);
        _washLogicRunning = false;
    }

#endregion
}
