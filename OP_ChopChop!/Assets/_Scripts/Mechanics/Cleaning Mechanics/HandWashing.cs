using System.Collections;
using UnityEngine;

public class HandWashing : MonoBehaviour
{
#region Members

    public bool IsWet { get; private set; }

    [SerializeField] private bool _isDirty;
    [SerializeField] public Collider HandWashCollider;
    [SerializeField] private Material _handMaterial, _outlineTexture, _warningOutlineTexture;

    private float _timer;
    private bool _hasSpawnedVFX;

#endregion

#region Methods

    void Start()
    { 
        IsWet = false;
        HandWashCollider.enabled = false;
        
        _timer = 20f;
        _isDirty = false;
        _hasSpawnedVFX = false;
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
        if (!IsWet) return;

        if (other.gameObject.GetComponent<HandWashing>() != null)
        {
            if (GameManager.Instance.CurrentShift != GameShift.Training)
            {
                SpawnManager.Instance.SpawnVFX(VFXType.BUBBLE, 
                                               HandWashCollider.transform, 
                                               3f);
            }
            _timer -= Time.deltaTime;

            if (_timer <= 0)
            {
                HandManager.Instance._handUsage = 30;
                HandWashCollider.enabled = false;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        _timer = 20f;

        if (IsWet)
            StartCoroutine(WetToggle());
    }

#endregion

#region Helpers

    public void Dirtify()
    {
        _isDirty = true;
        Debug.LogWarning("Dirtified");

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

    public void Cleaned() => _isDirty = false;
    public void ToggleWet() => IsWet = true;
    public void PlayVFX()
    {
        if (_isDirty && !_hasSpawnedVFX)
            StartCoroutine(SpawnVFXWithDelay());
    }
    
#endregion

#region Enuerators

    private IEnumerator WetToggle()
    {
        yield return new WaitForSeconds(5f);
        IsWet = false;
    }
    private IEnumerator SpawnVFXWithDelay()
    {
        _hasSpawnedVFX = true;
        SpawnManager.Instance.SpawnVFX(VFXType.STINKY, HandWashCollider.transform, 3f);
        yield return new WaitForSeconds(5f); // Delay before it can spawn again
        _hasSpawnedVFX = false;
    }

#endregion
}
