using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class HandManager : Singleton<HandManager>
{
#region Members
    [SerializeField] Collider[] _handWashColliders;
    [SerializeField] HandWashing[] _handWashingScripts;

    public int _handUsage { get; set; }
    #endregion

#region Unity

    protected override void Awake() => base.Awake();
    protected override void OnApplicationQuit() => base.OnApplicationQuit();
    private void Start()
    {
        _handWashColliders = new Collider[_handWashingScripts.Length];
        for (int i = 0; i < _handWashingScripts.Length; i++)
        {
            _handWashColliders[i] = _handWashingScripts[i].HandWashCollider;
        }
        _handUsage = 30;
    }

    private void FixedUpdate()
    {
        //test
        if(Input.GetKeyUp(KeyCode.S))
        {
            DecrementUsage();
            Debug.LogWarning(_handUsage);
        }

        if (_handUsage < 15)
        {
            foreach(Collider collider in _handWashColliders)
            {
                collider.enabled = true;
                collider.gameObject.GetComponent<HandWashing>().WarningIndicator();
            }
        }
        if(_handUsage <= 0)
        {
            foreach(Collider collider in _handWashColliders)
            {
                collider.gameObject.GetComponent<HandWashing>().Dirtify();
                collider.gameObject.GetComponent<HandWashing>().PlayVFX();
            }
        }
        else
        {
            foreach(Collider collider in _handWashColliders)
            {
                collider.gameObject.GetComponent<HandWashing>().Cleaned();
            }
        }
    }

#endregion

#region public functions

    public void DecrementUsage()
    {
        _handUsage--;
    }

#endregion
}
