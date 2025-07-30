using System.Collections.Generic;
using UnityEngine.InputSystem;
using System.Collections;
using UnityEngine;
using System;

public class GameManager : Singleton<GameManager>
{
    #region Properties

    public Action OnStartService, OnEndService;
    public InputActionReference Continue;
    public GameShift CurrentShift { get; private set; } = GameShift.Default;

    // DIFFICULTY
    public GameDifficulty Difficulty { get; private set; }
    public int MaxCustomerCount { get; private set; }
    public bool TutorialDone { get; set; }
    public bool IsGameOver { get; private set; }
    public bool IsPaused { get; private set; }
    public float CurrentPlayerMoney { get; private set; }

    #endregion  
    #region SerializeField

    [SerializeField] private float _testTimer;
    [SerializeField] private float _startingPlayerMoney;
    [SerializeField] private RestaurantReceipt _endOfDayReceipt;
    [SerializeField] private GameObject _logo;
    [SerializeField] private bool _logoRemoved = false;
    [SerializeField] private bool _isTutorial;

    [Header("Debugging")]
    [SerializeField] private bool _isDeveloperMode;

    #endregion
    #region Private

    // SCORING VALUES
    [SerializeField] private List<float> _customerSRScores;
    public int CustomersServed; // will be used for difficulty increase
    private float _finalScore;

    private const float MAX_MONEY = 9999f;
    private const float FIVE_MINUTES = 300f; // shift duration for Service
    private const float ONE_MINUTE = 60f; // shift duration for Pre-Service

    #endregion

    #region Unity

    protected override void OnApplicationQuit()
    {
        base.OnApplicationQuit();
        Continue.action.performed -= RemoveLogo;

        if (_isDeveloperMode)
            Debug.Log($"{this} developer mode: {_isDeveloperMode}");

        OnEndService -= ResetScores;
    }
    protected override void Awake() // set starting money
    {
        base.Awake();

        CurrentPlayerMoney = _startingPlayerMoney;
        CustomersServed = 0;
        MaxCustomerCount = 3;
        IsPaused = false;
        IsGameOver = false;
        Difficulty = GameDifficulty.EASY;

        // these should be empty when testing is done
        _customerSRScores = new List<float>(); // { 100f, 90f, 80f, 80f };

        Continue.action.Enable();
        Continue.action.performed += RemoveLogo;
        OnEndService += ResetScores;
    }
    private void Start() => StartCoroutine(CO_DelayedEventBind());
    private void Update() => Test();
    private void Test()
    {
        if (CurrentShift == GameShift.Service)
            ClockScript.Instance.UpdateNameOfPhaseTxt($"{CurrentShift}");

        if (!_isDeveloperMode) return;

        if (Input.GetKeyDown(KeyCode.Return))
        {
            Keyboard_RemoveLogo();
            Debug.Log("Removed Chop Chop Logo");
        }
        if (Input.GetKeyDown(KeyCode.Delete))
        {
            ChangeShift(GameShift.PostService);
        }
    }

    #endregion
    #region Public

    public void RemoveLogo(InputAction.CallbackContext context)
    {
        SoundManager.Instance.PlaySound("select");
        OnBoardingHandler.Instance.PlayOnboarding();

        // unpauses the game, removes logo, and start onboarding
        ChangeShift(GameShift.Training);
        Continue.action.Disable();
        _logoRemoved = true;
        _logo.SetActive(false);

        if (_logoRemoved)
        {
            Debug.Log("LOGO REMOVED");
            Continue.action.performed -= RemoveLogo;
        }
    }
    public void Keyboard_RemoveLogo()
    {
        SoundManager.Instance.PlaySound("select");
        OnBoardingHandler.Instance.PlayOnboarding();

        // unpauses the game, removes logo, and start onboarding
        ChangeShift(GameShift.Training);
        Continue.action.Disable();
        _logoRemoved = true;
        _logo.SetActive(false);

        if (_logoRemoved)
        {
            Debug.Log("LOGO REMOVED");
            Continue.action.performed -= RemoveLogo;
        }
    }
    public void TogglePause()
    {
        if (!SceneHandler.Instance.CanPause) return;

        IsPaused = !IsPaused;

        if (IsPaused)
        {
            Time.timeScale = 0f;
            MainMenuHandler.Instance?.TogglePlayIcon(false);
            MainMenuHandler.Instance?.TogglePausePanel(true);
        }
        else
        {
            Time.timeScale = 1f;
            MainMenuHandler.Instance?.TogglePausePanel(false);
            MainMenuHandler.Instance?.TogglePlayIcon(true);
        }
    }
    public void AddToCustomerScores(float n) => _customerSRScores.Add(n);
    public void IncrementCustomersServed() => CustomersServed++;
    public void AddMoney(float amt)
    {
        if (amt < 0f) return;

        CurrentPlayerMoney += amt;

        CurrentPlayerMoney = Mathf.Clamp(CurrentPlayerMoney, 0f, MAX_MONEY);
        SoundManager.Instance.PlaySound("earn money");
    }
    public void DeductMoney(float amt)
    {
        Debug.Log("Minus Player Money");
        if (amt < 0f) return;

        CurrentPlayerMoney -= amt;

        if (CurrentPlayerMoney < 0f)
            CurrentPlayerMoney = 0f;

        CurrentPlayerMoney = Mathf.Clamp(CurrentPlayerMoney, 0f, MAX_MONEY);
    }
    public void ChangeShift(GameShift chosenShift)
    {
        if (chosenShift == CurrentShift)
        {
            Debug.LogError("You cannot go to the same shift again!");
            return;
        }

        CurrentShift = chosenShift;

        switch (chosenShift)
        {
            case GameShift.Training: break;
            case GameShift.PreService: DoPreService(); break;
            case GameShift.Service: DoService(); break;
            case GameShift.PostService: DoPostService(); break;

            default:
                Debug.LogError("Invalid state chosen!");
                break;
        }
        SoundManager.Instance.PlaySound("change shift");
        Debug.LogWarning($"Shifted to {CurrentShift}"!);
    }
    public void EnableEOD()
    {
        if (_isTutorial)
            EnableEODReceipt();
    }
    public void ResetMGS()
    {
        ChangeShift(GameShift.PreService);
        ResetScores();
        Debug.LogWarning("Resetting MGS");
    }
    private void DisableTutorial()
    {
        if (!_isTutorial)
        {
            Debug.LogError($"{gameObject.name} is already not a tutorial!");
            return;
        }

        _isTutorial = false;
        OnBoardingHandler.Instance.OnTutorialEnd -= DisableTutorial;

        if (!_logoRemoved)
        {
            _logoRemoved = true;
            _logo.SetActive(false);
            Continue.action.performed += RemoveLogo;
        }
    }
    private void ResetScores()
    {
        if (_customerSRScores.Count > 0)
            _customerSRScores.Clear();

        _finalScore = 0f;
        CustomersServed = 0;
    }

    #endregion
    #region Game Shifts

    private void DoPreService() // change to 1 min when done testing
    {
        float serviceTimer = _isDeveloperMode ? _testTimer : ONE_MINUTE;

        // Debug.Log($"waiting {serviceTimer}s to change to service");
        StartCoroutine(CO_ShiftCountdown(serviceTimer, GameShift.Service));

        SoundManager.Instance.StopMusic();
        SoundManager.Instance.PlayMusic("pre-service bgm");

        ClockScript.Instance.UpdateTimeRemaining(serviceTimer);
        ClockScript.Instance.UpdateNameOfPhaseTxt($"{CurrentShift}");
    }
    private void DoService()
    {
        SoundManager.Instance.StopMusic();
        float timer = _isDeveloperMode ? _testTimer * 3f: FIVE_MINUTES; 

        SoundManager.Instance.PlayMusic("service bgm");
        ClockScript.Instance.UpdateTimeRemaining(timer);
        ClockScript.Instance.UpdateNameOfPhaseTxt($"{CurrentShift}");
        SoundManager.Instance.PlayMusic("bgm");

        OnStartService?.Invoke(); // all ingredients start decaying
        _finalScore = 0;

        // Debug.Log($"waiting {timer}s to change to service");
        StartCoroutine(KitchenCleaningManager.Instance.CO_EnableDirtyColliders());
        StartCoroutine(CO_ShiftCountdown(timer, GameShift.PostService));
    }
    private void DoPostService() // rating calculations
    {
        SoundManager.Instance.StopMusic();
        SoundManager.Instance.PlayMusic("post-service bgm");

        OnEndService?.Invoke();
        EnableEODReceipt();
        ClockScript.Instance.UpdateNameOfPhaseTxt($"{CurrentShift}");
    }
    private void ChangeDifficuty(int score)
    {
        if (score < 3) // player scored B or higher
        {
            if (Difficulty != GameDifficulty.HARD)
                Difficulty++;

            MaxCustomerCount++;
            KitchenCleaningManager.Instance.MaxDirtyColliders++;
        }
        else if (score == 3) // player scored C 
        {
            if (Difficulty != GameDifficulty.EASY)
                Difficulty++;

            if (MaxCustomerCount > 3)
            {
                MaxCustomerCount--;
                KitchenCleaningManager.Instance.MaxDirtyColliders--;
            }
        }
        else // player scored below C
        {
            StartCoroutine(CO_GameOver());
            return;
        }
    }

    #endregion
    #region EOD Rating

    private void EnableEODReceipt()
    {
        // enables the EOD receipt
        MainMenuHandler.Instance.ToggleEODPanel();
        _endOfDayReceipt = MainMenuHandler.Instance.gameObject?.
                           GetComponentInChildren<RestaurantReceipt>();

        // disables both the play button & live wallpaper
        MainMenuHandler.Instance.TogglePlayIcon(false);
        MainMenuHandler.Instance.ToggleLiveWallpaper();

        // prints the important stuff in the EOD receipt  
        DoCustomerRating();
        DoKitchenRating();
        DoPostServiceRating();
        
        // add the customers served in the EOD receipt
        CustomersServed = _isTutorial ? 2 : _endOfDayReceipt.totalcustomerServed;
        _endOfDayReceipt.GiveTotalCustomerServed();
    }
    private void DoCustomerRating()
    {
        if (_isTutorial) 
        {
            _endOfDayReceipt.GiveCustomerRating(0); // automatically gets a perfect score
            return;
        }

        float customerScore = IsGameOver ? 0f : GetAverageOf(_customerSRScores);
        int indexCustomerRating = _endOfDayReceipt.ReturnScoretoIndexRating(customerScore);
        
        _endOfDayReceipt.GiveCustomerRating(indexCustomerRating);
    }
    private void DoKitchenRating() 
    {   
        if (_isTutorial) 
        {
            _endOfDayReceipt.GiveKitchenRating(0); // automatically gets a perfect score
            return;
        }

        int indexKitchenRating = IsGameOver ? 4 :
            _endOfDayReceipt.ReturnScoretoIndexRating(KitchenCleaningManager.Instance.KitchenScore);
    
        _endOfDayReceipt.GiveKitchenRating(indexKitchenRating);
    }
    private void DoPostServiceRating() // FINAL SCORE 
    {
        if (_isTutorial) 
        {
            _endOfDayReceipt.GiveRestaurantRating(0); // automatically gets a perfect score
            return;
        }

        _finalScore = IsGameOver ? 4 : (KitchenCleaningManager.Instance.KitchenScore + 
                                        GetAverageOf(_customerSRScores)) / 2f;   
        
        int indexPostServiceRating = _endOfDayReceipt.ReturnScoretoIndexRating(_finalScore);

        _endOfDayReceipt.GiveRestaurantRating(indexPostServiceRating);
        ChangeDifficuty(indexPostServiceRating);
    }
    private float GetAverageOf(List<float> list)
    {
        // prevents a div/0 case
        if (list.Count < 1) return -1f;

        float n = 0f;

        Debug.LogWarning("Scores: ");
        for (int i = 0; i < list.Count; i++)
        {            
            Debug.LogWarning($"{list[i]}");
            n += list[i];
        }

        return n / list.Count;
    }
    

    #endregion

    #region Enumerators

    IEnumerator CO_ShiftCountdown(float timer, GameShift shift)
    {
        if (timer < 1) yield break;

        while (timer != 0)
        {
            yield return new WaitForSeconds(1f);
            timer--;
            ClockScript.Instance.UpdateTimeRemaining(timer);
        }

        SoundManager.Instance.PlaySound("change shift");
        ChangeShift(shift);
    }
    private IEnumerator CO_DelayedEventBind()
    {
        yield return new WaitForSeconds(1f);
        OnBoardingHandler.Instance.OnTutorialEnd += DisableTutorial;
    }
    public IEnumerator CO_CloseDownShop()
    {
        IsGameOver = true;
        SoundManager.Instance.PlaySound("game over 01");
        yield return new WaitForSeconds(4f);

        StopAllCoroutines();
        ChangeShift(GameShift.PostService);

        yield return StartCoroutine(CO_GameOver());
    }
    public IEnumerator CO_GameOver()
    {
        IsGameOver = true;
        SoundManager.Instance.PlaySound("game over 02");
        yield return new WaitForSeconds(2f);

        SceneHandler.Instance.LoadScene("TrainingScene");
        ChangeShift(GameShift.Training);
    }

#endregion
}

#region Enuemrations

    public enum GameDifficulty
    { 
        EASY, 
        NORMAL,
        HARD
    }

    public enum GameShift // IN A CERTAIN ORDER (DON'T RE-ORDER)
    {
        Default,
        Training,
        PreService,
        Service,
        PostService
    }

#endregion


/* -07.28.25- version
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class NEW_ColliderCheck : MonoBehaviour 
{
    #region Properties

    public CustomerOrder Order { get; set; }

    #endregion
    #region Private

    [SerializeField] private bool _isTutorial;
    [SerializeField] private float _dishPercantage = 0.8f;
    [SerializeField] private float _patiencePercentage = 0.2f;
    [SerializeField] private float _disableTimer;

    private Collider _collider;

    [Header("Debugging")]
    [SerializeField] private bool _isDevloperMode;

    #endregion

    #region Unity

    private void Awake()
    {
        _collider = GetComponent<BoxCollider>();
        if (_isDevloperMode)
            Debug.Log($"{this} developer mode: {_isDevloperMode}");

        if (_isTutorial)
            Debug.Log($"{this} tutorial mode: {_isTutorial}"); 
    }
    private void Start()
    {
        OnBoardingHandler.Instance.OnTutorialEnd += DisableTutorial;

        _collider.isTrigger = true;
        _collider.enabled = true;
        _disableTimer = 3f; 
    }
    private void Update() => test();
    private void OnTriggerEnter(Collider other)
    {
        if (Order == null)
        {
            Debug.LogError($"{Order} is null!");
            SoundManager.Instance.PlaySound("wrong");
            return;
        }
        if (other.gameObject.GetComponent<Ingredient>() != null)
        {            
            DoIngredientCollision(other.gameObject.GetComponent<Ingredient>());
            return;
        }

        NEW_Plate plate = other.gameObject.GetComponent<NEW_Plate>();
        NEW_Dish dish = other.gameObject.GetComponent<NEW_Dish>();

        // makes sure that you have both a PLATE & DISH script
        if (dish != null && plate != null)
        {
            DoDishCollision(dish, plate);
            Debug.Log("Finished dish collision!");
        }
    }
    private void OnDestroy() => 
        OnBoardingHandler.Instance.OnTutorialEnd -= DisableTutorial;
    
    private void test()
    {
        if (!_isDevloperMode) return;

        if (Input.GetKeyDown(KeyCode.Tab))
            Debug.Log($"{this} wanted plater: {Order.WantedPlatter}");        
    }

    #endregion
    #region Helpers

    private void DoIngredientCollision(Ingredient ing)
    {
        if (GameManager.Instance.CurrentShift != GameShift.Training)
        {
            Order.CustomerSR = 0f;
            StartCoroutine(CO_DisableCollider());
            StartCoroutine(Order.CO_AngryReaction());
            StartCoroutine(GameManager.Instance.CO_GameOver());
        }
        else 
        {
            SoundManager.Instance.PlaySound("wrong");
            SoundManager.Instance.PlaySound("ingredient order");
            Debug.LogError("Player served a ingredient to the customer!");
        }
    }
    private void DoDishCollision(NEW_Dish dish, NEW_Plate plate)
    {
        // customer's reaction when getting the dish
        CheckFoodConition(dish); 
        StartCoroutine(CO_DisableCollider());
        
        dish.DisableDish();
        plate.Served();          
    }
    private void CheckFoodConition(NEW_Dish dish)
    {
        if (dish.FoodCondition != FoodCondition.CLEAN)
        {
            TriggerContainatedOrder();  
            Debug.LogError("Triggered dirty order!");         
        }
        else if (dish.DishPlatter != Order.WantedPlatter) 
        {
            TriggerWrongOrder();                   
            Debug.LogError("Triggered wrong order!");
        }
        else
        {
            TriggerCorrectOrder(dish);
            TriggerOnboarding();
            Debug.LogWarning("Triggered correct order!");         
        }
    }
    private void TriggerContainatedOrder()
    {
        if (GameManager.Instance.CurrentShift != GameShift.Training)
        {       
            Order.CustomerSR = 0f;
            StartCoroutine(Order.CO_DirtyReaction());
            Debug.LogError("Player served a dirty order!");        
        }
        else
        {
            SoundManager.Instance.PlaySound("wrong");
            SoundManager.Instance.PlaySound("contaminated order");
        }        
    }
    private void TriggerWrongOrder()
    {
        if (GameManager.Instance.CurrentShift != GameShift.Training)
        {
            Order.CustomerSR = 0f;
            GameManager.Instance.AddToCustomerScores(Order.CustomerSR);
            StartCoroutine(Order.CO_AngryReaction());
            Debug.LogError("Player served the wrong order!");
        }
        else
        {
            SoundManager.Instance.PlaySound("wrong");
            SoundManager.Instance.PlaySound("wrong order");
        }
    }
    private void TriggerCorrectOrder(NEW_Dish dish)
    {
        // UX after serving the customer
        float dishScore = dish.Score * _dishPercantage;
        float patienceScore = Order.PatienceRate * _patiencePercentage;
        Order.CustomerSR = (dishScore + patienceScore) / 2f; // dish quality has more focus becuase of CAPSTN
        
        GameManager.Instance.AddToCustomerScores(Order.CustomerSR);
        StartCoroutine(Order.CO_HappyReaction());
        // Destroy(Order.gameObject);
    }
    private void TriggerOnboarding()
    {
        if (!_isTutorial) return;

        OnBoardingHandler.Instance.AddOnboardingIndex();
        OnBoardingHandler.Instance.PlayOnboarding();

        if (Order.IsTunaCustomer)
        {
            ShopManager.Instance.ClearList();
            Debug.LogWarning("Benny was served!");
        }
        else Debug.LogWarning("Atrium was served!");
    }
    public void DisableTutorial() => _isTutorial = false;

    #endregion
    
    #region Enumerators

    private IEnumerator CO_DisableCollider()
    {
        _collider.enabled = false;
        Debug.LogWarning("Collider disabled!");
        yield return new WaitForSeconds(_disableTimer);

        _collider.enabled = true;
        Debug.LogWarning("Collider enabled!");

        //Order = null;
        Debug.LogWarning("CustomerOrder is now null!");
    }

    #endregion  
}
*/