using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snap : MonoBehaviour
{
    [SerializeField]
    float _attachY;
    [SerializeField]
    float timer;

    public Collider SnapCollider;

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
            SetCollider(_other);
            DisableRigidBody(_other);
            Debug.Log("Triggered");
            SnapCollider.enabled = false;
        }
       else
        {
            StartCoroutine(IResetTrigger());
        }
    }

    void SnapToObject(Transform _foodObject)
    {
            _foodObject.SetParent(transform);
            _foodObject.localPosition = new Vector3(0, _attachY, 0);
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

    private IEnumerator IResetTrigger()
    {
        Debug.Log(timer);
        yield return new WaitForSeconds(timer);
        ResetSnap();
    }
}
