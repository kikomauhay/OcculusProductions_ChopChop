using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CustomerSpawningManager : Singleton<CustomerSpawningManager>
{
    protected override void Awake() { base.Awake(); }

    [Header("Arrays")]
    [SerializeField] private GameObject[] customerSpawnPoints;
    [SerializeField] private Transform[] customerSeatingPoints;
    [SerializeField] private List<GameObject> listOfCustomersInWaiting;
   

    [Header("CustomerVariable")]
    [SerializeField] private GameObject[] customerModelPrefab; //This is the prefab for the Customer itself

    [Header("Variable Counts")]
    [SerializeField] private int maxCustomerToSpawn;
    [SerializeField] private int currentCustomerCount;

    [Header("Timer")]
    [SerializeField] private float nextCustomerTimer;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(ITimerForNextCustomerSpawn()); //Pls put this under a condition once testing is done
    }

    // Update is called once per frame
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
        //int ranNum = Random.Range(0, 1); //for spawning customer variant

        if(currentCustomerCount >= customerSpawnPoints.Length) 
        { 
            StopCoroutine(ITimerForNextCustomerSpawn());
            return;
        }

        for (int i = 0; i < customerSpawnPoints.Length; i++)
        {
            if(currentCustomerCount <= maxCustomerToSpawn)
            {
                if (!customerSpawnPoints[i].GetComponent<SpawnLocationScript>()._isPrefabPresent)
                {
                    GameObject createdCustomer = Instantiate(customerModelPrefab[0],
                                                           customerSpawnPoints[i].transform.position,
                                                           Quaternion.identity);

                    //createdCustomer.GetComponent<CustomerOrder>()._getSetSeatNumber = customerSpawnPoints[i];

                    currentCustomerCount++;
                    listOfCustomersInWaiting.Add(createdCustomer);

                    customerSpawnPoints[i].gameObject.GetComponent<SpawnLocationScript>()._isPrefabPresent = true;
                    break;
                }
            } 
        }


        StartCoroutine(ITimerForNextCustomerSpawn());
    }
    

    public bool IsEmptySpawnLocation()
    {
        Debug.Log("isEmptyPlaying");
        for (int i = 0; i < customerSpawnPoints.Length; i++)
        {
            
            if (customerSpawnPoints[i].gameObject.GetComponent<SpawnLocationScript>()._isPrefabPresent == false)
            {
                Debug.Log("IsEmpty True");
                return true;
            }
            
        }
        Debug.Log("IsEmpty False");
        return false;
    }

    IEnumerator ITimerForNextCustomerSpawn()
    {
        yield return new WaitForSeconds(nextCustomerTimer);

        if(IsEmptySpawnLocation())
        {
            DoSpawnCustomer();
        }
    }

    public void RemoveCustomer(GameObject customer)
    {
        listOfCustomersInWaiting.Remove(customer);
    }

    protected override void OnApplicationQuit() { base.OnApplicationQuit(); }
}
