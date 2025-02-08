using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public enum OrderType
{
    Nigiri_Salmon,
    //Nigiri_Tuna,
    //Maki_Salmon,
    //Maki_Tuna,
}

public class CustomerOrder : MonoBehaviour
{
    [SerializeField] private Collider customerServeCollider;
    public Collider _getSetCustomerCollider 
    {
        get { return customerServeCollider; }
        set { customerServeCollider = value; }
    }

    [SerializeField] private OrderType dishType; // differernt types of dishes
    
    [SerializeField] private GameObject[] sushiOrder; //theOrder of the customer
    [SerializeField] private Transform customerOrderSpawnLocation; //Spawning of the order
    [SerializeField] private float customerDeleteTimer;

    [Header("Patience Rating")]
    [SerializeField] private float currentCustomerPaitenceTimer; //use this to take the score for the customer rating
    [SerializeField] private float maxCustomerPaitenceTimer;

    [Header("Satisfaction Rating")]
    [SerializeField] public float customerSatisfactionRating; //value that we'll use to check the dish decay value against the food
    [SerializeField] private float maxCustomerSR; //SR = SatisfactionRating
    [SerializeField] private float minCustomerSR;



    private void Awake()
    { 
        dishType = (OrderType)UnityEngine.Random.Range(0, System.Enum.GetValues(typeof(OrderType)).Length);
        //set a random dishType
        customerSatisfactionRating = UnityEngine.Random.Range(minCustomerSR, maxCustomerSR);
        // at start, each customer will have a random value for their taste value
    }

    // Start is called before the first frame update
    void Start()
    {
        SpawnCustomerOrder();
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(dishType.ToString());
        //For Debuggin
    }

    private GameObject SetCustomerOrder()
    {
        switch (dishType)
        {
            case OrderType.Nigiri_Salmon:
                return sushiOrder[0];

            /*
           case DishType.Nigiri_Tuna: 
               return sushiOrder[1];

           case DishType.Maki_Salmon: 
               return sushiOrder[2];

           case DishType.Maki_Tuna: 
               return sushiOrder[3];
            */
            default:
                return null;
        }
    }

    public void SpawnCustomerOrder()
    {
        GameObject customerOrder = Instantiate(SetCustomerOrder(),
                                               customerOrderSpawnLocation.position,
                                               customerOrderSpawnLocation.rotation);

        //customerOrder.GetComponent<SushiDishUI>().MaxTime = maxCustomerPaitenceTimer; //setting the timer

    }

    private void OnCollisionEnter(Collision collision)
    {
        if(CheckDishServed(collision.gameObject))
        {
          StartCoroutine(CustomerDeleteTimer());
        }
    }

   bool CheckDishServed(GameObject dishServedToCustomer)
    { 
        if(dishServedToCustomer == null)
        {
            return false;
        }

        Dish dishServed = dishServedToCustomer.GetComponent<Dish>(); //To gets the enum of the sushi dish

        if(dishServed.DishType.Equals(dishType)) //check if the Enum of the dish matches to customer's Enum
        {
            return true;
        }
        else
        {
            Debug.Log("Wrong Order");
            return false;
        }
    }

    IEnumerator CustomerDeleteTimer()
    {
        yield return new WaitForSeconds(customerDeleteTimer);

        MakeSeatEmpty();
    }

    private void MakeSeatEmpty() //Use this if customer is happy with the dish or he runs out of patience
    {
        CustomerSpawningManager.Instance.RemoveCustomer(this.gameObject);

        CustomerSpawningManager.Instance.GetComponent<SpawnLocationScript>()._isPrefabPresent = false;

        CustomerSpawningManager.Instance.StartCoroutine("ITimerForNextCustomerSpawn");

        Destroy(this.gameObject);
    }

    private void CustomerPaitenceZero()
    { 
        //insert code to give bad review etc.
        MakeSeatEmpty();
    }


}
