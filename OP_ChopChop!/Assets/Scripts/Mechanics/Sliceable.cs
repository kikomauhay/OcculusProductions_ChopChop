using UnityEngine;

public class Sliceable : MonoBehaviour
{
#region Members
    
    [SerializeField] private GameObject _currentPrefab, _nextPrefab;
    [SerializeField] private GameObject _sharpObject, _meatBoard;
    [SerializeField] private GameObject _smokeVFX;
    private int _chopCounter;
    public bool IsAttached;

#endregion

#region Methods

    void Start()
    {
        _sharpObject = EquipmentManager.Instance?.Knife;
        _meatBoard = EquipmentManager.Instance?.MeatBoard;
        IsAttached = false;
        _chopCounter = 0;
    }
    void Update()
    {
        if (_chopCounter >= 5)
        {
            Sliced();

            // needs an explanation
            _meatBoard.gameObject.GetComponent<Snap>().ResetSnap();
        }
    }
    void OnTriggerEnter(Collider other)
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
                _chopCounter++;

                SpawnVFX(_smokeVFX, _currentPosition, _currentRotation);
                SoundManager.Instance.PlaySound(Random.value > 0.5f ? "fish slice 01" : "fish slice 02");
                Debug.Log("Chopping");
            }
        }
    }   

#endregion

    void Sliced()
    {
        if (_currentPrefab != null)
        {
            // gets the position and rotation of prefab and then destroys it
            Vector3 _currentPosition = _currentPrefab.transform.position;
            Quaternion _currentRotation = _currentPrefab.transform.rotation;

            Destroy(_currentPrefab);
            SpawnVFX(_smokeVFX, _currentPosition, _currentRotation);
            Instantiate(_nextPrefab, _currentPosition, _currentRotation);
            
            SoundManager.Instance.PlaySound("knife chop");
            Debug.Log("SLICED!");
        }
    }
    void SpawnVFX(GameObject vfxPrefab, Vector3 position, Quaternion rotation)
    {
        if(vfxPrefab != null)
        {
            GameObject VFXInstance = Instantiate(vfxPrefab, position, rotation);
            Destroy(VFXInstance, 2f);
        }    
    }
}