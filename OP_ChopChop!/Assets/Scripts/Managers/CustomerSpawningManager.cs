using UnityEngine;
using System.Collections;

public class CustomerSpawningManager : Singleton<CustomerSpawningManager>
{
#region Members

    [Header("Customer Components")]
    [SerializeField] private GameObject[] _customerSpawnPoints; // AJ has plans for this
    [SerializeField] private GameObject[] _customerModelPrefab; // this is the prefab for the customers
    [SerializeField] private int _maxCustomerToSpawn, _currentCustomerCount;
    [SerializeField] private float _nextCustomerTimer; // delay time when the next customer will arrive

#endregion

#region Methods

    protected override void Awake() => base.Awake(); 
    protected override void OnApplicationQuit() => base.OnApplicationQuit();

    void Start()
    {
        // add a condition over this once testing is done
        StartCoroutine(SpawnNextCustomer()); 
    }
    void Update()
    {
        /*
        if (currentCustomerCount < customerSpawnPoints.Length)
        {
            StopCoroutine(ITimerForNextCustomerSpawn());
        }
        */
    }

#endregion

    private void SpawnCustomer()
    {
        //int ranNum = Random.Range(0, 1); //for spawning customer variant

        for (int i = 0; i < _customerSpawnPoints.Length; i++)
        {
            if (!_customerSpawnPoints[i].GetComponent<SpawnLocationScript>().IsPrefabPresent)
            {
                GameObject createCustomer = Instantiate(
                    _customerModelPrefab[0], 
                    _customerSpawnPoints[i].transform.position, 
                    Quaternion.identity
                );

                // the customer is now seated 
                _currentCustomerCount++;
                _customerSpawnPoints[i].gameObject.GetComponent<SpawnLocationScript>().IsPrefabPresent = true;

                break;
            }
        }
        StartCoroutine(SpawnNextCustomer());
    }

    public bool IsLocationEmpty()
    {
        Debug.Log("isEmptyPlaying");
        for (int i = 0; i < _customerSpawnPoints.Length; i++)
        {
            // what the fuck does this mean
            if (_customerSpawnPoints[i].gameObject.GetComponent<SpawnLocationScript>().IsPrefabPresent == false)
            {
                Debug.Log("IsEmpty True");
                return true;
            }
        }
        Debug.Log("IsEmpty False");
        return false;

        // you can do return customer.IsPrefabPresent since that's also a boolean
    }

    IEnumerator SpawnNextCustomer()
    {
        yield return new WaitForSeconds(_nextCustomerTimer);

        if(IsLocationEmpty())
            SpawnCustomer();        
    }
}
