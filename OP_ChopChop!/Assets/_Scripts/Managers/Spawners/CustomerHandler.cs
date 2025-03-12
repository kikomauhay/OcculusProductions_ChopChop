using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class CustomerHandler : StaticInstance<CustomerHandler>
{
#region Members
  
    [SerializeField] List<GameObject> _seatedCustomers;
    [SerializeField] GameObject[] _customerSeats, _collisionBoxes;
     
    const int MAX_CUSTOMER_COUNT = 4;   
    public int CustomerCount => _seatedCustomers.Count;

#endregion

    protected override void Awake() 
    {
        base.Awake();
        GameManager.Instance.OnStartService += StartCustomerSpawning;
    } 
    protected override void OnApplicationQuit() 
    {
        base.OnApplicationQuit();
        Reset();
    }
    void Reset() => GameManager.Instance.OnStartService -= StartCustomerSpawning;

#region Spawning

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
    void SpawnCustomer(int idx)
    {
        if (idx == -1)
        {
            Debug.LogError("All seats are full!");
            return;
        }

        // calls SpawnMgr to spawn the customer
        GameObject customer = SpawnManager.Instance.SpawnCustomer(_customerSeats[idx].transform);
        CustomerActions actions = customer.GetComponent<CustomerActions>();

        // assigns the index to the seat & collider
        CustomerSeat seat = _customerSeats[idx].GetComponent<CustomerSeat>();
        ColliderCheck collider = _collisionBoxes[idx].GetComponent<ColliderCheck>();

        // links a box collider & seat to the customer
        collider.CustomerOrder = customer.GetComponent<CustomerOrder>();
        _seatedCustomers.Add(customer);

        // sets the actions of the customer
        actions.TargetSeat = seat.transform.position;
        actions.SeatIndex = idx;

        // prevents multiple customers getting the same seat 
        seat.IsEmpty = false;
        Debug.LogWarning("Spawning a new customer!");
    }

    public void RemoveCustomer(GameObject customer) 
    {
        int idx = customer.GetComponent<CustomerActions>().SeatIndex;

        _seatedCustomers.Remove(customer);

        _customerSeats[idx].GetComponent<CustomerSeat>().IsEmpty = true;
        _collisionBoxes[idx].GetComponent<ColliderCheck>().CustomerOrder = null;

        // StartCoroutine(HandleCustomer());
    }

    void StartCustomerSpawning() => StartCoroutine(HandleCustomer());
    
#endregion

#region Enumerators

    public IEnumerator HandleCustomer()
    {
        
        while (GameManager.Instance.CurrentShift == GameShift.SERVICE)
        {
            yield return new WaitForSeconds(2f);
            SpawnCustomer(GiveAvaiableSeat());
        }

        // Debug.LogWarning("All seats are full!");
    }

#endregion
}