using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class HandManager : Singleton<HandManager>
{
    #region Members

    [SerializeField] private Collider[] _handWashColliders;
    [SerializeField] private HandWashing[] _handWashingScripts;
    [SerializeField] private GameObject[] _vfxStinky;
    [SerializeField] private int _handUsage;

    private XRGrabInteractable _knifeGI;

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
        _knifeGI.selectExited.RemoveListener(DecrementUsage);
    }

    protected override void Awake()
    {
        _handUsage = 5;
        base.Awake();
    }

    private void Start()
    {
        _handWashColliders = new Collider[_handWashingScripts.Length];

        for (int i = 0; i < _handWashingScripts.Length; i++)
        {
            _handWashColliders[i] = _handWashingScripts[i].HandWashCollider;
        }

        //QoL update this, attach stinky to hands and just toggle them
        for (int i = 0; i < _vfxStinky.Length; i++)
        {
            _vfxStinky[i].SetActive(false);
        }
    }

    private void Update()
    {
        //for inspector testing
        if (Input.GetKeyUp(KeyCode.Backspace))
        {
            Debug.LogWarning($"Hand cleanliness: {_handUsage}");
            //DecrementUsage();
        }
    }

    private void FixedUpdate()
    {
        CompareHandUsage();
    }

    private void ResetHandUsage(int _value)
    {
        Debug.Log("Hello");
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
        else if (_handUsage <= 5)
        {
            foreach (Collider collider in _handWashColliders)
            {
                collider.enabled = true;
                collider.gameObject.GetComponent<HandWashing>().WarningIndicator();
            }
        }
        else if (_handUsage > 5)
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


    public void DecrementUsage(SelectExitEventArgs args)
    {
        _handUsage--;
        Debug.LogWarning($"Oh no, my hand is getting diry! {_handUsage}");

        // CompareHandUsage();
    }

    public void SetKnife(Knife knife)
    {
        _knifeGI = knife.GetComponent<XRGrabInteractable>();

        _knifeGI.selectExited.AddListener(DecrementUsage);
    }

    #endregion
}
