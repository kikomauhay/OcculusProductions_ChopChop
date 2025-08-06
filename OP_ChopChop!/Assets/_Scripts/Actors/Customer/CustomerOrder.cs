using System.Collections;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(CustomerAppearance), typeof(CustomerActions))]
public class CustomerOrder : MonoBehaviour
{
    #region Properties

    public DishPlatter WantedPlatter { get; private set; } // what dish the customer wants to order   
    public float PatienceScore { get; set; }                  // (FoodScore of dish * 0.8 + _patienceRate * 0.2) / 2
    public bool IsLastCustomer { get; set; } = false;
    public float PatienceRate => _patienceDecreaseRate;
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

    #endregion
    #region Private

    private CustomerAppearance _appearance;
    private CustomerActions _actions;
    private GameObject _customerOrderUI;
    private float _customerScore; // starts at 100 and decreases over time
    private const float REACTION_TIME = 1f;

    #endregion

    #region Unity

    private void Start()
    {
        _appearance = GetComponent<CustomerAppearance>();
        _actions = GetComponent<CustomerActions>();

        InitializeEvents();
        InitializeWantedPlatter();
        InitializeCustomerScore();

        CreateCustomerUI();
        StartCoroutine(CO_PatienceCountdown());
    }
    private void OnDestroy()
    {
        DestroyCustomerUI();
        DeinitializeEvents();

        SpawnManager.Instance.RemoveCustomer(gameObject);
        GameManager.Instance.CheckRemainingCustomers();

        if (_isTunaCustomer) // only for onboarding
        {
            SpawnManager.Instance.DisableTutorial();
            ShopManager.Instance.ClearList();
            return;
        }

        /*

        if (GameManager.Instance.CurrentShift == GameShift.Service && IsLastCustomer)
        {
            GameManager.Instance.StopAllCoroutines();
            GameManager.Instance.ChangeShift(GameShift.PostService);
        } */
    }

    #endregion
    #region Helpers

    private void InitializeCustomerScore()
    {
        switch (GameManager.Instance.Difficulty) // will decrease overtime
        {
            // updated starting score to have an emphasis on food quality
            case GameDifficulty.EASY: _customerScore = 250f; break;
            case GameDifficulty.NORMAL: _customerScore = 225f; break;
            case GameDifficulty.HARD: _customerScore = 200f; break;
            default: break;
        }
    }
    private void InitializeWantedPlatter()
    {
        if (!_isTutorial)
        {
            WantedPlatter = (DishPlatter)Random.Range(0, _dishOrdersUI.Length);
            return;
        }

        WantedPlatter = _isTunaCustomer ? DishPlatter.SASHIMI_TUNA :
                                          DishPlatter.NIGIRI_SALMON;
    }
    private void CreateCustomerUI() // aligns Order UI & Customer Order
    {
        _customerOrderUI = Instantiate(_dishOrdersUI[_isTutorial ? 0 : (int)WantedPlatter],
                                       _orderUITransform.position,
                                       _orderUITransform.rotation,
                                       transform);
    }
    private void MakeSeatEmpty() // clears the seat of any customer references 
    {
        DestoryGO();

        if (!_isTutorial)
            SpawnManager.Instance.StartCustomerSpawning();
    }
    private void InitializeEvents()
    {
        if (_isTutorial)
            OnBoardingHandler.Instance.OnTutorialEnd += DestoryGO;

        else
            GameManager.Instance.OnEndService += DestoryGO;
    }
    private void DeinitializeEvents()
    {
        if (_isTutorial)
            OnBoardingHandler.Instance.OnTutorialEnd -= DestoryGO;

        else        
            GameManager.Instance.OnEndService -= DestoryGO;
    }
    private void DestroyCustomerUI() => Destroy(_customerOrderUI);
    private void DestoryGO() => Destroy(gameObject);

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
                PatienceScore = 0f;
            }

            // face change
            if (_customerScore >= 80)
                _appearance.SetFacialEmotion(FaceVariant.HAPPY);

            else if (_customerScore >= 50)
                _appearance.SetFacialEmotion(FaceVariant.NEUTRAL);

            else
                _appearance.SetAngryEmotion(0);
        }

        yield return StartCoroutine(CO_CustomerLostPatience());
    }
    private IEnumerator CO_CustomerLostPatience() // customer wasn't served
    {
        _appearance.SetAngryEmotion(2);
        _customerScore = 0f;
        SoundManager.Instance.PlaySound("cat angry");
        // yield return new WaitForSeconds(_reactionTimer);
        yield return new WaitForSeconds(REACTION_TIME);

        GameManager.Instance.IncrementCustomersServed();
        MakeSeatEmpty();
    }
    public IEnumerator CO_HappyReaction() // customer got the correct order
    {
        _appearance.SetFacialEmotion(FaceVariant.HAPPY);
        _actions.TriggerEating();
        SoundManager.Instance.PlaySound("cat happy");
        // yield return new WaitForSeconds(_reactionTimer);
        yield return new WaitForSeconds(REACTION_TIME);

        GameManager.Instance.IncrementCustomersServed();
        GameManager.Instance.AddMoney(Random.Range(_minCash, _maxCash));
        ShopManager.Instance.UpatePlayerMoneyUI();

        MakeSeatEmpty();
    }
    public IEnumerator CO_AngryReaction() // customer got the wrong order
    {
        _appearance.SetAngryEmotion(1);
        _customerScore = 0f;
        SoundManager.Instance.PlaySound("cat angry");
        // yield return new WaitForSeconds(_reactionTimer);
        yield return new WaitForSeconds(REACTION_TIME);

        // final actions
        GameManager.Instance.IncrementCustomersServed();
        MakeSeatEmpty();
    }
    public IEnumerator CO_DirtyReaction() // customer got an expired order
    {
        _appearance.SetFacialEmotion(FaceVariant.SUS);
        _customerScore = 0f;
        SoundManager.Instance.PlaySound("cat yuck");
        // yield return new WaitForSeconds(_reactionTimer);
        yield return new WaitForSeconds(REACTION_TIME);

        GameManager.Instance.IncrementCustomersServed();
        StartCoroutine(GameManager.Instance.CO_GameOver1());
    }

    #endregion
}