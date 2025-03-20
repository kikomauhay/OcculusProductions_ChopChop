using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Snap : MonoBehaviour
{
    [SerializeField] float timer;
    [SerializeField] Collider SnapCollider;

    private void OnTriggerEnter(Collider other)
    {
       if (other.gameObject.GetComponent<Sliceable>() != null)
       {
            other.gameObject.GetComponent<Sliceable>().IsAttached = true;
            other.gameObject.GetComponent<XRGrabInteractable>().enabled = false;
            other.gameObject.GetComponent<Sliceable>().SetSnap(SnapCollider);

            SnapToObject(other.transform);
            DisableRigidBody(other);

            SnapCollider.enabled = false;
            StartCoroutine(DelayedSetting(other));
       }
       else StartCoroutine(ResetTrigger());
    }

    void SnapToObject(Transform foodObject)
    {
        foodObject.position = SnapCollider.transform.position;
        foodObject.rotation = Quaternion.Euler(0, foodObject.rotation.eulerAngles.y, 0);
    }

    void SetCollider(Collider other)
    {
        if (other.GetComponent<Collider>() != null)
        {
            other.GetComponent<Collider>().isTrigger = true;
        }
    }


    void DisableRigidBody(Collider other)
    {
        if (other.GetComponent<Rigidbody>() != null)
        {
            other.GetComponent<Rigidbody>().isKinematic = true;
        }
    }

    public void CallReset() => StartCoroutine(ResetTrigger());
    

    private IEnumerator ResetTrigger()
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
