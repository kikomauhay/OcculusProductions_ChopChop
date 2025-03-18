using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snap : MonoBehaviour
{
    [SerializeField]
    float timer;

    [SerializeField]
    Collider SnapCollider;

    public void ResetSnap()
    {
        SnapCollider.enabled = true;
    }

    private void OnTriggerEnter(Collider _other)
    {
       if(_other.gameObject.GetComponent<Sliceable>() != null)
        {
            
            _other.gameObject.GetComponent<Sliceable>().IsAttached = true;

            SnapToObject(_other.transform);
            DisableRigidBody(_other);
            Debug.Log("Snapped!!");
            SnapCollider.enabled = false;
            StartCoroutine(DelayedSetting(_other));
        }
       else
        {
            StartCoroutine(IResetTrigger());
        }
    }

    void SnapToObject(Transform _foodObject)
    {
            _foodObject.localPosition = SnapCollider.transform.position;
            _foodObject.localRotation = Quaternion.Euler(0, _foodObject.localRotation.eulerAngles.y, 0);
    }

    void SetCollider(Collider _other)
    {
        if(_other.GetComponent<Collider>() != null)
        {
            _other.GetComponent<Collider>().isTrigger = true;
        }
    }

    void DisableRigidBody(Collider other)
    {
        Rigidbody rb = other.GetComponent<Rigidbody>();
        if(rb != null)
        {
            rb.isKinematic = true;
        }
    }
    public void CallReset()
    {
        StartCoroutine(IResetTrigger());
    }

    private IEnumerator IResetTrigger()
    {
        Debug.Log(timer);
        yield return new WaitForSeconds(timer);
        ResetSnap();
    }

    private IEnumerator DelayedSetting(Collider _other)
    {
        //Delay setting the collider to trigger
        yield return new WaitForSeconds(1.5F);
        SetCollider(_other);
    }
}
