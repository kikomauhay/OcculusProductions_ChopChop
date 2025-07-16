using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class HandManager : Singleton<HandManager>
{
    #region Members

    [SerializeField] private Collider[] _handWashColliders;
    [SerializeField] private HandWashing[] _handWashingScripts;
    [SerializeField] private GameObject[] _vfxStinky;
    [SerializeField] private int _handUsage;

    #endregion

    #region Unity

    protected override void Awake() => base.Awake();
    protected override void OnApplicationQuit() => base.OnApplicationQuit();

    private void OnEnable()
    {
        HandWashing.OnHandCleaned += ResetHandUsage;
    }

    private void OnDisable()
    {
        HandWashing.OnHandCleaned -= ResetHandUsage;
    }

    private void Start()
    {
        _handWashColliders = new Collider[_handWashingScripts.Length];

        //for loop, hand wash scripts
        for (int i = 0; i < _handWashingScripts.Length; i++)
        {
            _handWashColliders[i] = _handWashingScripts[i].HandWashCollider;
        }
        //for loop, stinky vfx
        for (int i = 0; i < _vfxStinky.Length; i++)
        {
            _vfxStinky[i].SetActive(false);
        }
    }

    private void FixedUpdate()
    {
        if (_handUsage <= 0)
        {
            foreach(Collider collider in _handWashColliders)
            {
                collider.gameObject.GetComponent<HandWashing>().Dirtify();
                
            }
            for (int i = 0; i < _vfxStinky.Length; i++)
            {
                _vfxStinky[i].SetActive(true);
            }
        }
        else if(_handUsage <= 5)
        {
            foreach(Collider collider in _handWashColliders)
            {
                collider.enabled = true;
                collider.gameObject.GetComponent<HandWashing>().WarningIndicator();
            }
        }
        else if (_handUsage > 5)
        {
            foreach(Collider collider in _handWashColliders)
            {
                collider.gameObject.GetComponent<HandWashing>().Cleaned();
                collider.enabled=false;
            }
            for (int i = 0; i < _vfxStinky.Length; i++)
            {
                _vfxStinky[i].SetActive(false);
            }
        }
    }

    private void ResetHandUsage(int _value)
    {
        _handUsage = _value;
    }

#endregion

#region public functions

    public void DecrementUsage()
    {
        _handUsage--;
        Debug.LogWarning($"Oh no, my hand is getting diry! {_handUsage}");
    }

#endregion
}
