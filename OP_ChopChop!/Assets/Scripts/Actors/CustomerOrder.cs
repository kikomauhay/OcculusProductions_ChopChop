using System.Collections;
using UnityEngine;
using System;

public class CustomerOrder : MonoBehaviour
{
#region Members

    [SerializeField] private DishType _dishType; // the dish that the customer wants
    
    [SerializeField] private GameObject[] _dishOrderUI; // the UI order of the customer
    [SerializeField] private Transform _customerTransform; //Spawning of the order
    [SerializeField] private float customerDeleteTimer;

    [Header("Patience Rating")]
    [SerializeField] private float currentCustomerPaitenceTimer; //use this to take the score for the customer rating
    [SerializeField] private float maxCustomerPaitenceTimer;

    [Header("Satisfaction Rating")]
    [SerializeField] public float _satisfactionRating; //value that we'll use to check the dish decay value against the food
    [SerializeField] private float maxCustomerSR; // SR = SatisfactionRating
    [SerializeField] private float minCustomerSR;
    

    private int _dishTypeLengh; // how many elements inside the DishType Enum

#endregion


    private void Start()
    { 
        _dishTypeLengh = Enum.GetValues(typeof(DishType)).Length;

        // set a random dishType
        _dishType = (DishType)UnityEngine.Random.Range(0, _dishTypeLengh);

        // at start, each customer will have a random value for their taste value
        _satisfactionRating = UnityEngine.Random.Range(minCustomerSR, maxCustomerSR);


        
        SpawnCustomerOrder();
    }

    void Update()
    {
        //Debug.Log(dishType.ToString());
        //For Debuggin
    }

    private GameObject SetCustomerOrderUI()
    {
        switch (_dishType)
        {
            case DishType.NIGIRI_SALMON:
                return _dishOrderUI[0];

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
        GameObject customerOrder = Instantiate(SetCustomerOrderUI(),
                                               _customerTransform.position,
                                               _customerTransform.rotation);

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
        if(dishServedToCustomer == null) return false;        

        Dish dishServed = dishServedToCustomer.GetComponent<Dish>(); //To gets the enum of the sushi dish

        if(dishServed.DishType.Equals(_dishType)) //check if the Enum of the dish matches to customer's Enum
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


    IEnumerator StartPatienceTimer() {

        while (true)
        {
            yield return new WaitForSeconds(1f);
            
        }

        
    }

}
