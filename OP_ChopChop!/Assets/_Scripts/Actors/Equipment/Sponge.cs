using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Trashable))]
public class Sponge : MonoBehaviour
{
#region Readers

    public bool IsWet => _isWet;
    public bool IsClean => _isClean;

#endregion

#region Members

    [SerializeField] private bool _isClean, _isWet;
    [SerializeField] private Material _wetMat, _cleanMat, _dirtyMat, _dirtyOSM;

    private MeshRenderer _rend;
    private Vector3 _startPosition;
    private const float WET_DURATION = 30f;

#endregion

#region Unity

    private void Awake() 
    {
        _rend = GetComponent<MeshRenderer>();

        if (!_isClean)
            Debug.LogWarning($"{this} is clean: {_isClean}");

        if (_isWet)
           Debug.LogWarning($"{this} is wet: {_isWet}");
    }
    private void Start()
    {
        name = "Sponge";
        _isWet = false;
        _isClean = true;

        _startPosition = transform.position;
    }

    private void OnDestroy()
    {
        if (GameManager.Instance.CurrentShift == GameShift.Training)
            OnBoardingHandler.Instance.OnTutorialEnd -= HitTheFloor;
    }

    #endregion

    #region Public

    public void HitTheFloor()
    {
        transform.position = _startPosition;
        transform.rotation = Quaternion.identity;
        SetDirty();
    }
    public void SetWet() // making the sponge wet also makes it clean 
    {
        _isWet = true;
        _isClean = true;
        _rend.materials = new Material[] { _wetMat };
        StartCoroutine(DrySponge());
    }
    public void SetDirty()
    {
        _isWet = false;
        _isClean = false;
        _rend.materials = new Material[] { _dirtyMat, _dirtyOSM };
    } 

#endregion

    private IEnumerator DrySponge()
    {
        SpawnManager.Instance.SpawnVFX(VFXType.BUBBLE, transform, WET_DURATION);
        yield return new WaitForSeconds(WET_DURATION);

        // makes the sponge clean
        _rend.materials = new Material[] { _cleanMat };
        _isWet = false;
    }
}
