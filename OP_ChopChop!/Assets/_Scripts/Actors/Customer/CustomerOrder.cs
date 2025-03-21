using System.Collections;
using UnityEngine;

/// <summary> -WHAT DOES THIS SCRIPT DO-
/// 
/// The core component for the customer 
///  
/// </summary>

[RequireComponent(typeof(CustomerAppearance), typeof(CustomerActions))]
public class CustomerOrder : MonoBehaviour
{

#region Readers

    public DishType CustomerDishType { get; private set; } // what dish the cust omer wants to order   
    public float CustomerSR { get; set; }                  // (FoodScore of dish + _patienceRate) / 2
    public float PatienceRate => _patienceRate;

#endregion

#region Members

    [Header("Dish UI")]
    [SerializeField] private GameObject[] _dishOrdersUI;  // the different order UI for the customer 
    [SerializeField] private Transform _orderUITransform; // Spawning of the order

    [Header("Customer Components")]
    [SerializeField] CustomerActions _actions;
    [SerializeField] CustomerAppearance _appearance;

    // TIMERS
    float _customerChewingTimer; // time it takes for the customer to eat and leave
    float _patienceRate;         // deduction rate to use
    float _customerScore;        // starts at 100 ang decreases over time
    
    [SerializeField] int _minCash, _maxCash; // testing

#endregion

    void Start()
    {
        CustomerDishType = DishType.NIGIRI_SALMON;

        _customerScore = 100f; // will decrease overtime
        _patienceRate = 1.65f; // referenced for the document
        _customerChewingTimer = 4f;
        
        CreateCustomerUI();
        StartCoroutine(PatienceCountdown());
    }

#region Spawning_Helpers

    void CreateCustomerUI() // find a fix so that SpawnMan does the spawning instead
    {
        Instantiate(_dishOrdersUI[Random.Range(0, _dishOrdersUI.Length)],
                    _orderUITransform.position,
                    _orderUITransform.rotation);
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

    /* -OUTDATED, BUT IS STILL HERE IN CASE THE NEW ONE DOESN'T WANNA WORK-    
    IEnumerator DoPositiveReaction() // customer got the correct dish
    {
        _appearance.ChangeEmotion(FaceVariant.HAPPY);
        yield return new WaitForSeconds(1f);

        _actions.TriggerEating();
        StartCoroutine(_appearance.DoChweing(_patienceRate));
        yield return new WaitForSeconds(_customerChewingTimer);

        MakeSeatEmpty();
    }
    IEnumerator DoNegativeReaction() // customer lost all patience or got the wrong order
    {       
        _appearance.ChangeEmotion(FaceVariant.HAPPY);
        yield return new WaitForSeconds(1f);

        _actions.TriggerEating();

        _customerScore = 0f;
        yield return new WaitForSeconds(_customerChewingTimer);

        MakeSeatEmpty();
    }
    */

    public IEnumerator DoReaction(FaceVariant emotionType)
    {
        // initlal reaction
        _appearance.ChangeEmotion(emotionType);
        yield return new WaitForSeconds(0.5f);

        if (emotionType == FaceVariant.MAD)
            _customerScore = 0f;
        
        // chewing + "chewing animation"
        _actions.TriggerEating();
        _appearance.DoChweing(_patienceRate);
        yield return new WaitForSeconds(_customerChewingTimer);

        // final reaction
        _appearance.ChangeEmotion(emotionType); 
        yield return new WaitForSeconds(0.5f);

        // payment/refund
        if (_patienceRate > 50f)
            GameManager.Instance.AddMoney(Random.Range(_minCash, _maxCash));
        
        else 
            GameManager.Instance.DeductMoney(Random.Range(_minCash, _maxCash));
        
        GameManager.Instance.IncrementCustomersServed();
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
        
        // yield return StartCoroutine(DoNegativeReaction());
        yield return StartCoroutine(DoReaction(FaceVariant.MAD));
    }

#endregion
}
