using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snap : MonoBehaviour
{
    [SerializeField] float timer;
    [SerializeField] Collider SnapCollider;

    private void OnTriggerEnter(Collider other)
    {
       if (other.gameObject.GetComponent<Sliceable>() != null)
       {
            other.gameObject.GetComponent<Sliceable>().IsAttached = true;

            SnapToObject(other.transform);
            // DisableRigidBody(_other);

            SnapCollider.enabled = false;
            StartCoroutine(DelayedSetting(other));
       }
       else StartCoroutine(ResetTrigger());
    }

    void SnapToObject(Transform foodObject)
    {
        foodObject.localPosition = SnapCollider.transform.position;
        foodObject.localRotation = Quaternion.Euler(0, foodObject.localRotation.eulerAngles.y, 0);
    }

    void SetCollider(Collider other)
    {
        if (other.GetComponent<Collider>() != null)
        {
            other.GetComponent<Collider>().isTrigger = true;
        }
    }
    
    /*
    void DisableRigidBody(Collider other)
    {
        Rigidbody rb = other.GetComponent<Rigidbody>();
        if(rb != null)
        {
            rb.isKinematic = true;
        }
    }
    */
/*    public void CallReset()
    {
        StartCoroutine(ResetTrigger());
    }*/

    public IEnumerator ResetTrigger()
    {
        yield return new WaitForSeconds(timer);
        SnapCollider.enabled = true;
    }

    private IEnumerator DelayedSetting(Collider other)
    {
        //Delay setting the collider to trigger
        yield return new WaitForSeconds(1.5f);
        SetCollider(other);
    }
}
