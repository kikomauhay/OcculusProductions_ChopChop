using System.Collections.Generic;
using System.Collections;
using UnityEngine;

/// </summary> -WHAT DOES THIS SCRIPT DO-
///
/// Spawns different objects that's needed in the game.
///
/// </summary>

public class SpawnManager : Singleton<SpawnManager>
{
#region Members

    [Header("Prefabs"), Tooltip("0 = smoke, 1 = bubble, 2 = sparkle, 3 = stinky")]
    [SerializeField] GameObject[] _vfxPrefabs; 
    [SerializeField] GameObject _platePrefab;

    [Header("Instantiated Bins"), Tooltip("0 = ingredients, 1 = foods, 2 = dishes, 3 = customers, 4 = VFXs")]
    [SerializeField] Transform[] _bins; // avoids clutters in the hierarchy  

    [Header("Customer Components")]
    [SerializeField] GameObject _customerPrefab;
    [SerializeField] CustomerSeat[] _customerSeats;
    [SerializeField] ColliderCheck[] _colliderChecks;
    List<GameObject> _seatedCustomers = new List<GameObject>();
    public int CustomerCount => _seatedCustomers.Count;
    const int MAX_CUSTOMER_COUNT = 4;

#endregion

#region Unity_Methods

    protected override void Awake() => base.Awake();
    protected override void OnApplicationQuit() 
    {
        base.OnApplicationQuit();
        Reset();
    }
    void Start() 
    {
        GameManager.Instance.OnStartService += StartCustomerSpawning;
        GameManager.Instance.OnEndService += ClearCustomerSeats;


    }
    void Reset() 
    {
        GameManager.Instance.OnStartService -= StartCustomerSpawning;
        GameManager.Instance.OnEndService -= ClearCustomerSeats;
    }
    IEnumerator HandleCustomer()
    {
        yield return new WaitForSeconds(5f);
        SpawnCustomer(GiveAvaiableSeat());

        while (GameManager.Instance.CurrentShift == GameShift.SERVICE)
        {
            yield return new WaitForSeconds(10f);  
            SpawnCustomer(GiveAvaiableSeat());
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
        if (idx == -1)
        {
            Debug.LogWarning("All seats are full!");
            return;
        }
        if (GameManager.Instance.CurrentShift != GameShift.SERVICE)
        {
            Debug.LogWarning("Game is not in the service phase!");
            return;
        }

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

    void StartCustomerSpawning() => StartCoroutine(HandleCustomer());
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

#region Testing

    void test()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SpawnVFX((VFXType)Random.Range(0, _vfxPrefabs.Length),
                      transform, 
                      Random.Range(1f, 5f));
        }

        if (Input.GetKeyDown(KeyCode.Delete))
        {
            // SpawnFoodItem(_platePrefab, SpawnObjectType.INGREDIENT, transform);

            RemoveCustomer(_seatedCustomers[Random.Range(0, _seatedCustomers.Count)]);
            Debug.LogWarning("Deleted a random customer");
        }
    }

#endregion
}
