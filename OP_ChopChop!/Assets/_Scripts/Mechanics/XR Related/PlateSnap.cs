using System.Collections;
using UnityEngine;

public class PlateSnap : MonoBehaviour
{
    [SerializeField] float _attachY;
    [SerializeField] float _timer;

    public Collider SnapCollider;
    
    void OnTriggerEnter(Collider other)
    {
        //if(_other.GetComponent<Sushi>() != null)
        //{
        //    SnapToObject(_other.transform);
        //    DisableRigidbody(_other);
        //    SnapCollider.enabled = false;
        //}

        if (other.GetComponent<Sponge>() != null)
            SpawnManager.Instance.SpawnVFX(VFXType.BUBBLE, transform, 5f);
        
        else
            StartCoroutine(ResetTrigger());
    }

    void SnapToObject(Transform t)
    {
        t.SetParent(transform);
        t.localPosition = new Vector3(0, _attachY, 0);
        t.localRotation = Quaternion.Euler(0, t.localRotation.eulerAngles.y, 0);
    }

    void DisableRigidbody(Collider _other)
    {
        Rigidbody _rb = _other.GetComponent<Rigidbody>();
        
        if (_rb != null)
        {
            _rb.isKinematic = true;
        }
    }

    IEnumerator ResetTrigger()
    {
        yield return new WaitForSeconds(_timer);
        SnapCollider.enabled = true;
    }
}