using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Trashable))]
public abstract class Equipment : MonoBehaviour 
{
#region Members

    public bool IsClean { get; private set; }

    [SerializeField] protected Material _cleanMat, _dirtyMat;
    protected Vector3 _startPosition;

    // DIRTY MECHANIC
    protected int _usageCounter; // counter to know how many times equipment has been used
    protected int _maxUsageCounter; // max counter before it gets dirty

#endregion

#region Unity_Methods

    protected virtual void Start() 
    {
        _startPosition = transform.position;
        _usageCounter = 0;

        IsClean = true;
        GetComponent<MeshRenderer>().material = _cleanMat;

        GameManager.Instance.OnStartService += ResetPosition;
    }
    protected void OnDestroy() 
    {
        ResetPosition();
        GameManager.Instance.OnStartService -= ResetPosition;
    }
    protected virtual void OnTriggerEnter(Collider other) 
    {
        if (other.gameObject.GetComponent<Sponge>() == null) return;
        
        Sponge sponge = other.gameObject.GetComponent<Sponge>();

        if (!sponge.IsWet) return;

        // sponge contaminates the equipment
        if (IsClean && !sponge.IsClean)
        {
            GetComponent<MeshRenderer>().material = _dirtyMat;
            _usageCounter = _maxUsageCounter;
            IsClean = false;
            return;
        }

        if (!IsClean && sponge.IsClean)
            DoCleaning();
    }
    protected void OnTriggerExit(Collider other)
    {
        if (other.gameObject.GetComponent<Sponge>() != null)
            StopCoroutine(CleanEquipment());
    }
    
#endregion

#region Public

    public void HitTheFloor()
    {
        _usageCounter = _maxUsageCounter;
        IncrementUseCounter();
        ResetPosition();

        Debug.Log($"{name} has hit the floor!");
    }
    public void IncrementUseCounter()
    {
        _usageCounter++;

        if (_usageCounter >= _maxUsageCounter)
        {
            _usageCounter = _maxUsageCounter;
            GetComponent<MeshRenderer>().material = _dirtyMat;
            IsClean = false;    
        }
    }

#endregion
    
    protected void ResetPosition() 
    {
        transform.position = _startPosition;
        transform.rotation = Quaternion.identity;
    }
    
#region Cleaning

    void ToggleClean() 
    {
        IsClean = !IsClean;

        // ternary operator syntax -> condition ? val_if_true : val_if_false
        GetComponent<MeshRenderer>().material = IsClean ? 
                                                _cleanMat : _dirtyMat;

        if (IsClean)
            _usageCounter = 0;
    }    
    protected virtual void DoCleaning()
    {
        SpawnManager.Instance.SpawnVFX(VFXType.BUBBLE, transform, 3f);
        
        StopCoroutine(CleanEquipment());
        StartCoroutine(CleanEquipment());
    }
    protected IEnumerator CleanEquipment()
    {
        yield return new WaitForSeconds(2f);

        if (!IsClean)
            ToggleClean();
    }

#endregion
}
