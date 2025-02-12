using System.Collections.Generic;
using System.Collections;
using UnityEngine;

// might make this a static instance and just get a reference fro SpawnMgr, so it does the actions, not this script specifically
public class CustomerSpawningManager : Singleton<CustomerSpawningManager>
{
#region Methods

    [Header("Arrays")]
    [SerializeField] private GameObject[] customerSpawnPoints;          // petition to change it to _spawnPoints
    [SerializeField] private GameObject[] customerCollisionPoints;      // petition to change it to _collisionBoxes
    [SerializeField] private List<GameObject> listOfCustomersInWaiting; // petition to change it to _seatedCustomers
   
    [Header("CustomerVariable")] // This is the prefab for the Customer itself
    [SerializeField] private GameObject[] customerModelPrefab; // petition to change to _customerPrefabs 

    [Header("Variable Counts")]
    [SerializeField] private int maxCustomerToSpawn;   // petition to change it to _maxCustomerCount
    [SerializeField] private int currentCustomerCount; // petition to change it to _currentCustomerCount

    [Header("Timer")] // just add an underscore before the text it should be good
    [SerializeField] private float minCustomerTimer;
    [SerializeField] private float maxCustomerTimer;
    private float nextCustomerTimer;

#endregion

#region Enumerators

    protected override void Awake() => base.Awake();
    protected override void OnApplicationQuit() => base.OnApplicationQuit();


    void Start()
    {
        StartCoroutine(SpawnNextCustomer()); // pls put this under a condition once testing is done
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

    private void DoSpawnCustomer()
    {
        // int ranNum = Random.Range(0, 1); //for spawning customer variant

        if (currentCustomerCount >= customerSpawnPoints.Length) 
        { 
            StopCoroutine(SpawnNextCustomer());
            return;
        }

        for (int i = 0; i < customerSpawnPoints.Length; i++)
        {
            if (currentCustomerCount <= maxCustomerToSpawn)
            {
                // the current box colllider is empty
                if (!customerSpawnPoints[i].GetComponent<SpawnLocationScript>().IsPrefabPresent)
                {
                    GameObject createdCustomer = Instantiate(customerModelPrefab[0],
                                                             customerSpawnPoints[i].transform.position,
                                                             customerSpawnPoints[i].transform.rotation);

                    // assigns a box collider to the customer
                    customerCollisionPoints[i].GetComponent<CustomerColliderCheck>().CustomerOrder = 
                        createdCustomer?.GetComponent<CustomerOrder>();

                    // Debug.LogWarning("Connected CustomerOrder to CollisionCheck");

                    currentCustomerCount++;
                    listOfCustomersInWaiting.Add(createdCustomer);

                    // prevents multiple links to one box collider
                    customerSpawnPoints[i].gameObject.GetComponent<SpawnLocationScript>().
                        IsPrefabPresent = true;
                    
                    break;
                }
            } 
        }
        StartCoroutine(SpawnNextCustomer());
    }
    
    public bool IsEmptySpawnLocation() // petition to change to HasAnEmptySeat()
    {
        // Debug.Log("isEmptyPlaying");
        for (int i = 0; i < customerSpawnPoints.Length; i++)
        {

            // what the fuck am I looking at
            if (customerSpawnPoints[i].gameObject.GetComponent<SpawnLocationScript>().
                IsPrefabPresent == false)
            {
                // Debug.Log("IsEmpty True");
                return true;
            }            


        }

        Debug.Log("IsEmpty False");
        return false;
    }
    public void RemoveCustomer(GameObject customer) => listOfCustomersInWaiting.Remove(customer);
    
#endregion

    IEnumerator SpawnNextCustomer()
    {
        // waits a random amt of seconds before spanwing a new customer
        nextCustomerTimer = Random.Range(minCustomerTimer, maxCustomerTimer);
        //Debug.Log("Enum CUSTOMER TIMER: " + nextCustomerTimer);

        yield return new WaitForSeconds(nextCustomerTimer);

        if (IsEmptySpawnLocation())
            DoSpawnCustomer();
    }
}
