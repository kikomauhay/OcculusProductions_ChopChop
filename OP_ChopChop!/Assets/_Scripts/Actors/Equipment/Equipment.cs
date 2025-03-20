using System.Collections;
using UnityEngine;

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
    protected void Reset() 
    {
        ResetPosition();
        GameManager.Instance.OnStartService -= ResetPosition;
    }
    
#endregion
    
#region Cleaning

    public void ToggleClean() 
    {
        IsClean = !IsClean;

        // ternary operator syntax -> condition ? val_if_true : val_if_false
        GetComponent<MeshRenderer>().material = IsClean ? 
                                                _cleanMat : _dirtyMat;
    }
    public void ResetUsageCounter() => _usageCounter = 0;

    protected void IncrementCounter()
    {
        _usageCounter++;

        if (_usageCounter == _maxUsageCounter)
        {
            ToggleClean();
            Debug.LogWarning($"{name} got dirty from being used too much!");
        }
    }

#endregion

#region Reposition

    public void ResetPosition() => StartCoroutine(Reposition());

    IEnumerator Reposition()
    {
        yield return new WaitForSeconds(Random.Range(7f, 10f));
        transform.position = _startPosition;
    }

#endregion

}
