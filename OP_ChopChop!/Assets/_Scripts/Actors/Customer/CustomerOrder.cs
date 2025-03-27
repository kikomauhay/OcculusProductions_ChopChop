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
    public float PatienceRate => _patienceDecreaseRate;

#endregion

#region Members

    [Header("Dish UI")]
    [SerializeField] private GameObject[] _dishOrdersUI;  // the different order UI for the customer 
    [SerializeField] private Transform _orderUITransform; // Spawning of the order
    
    [SerializeField] int _minCash, _maxCash; // testing

    [Header("Customer Components")]
    [SerializeField] CustomerActions _actions;
    [SerializeField] CustomerAppearance _appearance;
    GameObject _customerOrderUI;

    // TIMERS
    [SerializeField] float _patienceDecreaseRate; // 1.65; deduction rate to use
    [SerializeField] float _customerScore;        // 100; starts at 100 and decreases over time

#endregion

    void Start()
    {
        CustomerDishType = DishType.NIGIRI_SALMON;

        _customerScore = 100f;         // will decrease overtime
        _patienceDecreaseRate = 1.65f; // referenced from the document
        
        CreateCustomerUI();
        StartCoroutine(PatienceCountdown());
    }

#region Spawning_Helpers

    void CreateCustomerUI() // find a fix so that SpawnMan does the spawning instead
    {
        _customerOrderUI = Instantiate(_dishOrdersUI[Random.Range(0, _dishOrdersUI.Length)],
                                       _orderUITransform.position,
                                       _orderUITransform.rotation);
    }
    void MakeSeatEmpty() // clears the seat of any customer references 
    {
        SpawnManager.Instance.RemoveCustomer(gameObject);        
        SpawnManager.Instance.StartCustomerSpawning();
        GameManager.Instance.AddToCustomerScores(CustomerSR);

        // destroys both the customer and its UI
        Destroy(_customerOrderUI);
        Destroy(gameObject);
    }
    public bool OrderIsSameAs(Dish dish) => dish?.OrderDishType == CustomerDishType;
    
#endregion

#region Enumerators

    IEnumerator PatienceCountdown()
    {
        // grace period before it actually starts counting down
        yield return new WaitForSeconds(1.5f); 

        while (_customerScore > 0f)
        {
            yield return new WaitForSeconds(1f);

            _customerScore -= _patienceDecreaseRate;

            if (_customerScore < 1f)
            {
                _customerScore = 0f;
                CustomerSR = 0f;
            }
            
            // face change
            if (_customerScore >= 80)
                _appearance.SetFacialEmotion(FaceVariant.HAPPY);

            else if (_customerScore >= 50)
                _appearance.SetFacialEmotion(FaceVariant.NEUTRAL);
            
            else     
                _appearance.SetAngryEmotion(0);            
        }
        
        // customer lost all patience
        yield return StartCoroutine(CustomerLostPatience());
    }
    IEnumerator CustomerLostPatience() // customer wasn't served
    {
        _appearance.SetAngryEmotion(2);
        yield return new WaitForSeconds(2f);

        MakeSeatEmpty();
    }
    public IEnumerator HappyReaction() // customer got the correct order
    {
        // initial reaction
        _appearance.SetFacialEmotion(FaceVariant.HAPPY);
        yield return new WaitForSeconds(0.5f);

        // chewing + animations
        _actions.TriggerEating();
        StartCoroutine(_appearance.DoChweing(_customerScore));

        // final actions
        GameManager.Instance.IncrementCustomersServed();
        GameManager.Instance.AddMoney(Random.Range(_minCash, _maxCash));
        MakeSeatEmpty();
    }
    public IEnumerator AngryReaction() // customer got the wrong order
    {
        // initial reaction
        _appearance.SetAngryEmotion(1);
        _customerScore = 0f;
        yield return new WaitForSeconds(0.5f);

        /*
        // chewing + animations
        _actions.TriggerEating();
        StartCoroutine(_appearance.DoChweing(_customerScore)); 
        */

        // final actions
        Debug.LogWarning("THIS IS NOT MY ORDER!");
        GameManager.Instance.IncrementCustomersServed();
        MakeSeatEmpty();
    }
    public IEnumerator ExpiredReaction() // customer got an expired order
    {
        // initial reaction
        _appearance.SetFacialEmotion(FaceVariant.SUS);
        _customerScore = 0f;
        yield return new WaitForSeconds(0.5f);

        /*
        // chewing + animation
        _actions.TriggerEating();
        StartCoroutine(_appearance.DoChweing(_customerScore));
        */

        // final actions
        Debug.LogWarning("WHAT IS THIS FOOD??");
        GameManager.Instance.IncrementCustomersServed();
        MakeSeatEmpty();
    }       

#endregion
}
