using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CustomerOrder : MonoBehaviour
{
    [SerializeField] private BoxCollider customerBoxCollider;
    [SerializeField] private GameObject[] sushiOrder; //theOrder of the customer
    [SerializeField] private Transform customerOrderSpawnLocation;

    [SerializeField] private float currentCustomerPaitenceTimer; //use this to take the score for the customer rating
    [SerializeField] private float maxCustomerPaitenceTimer;

    [SerializeField] private float customerTasteValue; //value that we'll use to check the dish decay value against
    [SerializeField] private float maxCustomerTaseValue;
    [SerializeField] private float minCustomerTaseValue;


    // Start is called before the first frame update
    void Start()
    {
        customerTasteValue = Random.Range(minCustomerTaseValue, maxCustomerTaseValue); 
        // at start, each customer will have a random value for their taste value
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SpawnCustomerOrder()
    { 
        GameObject customerOrder = Instantiate(sushiOrder[0], 
                                               customerOrderSpawnLocation.position, 
                                               customerOrderSpawnLocation.rotation);

        //customerOrder.GetComponent<SushiDishUI>().MaxTime = maxCustomerPaitenceTimer; //setting the timer
        
    }
}
