using System.Collections;
using UnityEngine;

public class CustomerOrder : MonoBehaviour
{
#region Members

    public DishType CustomerDishType { get; private set; } // what dish the customer wants to order   
    public float CustomerSR { get; set; }                  // (FoodScore of dish + _patienceRate) / 2
    public float PatienceRate => _patienceRate;
    
    // timers
    private float _customerDeleteTimer; // time it takes for the customer to eat and leave
    private float _patienceRate;        // deduction rate to use
    private float _customerScore;       // starts at 100 ang decreases over time

    [Header("Dish UI")]
    [SerializeField] private GameObject[] _dishOrdersUI; // the different order UI for the customer 
    [SerializeField] private Transform _orderUITransform; //Spawning of the order

#endregion

#region Methods

    void Start()
    {
        // randomizes the customer's order
        // CustomerDishType = (DishType)Random.Range(0, System.Enum.GetValues(typeof(DishType)).Length);

        CustomerDishType = DishType.NIGIRI_SALMON; // test

        _customerScore = 100f; // will decrease overtime
        _patienceRate = 1.65f; 
        _customerDeleteTimer = 3f;
        
        // CreateCustomerUI();
        StartCoroutine(PatienceCountdown());

        Debug.LogWarning($"{name}: {CustomerDishType}");
    }

    void CreateCustomerUI()
    {
        Instantiate(_dishOrdersUI[(int)CustomerDishType], // i love type-casting
                    _orderUITransform.position,
                    _orderUITransform.rotation);
    }

    // will refine with AJ's help     
    void MakeSeatEmpty() // clears the seat of any customer references 
    {
        CustomerHandler.Instance.RemoveCustomer(gameObject);
        CustomerHandler.Instance.GetComponent<CustomerSeat>().IsEmpty = false;
        CustomerHandler.Instance.StartCoroutine("SpawnNextCustomer");

        // adds the customer's score to the Scores list
        GameManager.Instance.OnCustomerServed?.Invoke(CustomerSR);
        Destroy(gameObject);
    }
    public bool OrderIsSameAs(Dish dish) => dish?.OrderDishType == CustomerDishType;
    
#endregion

#region Enumerators

    IEnumerator DoPositiveReaction() // customer got the correct dish
    {
        // customer eats the food before despawning
        // can add animation of the customer eating & sfx

        yield return new WaitForSeconds(_customerDeleteTimer);

        MakeSeatEmpty();
    }
    IEnumerator DoNegativeReaction() // customer lost all patience or got the wrong order
    {       
        _customerScore = 0f;

        yield return new WaitForSeconds(_customerDeleteTimer);

        GameManager.Instance.OnCustomerLeft?.Invoke();
        MakeSeatEmpty();
    }
    IEnumerator PatienceCountdown()
    {
        yield return new WaitForSeconds(3f); // time it takes for the customer to take a seat

        while (_customerScore > 0f)
        {
            yield return new WaitForSeconds(1f);

            _customerScore -= _patienceRate;
            // Debug.Log($"Customer score of {name} is now {_customerScore}");

            if (_customerScore < 1f)
                _customerScore = 0f;
        }

        // customer lost all patience
        _customerScore = 0f;   
        Debug.Log(_customerScore);

        yield return StartCoroutine(DoNegativeReaction());
    }

#endregion
}
