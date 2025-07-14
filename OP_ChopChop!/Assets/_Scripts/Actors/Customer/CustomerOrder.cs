using System.Collections;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(CustomerAppearance), typeof(CustomerActions))]
public class CustomerOrder : MonoBehaviour
{
#region Readers

    public DishPlatter WantedPlatter { get; private set; } // what dish the customer wants to order   
    public float CustomerSR { get; set; }                  // (FoodScore of dish + _patienceRate) / 2
    public float PatienceRate => _patienceDecreaseRate;
    public bool IsLastCustomer { get; set; } = false;

    public bool IsTutorial => _isTutorial; 
    public bool IsTunaCustomer => _isTunaCustomer;

#endregion

#region Members

    [Header("Dish UI")]
    [SerializeField] private GameObject[] _dishOrdersUI;  // the different order UI for the customer 
    [SerializeField] private Transform _orderUITransform; // Spawning of the order
    
    [Header("Money-earned Range")]
    [SerializeField] private int _minCash;
    [SerializeField] private int _maxCash; // testing

    [Header("Money Reward Text"), SerializeField] private TextMeshProUGUI _txtMoneyReward;
    [SerializeField] private float _patienceDecreaseRate; // 1.65; deduction rate to use

    [Space(10f), SerializeField] private float _reactionTimer;

    [Header("Onboarding")] 
    [SerializeField] private bool _isTutorial;
    [SerializeField] private bool _isTunaCustomer;
   
    private CustomerAppearance _appearance;
    private CustomerActions _actions;
    private GameObject _customerOrderUI;
    private float _customerScore; // starts at 100 and decreases over time

#endregion

    private void Start()
    {
        if (!_isTutorial)
            GameManager.Instance.OnEndService += DestroyOrderUI;
        
        _appearance = GetComponent<CustomerAppearance>();
        _actions = GetComponent<CustomerActions>();

        switch (GameManager.Instance.Difficulty) // will decrease overtime
        {
            case GameDifficulty.EASY: _customerScore = 110f; break;
            case GameDifficulty.NORMAL: _customerScore = 100f; break;
            case GameDifficulty.HARD: _customerScore = 90f; break;
            default: break;
        }

        if (_isTutorial)
        {
            _patienceDecreaseRate = 0f;
            WantedPlatter = _isTunaCustomer ? 
                               DishPlatter.SASHIMI_TUNA : 
                               DishPlatter.NIGIRI_SALMON;

            OnBoardingHandler.Instance.OnTutorialEnd += Cleanup;
        }
        else 
        {
            _patienceDecreaseRate = 1.65f; // referenced from the document
            WantedPlatter = (DishPlatter)Random.Range(0, _dishOrdersUI.Length);
        }

        CreateCustomerUI();
        StartCoroutine(CO_PatienceCountdown());
    }
    private void OnDestroy()
    {
        if (_isTutorial)
        {
            OnBoardingHandler.Instance.OnTutorialEnd -= Cleanup;
            Destroy(_customerOrderUI);
        }
        else GameManager.Instance.OnEndService -= DestroyOrderUI; 

        if (_isTunaCustomer)
        {
            SpawnManager.Instance.DisableTutorial();
            ShopManager.Instance.ClearList();
            return;
        }

        if (!IsLastCustomer) return;

        if (GameManager.Instance.CurrentShift == GameShift.Service)
        {
            GameManager.Instance.StopAllCoroutines();
            GameManager.Instance.ChangeShift(GameShift.PostService);
        }
    }

    private void Cleanup() => Destroy(gameObject);

#region Spawning_Helpers

    private void CreateCustomerUI()
    {
        // aligns customer UI & customer order
        _customerOrderUI = Instantiate(_dishOrdersUI[_isTutorial ? 0 : (int)WantedPlatter], 
                                       _orderUITransform.position,
                                       _orderUITransform.rotation,
                                       transform);
    }
    private void MakeSeatEmpty() // clears the seat of any customer references 
    {
        // de-links the customer order & the seat
        SpawnManager.Instance.RemoveCustomer(gameObject);

        if (!_isTutorial)
        {
            SpawnManager.Instance.StartCustomerSpawning();
            GameManager.Instance.AddToCustomerScores(CustomerSR);
        }

        // destroys both the customer and its UI
        DestroyOrderUI();
        Destroy(gameObject);
        Debug.LogWarning($"Destroyed {this}");
    }
    private void DestroyOrderUI() => Destroy(_customerOrderUI);

#endregion

#region Enumerators

    private IEnumerator CO_PatienceCountdown()
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
        yield return StartCoroutine(CO_CustomerLostPatience());
    }
    private IEnumerator CO_CustomerLostPatience() // customer wasn't served
    {
        _appearance.SetAngryEmotion(2);
        SoundManager.Instance.PlaySound("cat angry");
        yield return new WaitForSeconds(_reactionTimer);

        MakeSeatEmpty();
    }
    public IEnumerator CO_HappyReaction() // customer got the correct order
    {
        // inital reaction
        _appearance.SetFacialEmotion(FaceVariant.HAPPY);
        _actions.TriggerEating();
        
        StartCoroutine(_appearance.DoChweing(_customerScore));
        SoundManager.Instance.PlaySound("cat happy");
        yield return new WaitForSeconds(_reactionTimer);

        // final actions
        GameManager.Instance.IncrementCustomersServed();
        GameManager.Instance.AddMoney(Random.Range(_minCash, _maxCash));
        MakeSeatEmpty();
    }
    public IEnumerator CO_AngryReaction() // customer got the wrong order
    {
        // initial reaction
        _appearance.SetAngryEmotion(1);
        _actions.TriggerEating();

        _customerScore = 0f;
        SoundManager.Instance.PlaySound("cat angry");
        yield return new WaitForSeconds(_reactionTimer);

        // final actions
        GameManager.Instance.IncrementCustomersServed();
        MakeSeatEmpty();
    }
    public IEnumerator CO_DirtyReaction() // customer got an expired order
    {
        // inital reaction
        _appearance.SetFacialEmotion(FaceVariant.SUS);
        _customerScore = 0f;
        SoundManager.Instance.PlaySound("cat yuck");
        yield return new WaitForSeconds(_reactionTimer);

        // final actions
        GameManager.Instance.IncrementCustomersServed();
        StartCoroutine(GameManager.Instance.CO_CloseDownShop());
    }       

#endregion
}