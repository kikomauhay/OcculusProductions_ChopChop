using System.Collections;
using UnityEngine;

public class CustomerOrder : MonoBehaviour
{
#region Members

    public DishType CustomerDishType { get; private set; } // what dish the customer wants to order   
    public float CustomerSR { get; set; } // (FoodScore of dish + _patienceRate) / 2   \
    public float PatienceRate => _patienceRate;
    
    // timers
    private float _customerDeleteTimer; // time it takes for the customer to eat and leave
    private float _patienceRate; // deduction rate to use
    private float _customerScore; // starts at 100 ang decreases over time

    [Header("Dish UI")]
    [SerializeField] private GameObject[] _dishOrdersUI; // the different order UI for the customer 
    [SerializeField] private Transform _orderUITransform; //Spawning of the order

#endregion

    private void Start()
    {         
        // randomizes the customer's order
        CustomerDishType = (DishType)Random.Range(0, System.Enum.GetValues(typeof(DishType)).Length);
        
        _customerScore = 100f; // will decrease overtime
        _patienceRate = 1.65f;
        _customerDeleteTimer = 3f;
        
        CreateCustomerUI();
        StartCoroutine(PatienceCountdown());
    }

#region Methods

    public bool CheckDishServed(GameObject dishServedToCustomer)
    { 
        Dish dishServed = dishServedToCustomer.GetComponent<Dish>(); // gets the enum of the dish

        if (dishServedToCustomer == null) return false;
        
        return dishServed.OrderDishType.Equals(CustomerDishType);
    }   
    private void CreateCustomerUI()
    {
        Instantiate(_dishOrdersUI[(int)CustomerDishType], // i love type-casting
                    _orderUITransform.position,
                    _orderUITransform.rotation);
    }     
    private void MakeSeatEmpty() // clears the seat of any customer references 
    {
        CustomerSpawningManager.Instance.RemoveCustomer(gameObject);
        CustomerSpawningManager.Instance.GetComponent<SpawnLocationScript>().IsPrefabPresent = false;
        CustomerSpawningManager.Instance.StartCoroutine("SpawnNextCustomer");

        // adds the customer's score to the Scores list
        GameManager.Instance.OnCustomerServed?.Invoke(CustomerSR);
        Destroy(gameObject);
    }

#endregion

#region Enumerators

    IEnumerator DoNegativeReaction() // customer looses all patience
    {
        Debug.LogWarning("Customer has left the restaurant leaving a bad review!");
        Debug.Log($"Customer score: {_customerScore}");
        
        yield return new WaitForSeconds(_customerDeleteTimer);
        
        CustomerSR = 0;

        GameManager.Instance.OnCustomerLeft?.Invoke();
        MakeSeatEmpty();
    }
    IEnumerator DoPositiveReaction() 
    {
        Debug.Log("Customer is chowing down on the food!");

        // customer eats the food before despawning

        yield return new WaitForSeconds(_customerDeleteTimer);

        MakeSeatEmpty();
    }
    IEnumerator PatienceCountdown()
    {
        Debug.Log("Waiting 3 seconds before counting down");
        yield return new WaitForSeconds(3f); // time it takes for the customer to take a seat

        while (_customerScore > 0f)
        {
            yield return new WaitForSeconds(1f);

            _customerScore -= _patienceRate;
            Debug.Log($"Customer score of {name} is now {_customerScore}");

            if (_customerScore < 1f)
                _customerScore = 0f;
        }

        // customer lost all patience
        _customerScore = 0f;   
        yield return StartCoroutine(DoNegativeReaction());
    }

#endregion
}
