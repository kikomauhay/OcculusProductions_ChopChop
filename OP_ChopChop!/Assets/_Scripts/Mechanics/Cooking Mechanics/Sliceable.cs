using UnityEngine;

public class Sliceable : MonoBehaviour
{
#region Members

    // will change this into an array later
    [SerializeField] private GameObject _currentPrefab, _nextPrefab; // make this an array 
    [SerializeField] private GameObject _smokeVFX;
    
    int _chopCounter;
    public bool IsAttached { get; set; }

#endregion

#region Methods

    void Start()
    {
        _chopCounter = 0;
        IsAttached = false;
    }
    void Update()
    {
        if (_chopCounter >= 5)
        {
            Sliced();

            // needs an explanation
            EquipmentManager.Instance.MeatBoard.GetComponent<Snap>().ResetSnap();
        }
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<Knife>() == null) return;

        if (IsAttached)
        {
            _chopCounter++;

            SpawnManager.Instance.SpawnVFX(VFXType.SPARKLE,
                                           transform);

            SoundManager.Instance.PlaySound(Random.value > 0.5f ?
                                            "fish slice 01" :
                                            "fish slice 02");
            Debug.LogWarning("Chopping");
        }
    }   

#endregion

    void Sliced()
    {
        if (_currentPrefab != null)
        {
            Destroy(_currentPrefab);
            
            SpawnManager.Instance.SpawnVFX(VFXType.SMOKE, transform);

            SpawnManager.Instance.SpawnFoodItem(_nextPrefab, 
                                                FoodItemType.INGREDIENT,
                                                transform);
            
            SoundManager.Instance.PlaySound("knife chop");
            Debug.Log("SLICED!");
        }
    }
}