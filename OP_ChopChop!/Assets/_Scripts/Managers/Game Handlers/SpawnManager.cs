using System.Collections.Generic;
using System.Collections;
using UnityEngine;

/// </summary> -WHAT DOES THIS SCRIPT DO-
///
/// Spawns different objects that's needed in the game.
///
/// </summary>

public class SpawnManager : StaticInstance<SpawnManager>
{
#region Members

    [Header("Prefabs"), Tooltip("0 = smoke, 1 = bubble, 2 = sparkle, 3 = stinky")]
    [SerializeField] GameObject[] _vfxPrefabs;
    [SerializeField] GameObject _customerPrefab;

    [Header("Instantiated Bins"), Tooltip("0 = ingredients, 1 = foods, 2 = dishes, 3 = customers, 4 = VFXs")]
    [SerializeField] Transform[] _bins; // avoids clutters in the hierarchy  

    [Header("Customer Components")]
    [SerializeField] CustomerSeat[] _customerSeats;
    [SerializeField] ColliderCheck[] _colliderChecks;
    List<GameObject> _seatedCustomers = new List<GameObject>();
    const int MAX_CUSTOMER_COUNT = 4;

    [Header("Customer Spawning Timers"), Tooltip("Can be changed to use for testing")]
    [SerializeField] float _initialCustomerSpawnTime; // 2s
    [SerializeField] float _customerSpawnInterval;    // 10s

#endregion

#region Unity_Methods

    protected override void Awake() => base.Awake();
    protected override void OnApplicationQuit() => base.OnApplicationQuit();
    
    void Start() // BIND TO EVENTS
    {
        GameManager.Instance.OnStartService += StartCustomerSpawning;
        GameManager.Instance.OnEndService += ClearCustomerSeats;
    }
    /*
    void OnDestroy() // UNBIND FROM EVENTS
    {
        GameManager.Instance.OnStartService -= StartCustomerSpawning;
        GameManager.Instance.OnEndService -= ClearCustomerSeats;
    } */
    
    IEnumerator CreateCustomer()
    {
        yield return new WaitForSeconds(_initialCustomerSpawnTime);
        SpawnCustomer(GiveAvaiableSeat());

        while (GameManager.Instance.CurrentShift == GameShift.SERVICE)
        {
            yield return new WaitForSeconds(_customerSpawnInterval);  
            SpawnCustomer(GiveAvaiableSeat());

            // coroutine should stop spawning once all seats are full
            if (_seatedCustomers.Count == MAX_CUSTOMER_COUNT) yield break;   
        }
    }
    
#endregion

#region Spawning

    public void SpawnVFX(VFXType type, Transform t, float destroyTime)
    {   
        GameObject vfxInstance = Instantiate(_vfxPrefabs[(int)type], 
                                             t.position, t.rotation,
                                             _bins[4]);

        Destroy(vfxInstance, destroyTime);
    }
    public GameObject SpawnObject(GameObject obj, Transform t, SpawnObjectType type)
    {
        return Instantiate(obj,
                           t.position, t.rotation,
                           _bins[(int)type]);
    }    
    void SpawnCustomer(int idx)
    {
        if (idx == -1) return;

        if (GameManager.Instance.CurrentShift != GameShift.SERVICE) return;
        
        GameObject customer = SpawnObject(_customerPrefab, 
                                          _customerSeats[idx].transform, 
                                          SpawnObjectType.CUSTOMER);

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

#endregion

#region Customer_Helpers

    public void RemoveCustomer(GameObject customer) 
    {
        int idx = customer.GetComponent<CustomerActions>().SeatIndex;

        _seatedCustomers.Remove(customer);

        _customerSeats[idx].IsEmpty = true;
        _colliderChecks[idx].CustomerOrder = null;
    }   
    int GiveAvaiableSeat() // sets the index where the customer should sit
    {
        for (int i = 0; i < _customerSeats.Length; i++)
        {
            CustomerSeat seat = _customerSeats[i].gameObject.GetComponent<CustomerSeat>();

            if (seat.IsEmpty)
                return i;
            
            else 
                continue;
        }
        return -1; // all seats are empty
    }

#endregion
    
#region Event_Methods

    public void StartCustomerSpawning() => StartCoroutine(CreateCustomer());
    void ClearCustomerSeats()
    {
        foreach (GameObject obj in _seatedCustomers)
            Destroy(obj);

        _seatedCustomers.Clear();

        foreach (CustomerSeat seat in _customerSeats)
            seat.IsEmpty = true;        

        foreach (ColliderCheck col in _colliderChecks)
            col.CustomerOrder = null;

        StopAllCoroutines(); 
    }
    
#endregion
}
