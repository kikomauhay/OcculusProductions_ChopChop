using UnityEngine;
using System;

public class NigiriAssembly : MonoBehaviour
{
    [SerializeField] GameObject _salmonNigiri, _tunaNigiri, _smokeVFX;

    private void OnTriggerEnter(Collider other)
    {
        // for now use Sliceable Component since we're waiting for the ingredient base class
        if (other.gameObject.GetComponent<SalmonIngredient>())
        {
            Vector3 _currentPosition = this.transform.position;
            Quaternion _currentRotation = this.transform.rotation;
            Destroy(this.gameObject);
            Destroy(other.gameObject);
            SpawnVFX(_smokeVFX, _currentPosition, _currentRotation);
            Instantiate(_salmonNigiri, _currentPosition, _currentRotation);
        }
        if (other.gameObject.GetComponent<TunaIngredient>())
        {
            Vector3 _currentPosition = this.transform.position;
            Quaternion _currentRotation = this.transform.rotation;
            Destroy(this.gameObject);
            SpawnVFX(_smokeVFX, _currentPosition, _currentRotation);
            Instantiate(_tunaNigiri, _currentPosition, _currentRotation);
        }
    }

    void SpawnVFX(GameObject _vfxPrefab, Vector3 _position, Quaternion _rotation)
    {
        if (_vfxPrefab != null)
        {
            GameObject _VFXInstance = Instantiate(_vfxPrefab, _position, _rotation);
            Destroy(_VFXInstance, 2f);
        }
    }
}
