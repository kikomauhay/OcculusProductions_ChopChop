using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentCleaning : MonoBehaviour
{
    [SerializeField] GameObject _bubblesVfx;
    [SerializeField] CleanManager _cleanManager;
    [SerializeField] float _maxChange;
    private void OnTriggerEnter(Collider other)
    {
        Rigidbody _rb = other.GetComponent<Rigidbody>();
        Vector3 _lastVelocity = _rb.velocity;
        float dif = Mathf.Abs((_rb.velocity - _lastVelocity).magnitude/Time.fixedDeltaTime);

        if (other.gameObject.GetComponent<Sponge>() == null) return;

        if(other.gameObject.GetComponent<Sponge>().IsWet)
        {
            Vector3 _currentPosition = other.transform.position;
            Quaternion _currentRotation = other.transform.rotation;
            SpawnVFX(_bubblesVfx, _currentPosition, _currentRotation);
            _cleanManager.IncreaseCleanRate();
        }
    }

    void SpawnVFX(GameObject vfxPrefab, Vector3 position, Quaternion rotation)
    {
        if (vfxPrefab != null)
        {
            GameObject VFXInstance = Instantiate(vfxPrefab, position, rotation);
            Destroy(VFXInstance, 2f);
        }
    }
}
