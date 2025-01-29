using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerSpawningManager : Singleton<CustomerSpawningManager>
{
    protected override void Awake() { base.Awake(); }

    [Header("Arrays")]
    [SerializeField] private GameObject[] customerSpawnPoints;
    [SerializeField] private Transform[] customerSeatingPoints;
   

    [Header("CustomerVariable")]
    [SerializeField] private GameObject[] customerModelPrefab; //This is the prefab for the Customer itself
    [SerializeField] private float customerSpeed;

    [Header("Variable Counts")]
    [SerializeField] private int maxCustomerToSpawn;
    [SerializeField] private int currentCustomerCount;

    [Header("Timer")]
    [SerializeField] private float nextCustomerTimer;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(ITimerForNextCustomerSpawn()); //Pls put this under a condition once testing is done
        customerSpeed = customerSpeed * Time.deltaTime;
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

        for(int i = 0; i < customerSpawnPoints.Length; i++)
        {
            if(!customerSpawnPoints[i].GetComponent<SpawnLocationScript>()._IsPrefabPresent)
            {
                GameObject createdCustomer = Instantiate(customerModelPrefab[0], 
                                                        customerSpawnPoints[i].transform.position, 
                                                        Quaternion.identity);
                    
                   

                currentCustomerCount++;

                customerSpawnPoints[i].gameObject.GetComponent<SpawnLocationScript>()._IsPrefabPresent = true;
                break;
            }
        }

        StartCoroutine(ITimerForNextCustomerSpawn());
    }

    /*
    private void CustomerMoveToSeat(GameObject createdCustomer)
    {
        customerSpeed = customerSpeed * Time.deltaTime;

        for (int x = 0; x < customerSeatingPoints.Length; x++)
        {
           
        }

        createdCustomer.GetComponent<CustomerOrder>().SpawnCustomerOrder();
        
    }
    */

    public bool IsEmptySpawnLocation()
    {
        Debug.Log("isEmptyPlaying");
        for (int i = 0; i < customerSpawnPoints.Length; i++)
        {
            if (customerSpawnPoints[i].gameObject.GetComponent<SpawnLocationScript>()._IsPrefabPresent == false)
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

    protected override void OnApplicationQuit() { base.OnApplicationQuit(); }
}
