using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandManager : MonoBehaviour
{
    [SerializeField] Collider[] _handWashColliders;

    private int _handUsage;

    private void Awake()
    {
        _handUsage = 30;

        foreach (Collider collider in _handWashColliders)
        {
            collider.enabled = false;
        }
    }

    private void FixedUpdate()
    {
        //test
        if(Input.GetKeyUp(KeyCode.S))
        {
            DecrementUsage();
        }

        if (_handUsage < 15)
        {
            foreach(Collider collider in _handWashColliders)
            {
                collider.enabled = true;
                collider.gameObject.GetComponent<HandWashing>().WarningIndicator();
            }
        }
        else if(_handUsage <= 0)
        {
            foreach(Collider collider in _handWashColliders)
            {
                collider.gameObject.GetComponent<HandWashing>().Dirtify();
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

#region public functions

    public void DecrementUsage()
    {
        _handUsage--;
    }

#endregion
}
