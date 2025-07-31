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

#endregion

#region Methods

    void Start()
    { 
        IsWet = false;
        HandWashCollider.enabled = false;
        
        _timer = 3f;
        _isDirty = false;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<Ingredient>() != null)
        {
            if (_isDirty)
            {
                other.gameObject.GetComponent<Ingredient>().SetMoldy();
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (!IsWet) return;

        if (other.gameObject.GetComponent<HandWashing>() != null)
        {
            HandManager.Instance.ToggleBubblesOn();

            _timer -= Time.deltaTime;

            if (_timer <= 0)
            {
                // Debug.LogWarning($"Hand Status: {_isDirty}");
                OnHandCleaned?.Invoke(20);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        _timer = 3f;

        if (IsWet)
            StartCoroutine(WetToggle());
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
    public void ToggleWet() => IsWet = true;
    
#endregion

#region Enuerators

    private IEnumerator WetToggle()
    {
        yield return new WaitForSeconds(5f);
        IsWet = false;
        HandManager.Instance.ToggleBubblesOff();
    }

#endregion
}
