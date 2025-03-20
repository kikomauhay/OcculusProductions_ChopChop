using System.Collections;
using UnityEngine;
using System;

public class CustomerOrder : MonoBehaviour
{
    #region Members

    public Action OnBadOrder, OnGoodOrder;

    public DishType CustomerDishType { get; private set; } // what dish the customer wants to order   
    public float CustomerSR { get; set; }                  // (FoodScore of dish + _patienceRate) / 2
    public float PatienceRate => _patienceRate;
    
    // timers
    private float _customerDeleteTimer; // time it takes for the customer to eat and leave
    private float _patienceRate;        // deduction rate to use
    private float _customerScore;       // starts at 100 ang decreases over time

    [Header("Dish UI")]
    [SerializeField] private GameObject[] _dishOrdersUI;  // the different order UI for the customer 
    [SerializeField] private Transform _orderUITransform; // Spawning of the order

#endregion

    void Start()
    {
        CustomerDishType = DishType.NIGIRI_SALMON;

        _customerScore = 100f; // will decrease overtime
        _patienceRate = 1.65f; 
        _customerDeleteTimer = 3f;
        
        CreateCustomerUI();
        StartCoroutine(PatienceCountdown());
    }

#region Methods

    void CreateCustomerUI()
    {
        Instantiate(_dishOrdersUI[0],
                    _orderUITransform.position,
                    _orderUITransform.rotation,
                    transform);
    }
    void MakeSeatEmpty() // clears the seat of any customer references 
    {
        SpawnManager.Instance.RemoveCustomer(gameObject);
        SpawnManager.Instance.StartCoroutine("HandleCustomer");
        GameManager.Instance.AddToCustomerScores(CustomerSR);

        Destroy(gameObject);
    }
    public bool OrderIsSameAs(Dish dish) => dish?.OrderDishType == CustomerDishType;
    
#endregion

#region Enumerators

    IEnumerator DoPositiveReaction() // customer got the correct dish
    {
        OnGoodOrder?.Invoke();
        yield return new WaitForSeconds(_customerDeleteTimer);

        MakeSeatEmpty();
    }
    IEnumerator DoNegativeReaction() // customer lost all patience or got the wrong order
    {       
        _customerScore = 0f;
        OnBadOrder?.Invoke();

        yield return new WaitForSeconds(_customerDeleteTimer);


        MakeSeatEmpty();
    }
    IEnumerator PatienceCountdown()
    {
        yield return new WaitForSeconds(3f); // time it takes for the customer to take a seat

        while (_customerScore > 0f)
        {
            yield return new WaitForSeconds(1f);

            _customerScore -= _patienceRate;

            if (_customerScore < 1f)
                _customerScore = 0f;
        }
        
        // custoemr lost all patience
        _customerScore = 0f;   

        yield return StartCoroutine(DoNegativeReaction());
    }

#endregion
}
