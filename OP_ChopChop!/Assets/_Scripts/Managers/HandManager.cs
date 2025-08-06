using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class HandManager : Singleton<HandManager>
{
    #region Members

    [SerializeField] private Collider[] _handWashColliders;
    [SerializeField] private HandWashing[] _handWashingScripts;
    [SerializeField] private GameObject[] _vfxStinky;
    [SerializeField] private GameObject[] _vfxBubbles;
    [SerializeField] private int _handUsage;

    private List<XRGrabInteractable> _grabbableGI = new List<XRGrabInteractable>();

    #endregion

    #region Unity

    protected override void OnApplicationQuit() => base.OnApplicationQuit();
    private void OnEnable()
    {
        HandWashing.OnHandCleaned += ResetHandUsage;
    }
    private void OnDisable()
    {
        HandWashing.OnHandCleaned -= ResetHandUsage;

        foreach (XRGrabInteractable _interactable in _grabbableGI)
        {
            _interactable.selectExited.RemoveListener(DecrementUsage);
        }
    }

    protected override void Awake()
    {
        _handUsage = 15;
        base.Awake();
    }

    private void Start()
    {
        _handWashColliders = new Collider[_handWashingScripts.Length];

        for (int i = 0; i < _handWashingScripts.Length; i++)
        {
            _handWashColliders[i] = _handWashingScripts[i].HandWashCollider;
        }

        for (int i = 0; i < _vfxStinky.Length; i++)
        {
            _vfxStinky[i].SetActive(false);
        }

        ToggleBubblesOff();
    }

    private void FixedUpdate()
    {
        CompareHandUsage();
    }

    private void ResetHandUsage(int _value)
    {
        _handUsage = _value;
    }

#endregion

    #region Helpers

    private void CompareHandUsage()
    {
        if (_handUsage <= 0)
        {
            foreach (Collider collider in _handWashColliders)
            {
                collider.gameObject.GetComponent<HandWashing>().Dirtify();
            }
            for (int i = 0; i < _vfxStinky.Length; i++)
            {
                _vfxStinky[i].SetActive(true);
            }
        }
        else if (_handUsage <= 15)
        {
            foreach (Collider collider in _handWashColliders)
            {
                collider.enabled = true;
                collider.gameObject.GetComponent<HandWashing>().WarningIndicator();
            }
        }
        else if (_handUsage > 15)
        {
            foreach (Collider collider in _handWashColliders)
            {
                collider.gameObject.GetComponent<HandWashing>().Cleaned();
                collider.enabled = false;
            }
            for (int i = 0; i < _vfxStinky.Length; i++)
            {
                _vfxStinky[i].SetActive(false);
            }
        }
    }

    public void ToggleBubblesOn()
    {
        for (int i = 0; i < _vfxBubbles.Length; i++)
        {
            _vfxBubbles[i].SetActive(true);
        }
    }

    public void ToggleBubblesOff()
    {
        for (int i = 0; i < _vfxBubbles.Length; i++)
        {
            _vfxBubbles[i].SetActive(false);
        }
    }

    public void DecrementUsage(SelectExitEventArgs args)
    {
        _handUsage--;
        // Debug.LogWarning($"Oh no, my hand is getting diry! {_handUsage}");
    }

    public void RegisterGrabbable(XRGrabInteractable _interactable)
    {
        if (!_grabbableGI.Contains(_interactable))
        {
            _grabbableGI.Add(_interactable);
            _interactable.selectExited.AddListener(DecrementUsage);
        }
    }

    #endregion
}
