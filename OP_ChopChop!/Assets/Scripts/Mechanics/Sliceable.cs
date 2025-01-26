using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sliceable : MonoBehaviour
{
    [SerializeField]
    GameObject _currentPrefab;

    [SerializeField]
    GameObject _nextPrefab;

    [SerializeField]
    GameObject _sharpObject;

    [SerializeField]
    GameObject _meatBoard;

    [SerializeField]
    GameObject _smokeVFX;

    int _chopCounter;
    public bool IsAttached = false;

    private void Start()
    {
        _sharpObject = EquipmentManager.Instance?.Knife;
        _meatBoard = EquipmentManager.Instance?.MeatBoard;
    }

    // Update is called once per frame
    void Update()
    {
        if (_chopCounter >= 5)
        {
            Debug.Log("SLICED");
            Sliced();
            _meatBoard.gameObject.GetComponent<Snap>().ResetSnap();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // check other gameobject if it has a knife script
        Knife knife = other.gameObject.GetComponent<Knife>();

        // if not null, +1 on chop counter
        if (IsAttached)
        {
            if (knife != null)
            {
                Vector3 _currentPosition = _currentPrefab.transform.position;
                Quaternion _currentRotation = _currentPrefab.transform.rotation;
                Debug.Log("Chopping");
                SpawnVFX(_smokeVFX, _currentPosition, _currentRotation);
                _chopCounter++;
            }

        }
    }

    void Sliced()
    {
        if(_currentPrefab != null)
        {
            // Get pos and rotation of prefab and then destroy
            Vector3 _currentPosition = _currentPrefab.transform.position;
            Quaternion _currentRotation = _currentPrefab.transform.rotation;

            Destroy(_currentPrefab);
            SpawnVFX(_smokeVFX, _currentPosition, _currentRotation);
            Instantiate(_nextPrefab, _currentPosition, _currentRotation);

        }
    }

    void SpawnVFX(GameObject _vfxPrefab, Vector3 _position, Quaternion _rotation)
    {
        if(_vfxPrefab != null)
        {
            GameObject VFXInstance = Instantiate(_vfxPrefab, _position, _rotation);
            Destroy(VFXInstance, 2f);
        }    
    }
}