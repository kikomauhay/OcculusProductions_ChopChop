using System.Collections;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(Trashable))]
public class Sponge : MonoBehaviour
{
    #region Properties

    public bool IsWet => _isWet;
    public bool IsClean => _isClean;

    #endregion
    #region SerializeField

    [SerializeField] private bool _isClean, _isWet;
    [SerializeField] private Material _wetMat, _cleanMat, _dirtyMat, _dirtyOSM;

    #endregion
    #region Private

    private MeshRenderer _rend;
    private XRGrabInteractable _interactable;
    private Vector3 _startPosition;
    private Quaternion _startRotation;
    private const float WET_DURATION = 30f;

    #endregion

    #region Unity

    private void Awake()
    {
        _rend = GetComponent<MeshRenderer>();
        _interactable = GetComponent<XRGrabInteractable>();

        if (!_isClean)
            Debug.LogWarning($"{this} is clean: {_isClean}");

        if (_isWet)
            Debug.LogWarning($"{this} is wet: {_isWet}");
    }
    private void Start()
    {
        GameManager.Instance.OnStartService += ResetPosition;
        OnBoardingHandler.Instance.OnTutorialEnd += ResetPosition;

        name = "Sponge";
        _isWet = false;
        _isClean = true;

        _startPosition = transform.position;
        _startRotation = transform.rotation;

        if (_interactable != null)
            HandManager.Instance.RegisterGrabbable(_interactable);
    }
    private void OnDestroy()
    {
        GameManager.Instance.OnStartService -= ResetPosition;
        OnBoardingHandler.Instance.OnTutorialEnd -= ResetPosition;
    }

    #endregion
    #region Public

    public void ResetPosition()
    {
        transform.position = _startPosition;
        transform.rotation = _startRotation;
    }
    public void HitTheGround()
    {
        transform.rotation = Quaternion.identity;
        ResetPosition();
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
    #region Enumerators

    private IEnumerator DrySponge()
    {
        SpawnManager.Instance.SpawnVFX(VFXType.BUBBLE, transform, WET_DURATION);
        yield return new WaitForSeconds(WET_DURATION);

        // makes the sponge clean
        _rend.materials = new Material[] { _cleanMat };
        _isWet = false;
    }
    
    #endregion
}
