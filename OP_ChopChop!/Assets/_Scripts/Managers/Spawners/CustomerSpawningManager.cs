using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class CustomerSpawningManager : StaticInstance<CustomerSpawningManager>
{
#region Methods
  
    [SerializeField] List<GameObject> _seatedCustomers;
    [SerializeField] GameObject[] _customerSeats, _collisionBoxes;             
    [SerializeField] GameObject _customerPrefab;
     
    const int MAX_CUSTOMER_COUNT = 4;
    int _currentCustomerCount; 
    float _minWaitingTime, _maxWaitingTime, _customerWaitTimer;

#endregion

#region Unity_Methods

    protected override void Awake() => base.Awake();
    protected override void OnApplicationQuit() => base.OnApplicationQuit();

    void Start()
    {
        _minWaitingTime = 4f;
        _maxWaitingTime = 7f;
        _customerWaitTimer = 3f;

        Debug.Log("Starting to spawn a customer");
        StartCoroutine(SpawnNextCustomer()); // pls put this under a condition once testing is done
    }

#endregion

#region Spawn_Methods

    void DoSpawnCustomer()
    {
        if (_currentCustomerCount > MAX_CUSTOMER_COUNT)
        {
            Debug.LogError("Too many customers at the moment!");
            return;
        }

        if (_currentCustomerCount > _customerSeats.Length) 
        { 
            StopCoroutine(SpawnNextCustomer());
            return;
        }

        for (int i = 0; i < _customerSeats.Length; i++)
        {
            if (!_customerSeats[i].GetComponent<CustomerSeat>().HasCustomer)
            {
                GameObject customer = SpawnManager.Instance.SpawnCustomer(_customerPrefab,
                                                                          _customerSeats[i].transform);

                Debug.Log("Spanwed a customer");

                // links a box collider to the customer
                _collisionBoxes[i].GetComponent<ColliderCheck>().CustomerOrder = customer.GetComponent<CustomerOrder>();
                _currentCustomerCount++;
                _seatedCustomers.Add(customer);

                Debug.Log("Linked the customer to a collider");

                // prevents multiple links
                _customerSeats[i].gameObject.GetComponent<CustomerSeat>().HasCustomer = true;

                break;
            }
        }
        StartCoroutine(SpawnNextCustomer());
    }

    public bool HasAnEmptySeat()
    {
        for (int i = 0; i < _customerSeats.Length; i++)
        {
            if (!_customerSeats[i].gameObject.GetComponent<CustomerSeat>().HasCustomer)
            {
                Debug.LogWarning("There is an empty seat");
                return true;
            }            
        }

        Debug.LogWarning("There are no empty seats");
        return false;
    }
    public void RemoveCustomer(GameObject customer) => _seatedCustomers.Remove(customer);
    
#endregion

    IEnumerator SpawnNextCustomer()
    {
        _customerWaitTimer = Random.Range(_minWaitingTime, _maxWaitingTime);

        yield return new WaitForSeconds(_customerWaitTimer);

        if (HasAnEmptySeat())
            DoSpawnCustomer();
    }
}
