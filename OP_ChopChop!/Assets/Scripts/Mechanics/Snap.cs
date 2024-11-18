using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snap : MonoBehaviour
{
    [SerializeField]
    float AttachY;
    [SerializeField]
    float Timer;

    public Collider SnapCollider;

    private void OnTriggerEnter(Collider other)
    {
       if(other.gameObject.GetComponent<Sliceable>() != null)
        {
            other.gameObject.GetComponent<Sliceable>().IsAttached = true;

            SnapToObject(other.transform);
            SetCollider(other);
            DisableRigidBody(other);
            Debug.Log("Triggered");
            SnapCollider.enabled = false;
        }
       else
        {
            StartCoroutine(IResetTrigger());
        }
    }

    void SnapToObject(Transform FoodObject)
    {
            FoodObject.SetParent(transform);
            FoodObject.localPosition = new Vector3(0, AttachY, 0);
            FoodObject.localRotation = Quaternion.Euler(0, FoodObject.localRotation.eulerAngles.y, 0);
    }

    void SetCollider(Collider other)
    {
        if(other.GetComponent<Collider>() != null)
        {
            other.GetComponent<Collider>().isTrigger = true;
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

    private IEnumerator IResetTrigger()
    {
        yield return new WaitForSeconds(Timer);
        ResetSnap();
    }

    public void ResetSnap()
    {
        SnapCollider.enabled = true;
    }
}
