using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NigiriAssembly : MonoBehaviour
{
    [SerializeField]
    GameObject _salmonNigiri;

    [SerializeField]
    GameObject _tunaNigiri;

    [SerializeField]
    GameObject _smokeVFX;
    private void OnTriggerEnter(Collider _other)
    {
        //for now use Sliceable Component since we're waiting for the ingredient base class
        if(_other.gameObject.GetComponent<SalmonSlice>())
        {
            Vector3 _currentPosition = this.transform.position;
            Quaternion _currentRotation = this.transform.rotation;
            Destroy(this.gameObject);
            Destroy(_other.gameObject);
            SpawnVFX(_smokeVFX, _currentPosition, _currentRotation);
            Instantiate(_salmonNigiri, _currentPosition, _currentRotation);
        }
        if(_other.gameObject.GetComponent<TunaSlice>())
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
