using System.Collections;
using UnityEngine;

public class HandWashing : MonoBehaviour
{
    public bool IsWet { get; private set; }

    [SerializeField] Collider _handWashCollider;
    [SerializeField] bool _isDirty;
    [SerializeField] Material _handMaterial, _outlineTexture, _warningOutlineTexture;

    private float _timer;
    private int _handUsage;
    private bool _hasSpawnedVFX;

    void Start()
    { 
        _handWashCollider.enabled = false;
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

    public void Dirtify()
    {
        _isDirty = true;
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

    IEnumerator WetToggle()
    {
        yield return new WaitForSeconds(5F);
        IsWet = false;
    }

    private IEnumerator SpawnVFXWithDelay()
    {
        SpawnManager.Instance.SpawnVFX(VFXType.STINKY, _handWashCollider.transform, 3F);
        yield return new WaitForSeconds(5f); // Delay before it can spawn again
        _hasSpawnedVFX = false;
    }
    #endregion
}
