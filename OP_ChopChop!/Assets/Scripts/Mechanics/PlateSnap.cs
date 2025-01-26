using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateSnap : MonoBehaviour
{
    [SerializeField]
    float _attachY;
    [SerializeField]
    float _timer;
    [SerializeField]
    float _VFXScale;
    [SerializeField]
    GameObject _bubblesVFX;

    public Collider SnapCollider;
    private void OnTriggerEnter(Collider _other)
    {
        if(_other.GetComponent<Sushi>() != null)
        {
            SnapToObject(_other.transform);
            DisableRigidbody(_other);
            SnapCollider.enabled = false;
        }
        else if(_other.GetComponent<Sponge>() != null)
        {
            Vector3 _currentPosition = this.transform.position;
            Quaternion _currentRotation = this.transform.rotation;

            SpawnVFX(_bubblesVFX, _currentPosition, _currentRotation);
        }
        else
        {
            StartCoroutine(IResetTrigger());
        }
    }

    void SnapToObject(Transform _foodObject
        )
    {
        _foodObject.SetParent(transform);
        _foodObject.localPosition = new Vector3(0, _attachY, 0);
        _foodObject.localRotation = Quaternion.Euler(0, _foodObject.localRotation.eulerAngles.y, 0);
    }

    void DisableRigidbody(Collider _other)
    {
        Rigidbody _rb = _other.GetComponent<Rigidbody>();
        if (_rb != null)
        {
            _rb.isKinematic = true;
        }
    }

    void SpawnVFX(GameObject _vfxPrefab, Vector3 _position, Quaternion _rotation)
    {
        if (_vfxPrefab != null)
        {
            GameObject VFXInstance = Instantiate(_vfxPrefab, _position, _rotation);
            VFXInstance.transform.localScale *= _VFXScale;
            Destroy(VFXInstance, 2f);
        }
    }

    private IEnumerator IResetTrigger()
    {
        yield return new WaitForSeconds(_timer);
        SnapCollider.enabled = true;
    }
}