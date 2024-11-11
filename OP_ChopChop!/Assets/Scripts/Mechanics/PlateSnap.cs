using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateSnap : MonoBehaviour
{
    [SerializeField]
    float AttachY;
    [SerializeField]
    float Timer;

    public Collider SnapCollider;
    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<SalmonNigiri>() != null)
        {
            SnapToObject(other.transform);
            DisableRigidbody(other);
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

    void DisableRigidbody(Collider other)
    {
        Rigidbody rb = other.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = true;
        }
    }

    private IEnumerator IResetTrigger()
    {
        yield return new WaitForSeconds(Timer);
        SnapCollider.enabled = true;
    }
}
