using System.Collections.Generic;
using System.Collections;
using UnityEngine;

/// </summary> -WHAT DOES THIS SCRIPT DO-
///
/// Spawns anything that comes out of the game
/// Uses events to handle different spawning types
///
/// </summary>

public class SpawnManager : Singleton<SpawnManager>
{
#region Members

    public int CustomerCount => _seatedCustomers.Count;
    const int MAX_CUSTOMER_COUNT = 4;   

    [Header("Prefabs"), Tooltip("0 = smoke, 1 = bubble, 2 = sparkle, 3 = stinky")]
    [SerializeField] GameObject[] _vfxPrefabs; 
    [SerializeField] GameObject _customerPrefab, _platePrefab;
    
    [Header("Instantiated Areas"), Tooltip("0 = ingredients, 1 = foods, 2 = dishes, 3 = customers, 4 = VFXs")]
    [SerializeField] Transform[] _areas; // avoids clutters in the hierarchy  

    [Header("Other Customer Requirements")]
    [SerializeField] CustomerSeat[] _customerSeats;
    [SerializeField] ColliderCheck[] _colliderChecks;
    List<GameObject> _seatedCustomers = new List<GameObject>();

#endregion

#region Unity_Methods

    protected override void Awake() => base.Awake();
    protected override void OnApplicationQuit() => base.OnApplicationQuit();
    
    // add an "if player hat is worn" condition
    void Start() => StartCoroutine(HandleCustomer());
    //void Update() => test();

#endregion

#region Spawn_Methods

    public void SpawnVFX(VFXType type, Transform t)
    {
        GameObject vfxInstance = Instantiate(_vfxPrefabs[(int)type], 
                                             t.position, t.rotation,
                                             _areas[4]);
        
        Destroy(vfxInstance, 2f); // destory time could also be variable
    }
    public void SpawnFoodItem(GameObject obj, SpawnObjectType type, Transform t) 
    {
        obj = Instantiate(obj, t.position, t.rotation,
                          _areas[(int)type]); // will need to test this on H if this really works 
    }
    public GameObject SpawnCustomer(Transform t)
    {
        GameObject obj = Instantiate(_customerPrefab, 
                                     t.position, t.rotation,
                                     _areas[3]);

        Debug.Log("Spawned the customer");

        return obj;
    }    

#endregion

#region Customer_Spawn_Methods

    int GiveAvaiableSeat() // sets the index where the customer should sit
    {
        for (int i = 0; i < _customerSeats.Length; i++)
        {
            CustomerSeat seat = _customerSeats[i].gameObject.GetComponent<CustomerSeat>();

            if (seat.IsEmpty)
            {
                Debug.LogWarning("There is an empty seat!");
                return i;
            }
            else continue;
        }
        return -1; // no seats are empty
    }
    void SpawnCustomer(int idx)
    {
        if (idx == -1)
        {
            Debug.LogError("All seats are full!");
            return;
        }

        GameObject customer = SpawnCustomer(_customerSeats[idx].transform);
        CustomerActions customerActions = customer.GetComponent<CustomerActions>();

        // assigns the index to the seat & collider
        CustomerSeat seat = _customerSeats[idx];
        ColliderCheck collider = _colliderChecks[idx];

        // links a box collider & seat to the customer
        collider.CustomerOrder = customer.GetComponent<CustomerOrder>();
        _seatedCustomers.Add(customer);

        // sets the actions of the customer
        customerActions.TargetSeat = seat.transform.position;
        customerActions.SeatIndex = idx;

        // prevents multiple customers getting the same seat 
        seat.IsEmpty = false;
    }

    public void RemoveCustomer(GameObject customer) 
    {
        int idx = customer.GetComponent<CustomerActions>().SeatIndex;

        _seatedCustomers.Remove(customer);

        _customerSeats[idx].IsEmpty = true;
        _colliderChecks[idx].CustomerOrder = null;
        // Destroy(customer);

        StartCoroutine(HandleCustomer());
    }

    IEnumerator HandleCustomer()
    {
        Debug.LogWarning("Spawning a new customer!");   
        
        while (_seatedCustomers.Count < MAX_CUSTOMER_COUNT)
        {
            yield return new WaitForSeconds(2f);
            SpawnCustomer(GiveAvaiableSeat());
        }

        Debug.LogWarning("All seats are full!");
    }

#endregion

    void test()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SpawnVFX((VFXType)Random.Range(0, _vfxPrefabs.Length),
                      transform);
        }

        if (Input.GetKeyDown(KeyCode.Delete))
        {
            // SpawnFoodItem(_platePrefab, SpawnObjectType.INGREDIENT, transform);

            RemoveCustomer(_seatedCustomers[Random.Range(0, _seatedCustomers.Count)]);
            Debug.LogWarning("Deleted a random customer");
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            SpawnCustomer(GiveAvaiableSeat());
            Debug.LogWarning("Added a random customer");
        }        
    }
}
