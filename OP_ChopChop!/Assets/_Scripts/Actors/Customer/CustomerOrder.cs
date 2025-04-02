using System.Collections;
using UnityEngine;
using TMPro;

/// <summary> -WHAT DOES THIS SCRIPT DO-
/// 
/// The core component for the customer 
///  
/// </summary>

[RequireComponent(typeof(CustomerAppearance), typeof(CustomerActions))]
public class CustomerOrder : MonoBehaviour
{
#region Readers

    public DishType CustomerDishType { get; private set; } // what dish the customer wants to order   
    public float CustomerSR { get; set; }                  // (FoodScore of dish + _patienceRate) / 2
    public float PatienceRate => _patienceDecreaseRate;
    public bool IsLastCustomer { get; set; } = false;

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

    [Header("MoneyRewardTxt")]
    [SerializeField] private TextMeshProUGUI _txtMoneyReward;

    // TIMERS
    [SerializeField] float _patienceDecreaseRate; // 1.65; deduction rate to use
    [SerializeField] float _customerScore;        // 100; starts at 100 and decreases over time

    // REACTION FACES
    [SerializeField] float _reactionTimer;

    [SerializeField] bool _isTutorial;

#endregion

    void Start()
    {
        GameManager.Instance.OnEndService += DestroyOrderUI;

        CustomerDishType = _isTutorial ? DishType.NIGIRI_SALMON : (DishType)Random.Range(0, 4);
        
        switch (GameManager.Instance.Difficulty) // will decrease overtime
        {
            case GameDifficulty.EASY:   _customerScore = 110f; break;
            case GameDifficulty.NORMAL: _customerScore = 100f; break;
            case GameDifficulty.HARD:   _customerScore = 90f;  break;

            default: break;
        }

        if (_patienceDecreaseRate == 0f)
            _patienceDecreaseRate = 1.65f; // referenced from the document
        
        CreateCustomerUI();
        StartCoroutine(PatienceCountdown());
    }
    void OnDestroy() 
    {
        GameManager.Instance.OnEndService -= DestroyOrderUI;

        if (!IsLastCustomer) return;

        if (GameManager.Instance.CurrentShift == GameShift.SERVICE)
        {
            GameManager.Instance.StopAllCoroutines();
            GameManager.Instance.ChangeShift(GameShift.POST_SERVICE);
        }
        else if (GameManager.Instance.CurrentShift == GameShift.TRAINING)
        {
            StartCoroutine(OnBoardingHandler.Instance.EndOfDayTutorial());
        }

    }
#region Spawning_Helpers

    void CreateCustomerUI()
    {
        _customerOrderUI = Instantiate(_dishOrdersUI[(int)CustomerDishType], // aligns customer UI & customer order
                                       _orderUITransform.position,
                                       _orderUITransform.rotation);
    }
    void MakeSeatEmpty() // clears the seat of any customer references 
    {
        SpawnManager.Instance.RemoveCustomer(gameObject);        
        SpawnManager.Instance.StartCustomerSpawning();
        GameManager.Instance.AddToCustomerScores(CustomerSR);

        // destroys both the customer and its UI
        DestroyOrderUI();   
        Destroy(gameObject);
    }
    public bool OrderIsSameAs(Dish dish) => dish?.OrderDishType == CustomerDishType;
    void DestroyOrderUI() => Destroy(_customerOrderUI);

#endregion

#region Enumerators

    IEnumerator PatienceCountdown()
    {
        // grace period before it actually starts counting down
        yield return new WaitForSeconds(2f); 

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
        SoundManager.Instance.PlaySound("cat angry", SoundGroup.CUSTOMER);
        yield return new WaitForSeconds(_reactionTimer);

        MakeSeatEmpty();
    }
    public IEnumerator HappyReaction() // customer got the correct order
    {
        // inital reaction
        _appearance.SetFacialEmotion(FaceVariant.HAPPY);
        StartCoroutine(_appearance.DoChweing(_customerScore));
        SoundManager.Instance.PlaySound("cat happy", SoundGroup.CUSTOMER);
        yield return new WaitForSeconds(_reactionTimer);

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
        SoundManager.Instance.PlaySound("cat angry", SoundGroup.CUSTOMER);
        yield return new WaitForSeconds(_reactionTimer);

        // final actions
        GameManager.Instance.IncrementCustomersServed();
        MakeSeatEmpty();
    }
    public IEnumerator ExpiredReaction() // customer got an expired order
    {
        // inital reaction
        _appearance.SetFacialEmotion(FaceVariant.SUS);
        _customerScore = 0f;
        SoundManager.Instance.PlaySound("cat yuck", SoundGroup.CUSTOMER);
        yield return new WaitForSeconds(_reactionTimer);

        // final actions
        GameManager.Instance.IncrementCustomersServed();
        StartCoroutine(GameManager.Instance.CloseDownShop());
    }       

#endregion
}
