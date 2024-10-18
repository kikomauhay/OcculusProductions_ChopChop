using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snap : MonoBehaviour
{
    [SerializeField]
    float AttachY;

    private Rigidbody rb;
    private void OnTriggerEnter(Collider other)
    {
       if(other.GetComponent<Sliceable>() != null)
        {

            SnapToObject(other.transform);
            DisableRigidbody(other);
            Debug.Log("Triggered");
        }
    }

    void SnapToObject(Transform FoodObject)
    {
            FoodObject.SetParent(transform);
            FoodObject.localPosition = new Vector3 (0,AttachY,0);
            FoodObject.localRotation = Quaternion.identity;
    }

    void DisableRigidbody(Collider other)
    {
        Rigidbody rb = other.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = true;
        }
    }
}
