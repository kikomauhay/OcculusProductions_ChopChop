using System.Collections;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;

[RequireComponent(typeof(Trashable))]
public class Sponge : MonoBehaviour
{
#region Readers

    public bool IsWet => _isWet;
    public bool IsClean => _isClean;

#endregion

#region Members

    [SerializeField] private int _usageCounter, _maxUsageCounter;
    [SerializeField] private bool _isClean, _isWet;
    [SerializeField] private Material _wetMat, _cleanMat, _dirtyMat;

    private MeshRenderer _rend;
    private Vector3 _startPosition;
    private const float WET_DURATION = 10f;
    
#endregion 

#region Unity

    private void Start()
    {
        name = "Sponge";
        _isWet = false;
        _usageCounter = 0;

        if (_maxUsageCounter == 0)
            _maxUsageCounter = 10;

        _rend = GetComponent<MeshRenderer>();
        _rend.material = _isClean ? _cleanMat : _dirtyMat;
        _startPosition = transform.position;
    }
    
    private IEnumerator DrySponge()
    {
        SpawnManager.Instance.SpawnVFX(VFXType.BUBBLE, transform, WET_DURATION);
        yield return new WaitForSeconds(WET_DURATION);

        // makes the sponge clean
        _rend.material = _cleanMat;
        _isWet = false;
        _usageCounter = 0;
    }

#endregion

#region Public

    public void SetWet() 
    {
        _isWet = true;
        _rend.material = _wetMat;
        StartCoroutine(DrySponge());
    }

    public void ResetPosition() => transform.position = _startPosition;

#endregion
}
