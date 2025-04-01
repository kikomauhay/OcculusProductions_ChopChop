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

    private void Awake()
    {
        _handUsage = 20;        
    }

    void Start()
    { 
        _handWashCollider.enabled = false;
        _isDirty = true;
        IsWet = false;
        _timer = 20F;
        _hasSpawnedVFX = false;

        // Debug.Log($"Hand Dirty is {_isDirty}");
    }

    private void FixedUpdate()
    {
        //test
        if(Input.GetKeyUp(KeyCode.S))
        {
            DecrementUsage();
        }

        if (_handUsage < 10)
        {
            _handWashCollider.enabled = true;
            WarningIndicator();
        }
        if (_handUsage <= 0)
        {
            Dirtify();

            if(!_hasSpawnedVFX)
            {
                _hasSpawnedVFX = true;
               StartCoroutine(SpawnVFXWithDelay());
            }
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
                Dirtify();

                if (!_hasSpawnedVFX)
                {
                    _hasSpawnedVFX = true;
                    StartCoroutine(SpawnVFXWithDelay());
                }
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

    private void Dirtify()
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

    private void WarningIndicator()
    {
        SkinnedMeshRenderer r = GetComponentInChildren<SkinnedMeshRenderer>();
        if(r!=null)
        {
            r.materials = new Material[] { _handMaterial, _warningOutlineTexture };
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
        Debug.LogWarning(_handUsage);
        _handUsage--;
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
