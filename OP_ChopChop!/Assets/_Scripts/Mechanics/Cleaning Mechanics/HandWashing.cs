using System.Collections;
using UnityEngine;

public class HandWashing : MonoBehaviour
{
    public bool IsWet { get; private set; }

    [SerializeField] public Collider HandWashCollider;
    [SerializeField] bool _isDirty;
    [SerializeField] Material _handMaterial, _outlineTexture, _warningOutlineTexture;

    private float _timer;
    private bool _hasSpawnedVFX;

    void Start()
    { 
        HandWashCollider.enabled = false;
        _isDirty = false;
        IsWet = false;
        _timer = 20F;
        _hasSpawnedVFX = false;

        // Debug.Log($"Hand Dirty is {_isDirty}");
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
                                                   HandWashCollider.transform, 
                                                   3F);
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
        _timer = 20F;

        if (IsWet)
            StartCoroutine(WetToggle());
    }

#region Functions

    public void Dirtify()
    {
        _isDirty = true;
        Debug.LogWarning("Dirtified");
        if(_isDirty)
        {
            SkinnedMeshRenderer r = GetComponentInChildren<SkinnedMeshRenderer>();
            if(r != null)
            {
                r.materials = new Material[] { _handMaterial, _outlineTexture };
            }
        }
    }

    public void WarningIndicator()
    {
        SkinnedMeshRenderer r = GetComponentInChildren<SkinnedMeshRenderer>();
        if(r!=null)
        {
            r.materials = new Material[] { _handMaterial, _warningOutlineTexture };
        }
    }

    public void Cleaned()
    {
        _isDirty = false;
    }
    public void ToggleWet()
    {
        IsWet = true;
    }

    public void PlayVFX()
    {
        if (_isDirty && !_hasSpawnedVFX)
            StartCoroutine(SpawnVFXWithDelay());
    }

    IEnumerator WetToggle()
    {
        yield return new WaitForSeconds(5F);
        IsWet = false;
    }

    private IEnumerator SpawnVFXWithDelay()
    {
        _hasSpawnedVFX = true;
        SpawnManager.Instance.SpawnVFX(VFXType.STINKY, HandWashCollider.transform, 3F);
        yield return new WaitForSeconds(5f); // Delay before it can spawn again
        _hasSpawnedVFX = false;
    }
    #endregion
}
