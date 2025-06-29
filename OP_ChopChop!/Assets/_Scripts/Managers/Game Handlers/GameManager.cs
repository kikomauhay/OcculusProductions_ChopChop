using System.Collections.Generic;
using UnityEngine.InputSystem;
using System.Collections;
using UnityEngine;
using System;

/// <summary> -WHAT DOES THIS SCRIPT DO-
///
/// - Acts as the brains of the game.
/// - Stores all the scores of the food served and customerSR.
/// - Rates the Restaurant Quality once the shift ends.
///
/// </summary>

/// <summary> -RATING PROCESS-
/// 
/// 1. Calculate the Food Score, then add it to the proper list
/// 2. Calculate Customer SR, then add it to the proper list
/// 3. At the end of the day, calculate the overall restaurant rate
/// 
/// </summary>

public class GameManager : Singleton<GameManager>
{
#region Members

#region Public

    public Action OnStartService, OnEndService;
    public InputActionReference Continue;

#endregion
#region Properties

    public GameShift CurrentShift { get; private set; } = GameShift.Default;
    
    // DIFFICULTY
    public GameDifficulty Difficulty { get; private set; }
    public int MaxCustomerCount { get; set; }
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
    private List<float> _customerSRScores;
    public int CustomersServed; // will be used for difficulty increase
    private float _finalScore;

    private const float MAX_MONEY = 9999f;
    private const float FIVE_MINUTES = 300f; // shift duration for Service
    private const float ONE_MINUTE = 60f; // shift duration for Service

    #endregion

    #endregion

    #region Methods

    #region Unity

    protected override void OnApplicationQuit()
    {
        base.OnApplicationQuit();
        Continue.action.performed -= RemoveLogo;
        
         if (_isDeveloperMode)
            Debug.Log($"{this} developer mode: {_isDeveloperMode}");
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
    }
    private void Update()
    {
        if (CurrentShift == GameShift.Service)
            ClockScript.Instance.UpdateNameOfPhaseTxt($"{CurrentShift}");

        if (!_isDeveloperMode) return;

        if (Input.GetKeyDown(KeyCode.Return))
        {
            Keyboard_RemoveLogo();
            Debug.Log("Removed Chop Chop Logo");
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
            case GameShift.Training:                     break;
            case GameShift.PreService:  DoPreService();  break;
            case GameShift.Service:     DoService();     break;
            case GameShift.PostService: DoPostService(); break;

            default:
                Debug.LogError("Invalid state chosen!");
                break;
        }
        SoundManager.Instance.PlaySound("change shift");
    } 
    public void EnableEOD()
    {
        if (_isTutorial)
            TurnOnEndOfDayReceipt();
    }

#endregion
#region Private

#region Game-related

    private void DoPreService() // change to 1 min when done testing
    {
        ClockScript.Instance.UpdateNameOfPhaseTxt("Pre-Service");

        float serviceTimer = _isDeveloperMode ? _testTimer : ONE_MINUTE;
        
        Debug.Log($"waiting {serviceTimer}s to change to service");
        StartCoroutine(CO_ShiftCountdown(serviceTimer, GameShift.Service));

        // ClockScript.Instance.UpdateTimeRemaining(serviceTimer);
    }     
    private void DoService()
    {
        ClockScript.Instance.UpdateNameOfPhaseTxt("Service");
        SoundManager.Instance.PlayMusic("bgm");

        float timer = _isDeveloperMode ? _testTimer * 10f : FIVE_MINUTES; 

        OnStartService?.Invoke(); // all ingredients start decaying
        _finalScore = 0;

        // change to 5 mins when done testing
        Debug.Log($"waiting {timer}s to change to service");
        StartCoroutine(CO_ShiftCountdown(timer, GameShift.PostService));

        // ClockScript.Instance.UpdateTimeRemaining(_testTimer);
    }
    private void DoPostService() // rating calculations
    {
        OnEndService?.Invoke();
        TurnOnEndOfDayReceipt();
    }
    private void ChangeDifficuty(int score)
    {
        if (score < 3) // B or higher
        {
            if (Difficulty != GameDifficulty.HARD)
                Difficulty++;
 
            MaxCustomerCount++;
        }
        else if (score == 3) // C
        {
            if (Difficulty != GameDifficulty.EASY)
                Difficulty++;

            if (MaxCustomerCount > 3)
                MaxCustomerCount--;
        }
        else 
        {
            StartCoroutine(CO_GameOver());
            return;
        }

        // press continue in the UI button to go to the next shift
            // fade in fade out
            // change back to pre service

        // player lifts the side counter door and goes back to training scene
            // 
    }

#endregion
#region End-Of-Day Rating

   
    private void TurnOnEndOfDayReceipt()
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

        for (int i = 0; i < list.Count; i++) 
            n += list[i];

        return n / list.Count;
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
    }

#endregion

#endregion

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
    IEnumerator CO_DelayedEventBind()
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