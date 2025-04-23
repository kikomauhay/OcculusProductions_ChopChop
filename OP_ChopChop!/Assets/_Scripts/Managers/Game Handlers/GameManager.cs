using System.Collections.Generic;
using UnityEngine.InputSystem;
using System.Collections;
using UnityEngine;
using System;
using System.Runtime.CompilerServices;
using System.Reflection;
using Unity.VisualScripting;

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

    public Action OnStartService, OnEndService, OnTraining;

    public GameShift CurrentShift { get; private set; } = GameShift.DEFAULT;
    
    // DIFFICULTY
    public GameDifficulty Difficulty { get; private set; }
    public int MaxCustomerCount { get; set; }

    public InputActionReference Continue;

    public bool IsGameOver { get; private set; }
    public bool IsPaused { get; private set; }
    public float CurrentPlayerMoney { get; private set; }
    public const float MAX_MONEY = 9999f;

    private const float FIVE_MINUTES = 300f; // shift duration for Service
    private const float ONE_MINUTE = 60f; // shift duration for Service

    // SCORING VALUES
    private List<float> _customerSRScores;
    public int CustomersServed; // will be used for difficulty increase
    private float _finalScore;

    [SerializeField] private float _testTimer;
    [SerializeField] private float _startingPlayerMoney;
    [SerializeField] private RestaurantReceipt _endOfDayReceipt;
    [SerializeField] private GameObject _logo;
    [Space(10f), SerializeField] private bool _isDebugging;

#endregion

#region Unity

    protected override void OnApplicationQuit()
    {
        base.OnApplicationQuit(); 
        Continue.action.performed -= RemoveLogo;
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
        _customerSRScores = new List<float>() { 100f, 90f, 80f, 80f };

        Continue.action.Enable();
        Continue.action.performed += RemoveLogo;
    }

    void RemoveLogo(InputAction.CallbackContext context)
    {
        SoundManager.Instance.PlaySound("select", SoundGroup.GAME);

        // unpause game, remove logo, and start onboarding
        ChangeShift(GameShift.TRAINING);
        Continue.action.Disable();
        _logo.SetActive(false);
        
        // used for testing so that you won't hear the voice lines in MGS
        //if (CurrentShift == GameShift.PRE_SERVICE) return;

        StartCoroutine(OnBoardingHandler.Instance.CallOnboarding(0));
    }

    IEnumerator ShiftCountdown(float timer, GameShift shift)
    {
        yield return new WaitForSeconds(timer);
        SoundManager.Instance.PlaySound("change shift", SoundGroup.GAME);
        ChangeShift(shift);
    }

#endregion

#region Public

    // GAME PAUSING
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

    // SCORING-RELATED
    public void AddToCustomerScores(float n) => _customerSRScores.Add(n);
    public void IncrementCustomersServed() => CustomersServed++;

    // CASH-RELATED
    public void AddMoney(float amt)
    {
        if (amt < 0f) return;

        CurrentPlayerMoney += amt;

        CurrentPlayerMoney = Mathf.Clamp(CurrentPlayerMoney, 0f, MAX_MONEY);
        SoundManager.Instance.PlaySound("earn money", SoundGroup.GAME);
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

    // GAME OVER-RELATED

    public IEnumerator CloseDownShop()
    {
        IsGameOver = true;
        SoundManager.Instance.PlaySound("game over 01", SoundGroup.GAME);
        yield return new WaitForSeconds(4f);

        StopAllCoroutines();
        ChangeShift(GameShift.POST_SERVICE);
    }
    public IEnumerator GameOver()
    {
        IsGameOver = true;
        SoundManager.Instance.PlaySound("game over 02", SoundGroup.GAME);
        yield return new WaitForSeconds(2f);

        SceneHandler.Instance.LoadScene("TrainingScene");
    }

#endregion

#region GameShifts

    public void ChangeShift(GameShift chosenShift)
    {
        // can't go to the same shift again
        if (chosenShift == CurrentShift) return;

        CurrentShift = chosenShift;

        switch (chosenShift)
        {
            case GameShift.TRAINING:     OnTraining?.Invoke(); break;
            case GameShift.PRE_SERVICE:  DoPreService();       break;
            case GameShift.SERVICE:      DoService();          break;
            case GameShift.POST_SERVICE: DoPostService();      break;

            default:
                Debug.LogError("Invalid state chosen!");
                break;
        }
        SoundManager.Instance.PlaySound("change shift", SoundGroup.GAME);
    }

    void DoPreService() // change to 1 min when done testing
    {
        float serviceTimer = _isDebugging ? _testTimer : ONE_MINUTE;
        
        Debug.Log($"waiting {serviceTimer}s to change to service");
        StartCoroutine(ShiftCountdown(serviceTimer, GameShift.SERVICE));

        ClockScript.Instance.UpdateTimeRemaining(serviceTimer);
        ClockScript.Instance.UpdateNameOfPhaseTxt("Pre-Service");
    }     
    void DoService()
    {
        float timer = _isDebugging ? _testTimer * 10f : FIVE_MINUTES; 

        OnStartService?.Invoke(); // all ingredients start decaying
        _finalScore = 0;

        // change to 5 mins when done testing
        Debug.Log($"waiting {timer}s to change to service");
        StartCoroutine(ShiftCountdown(timer, GameShift.POST_SERVICE));

        ClockScript.Instance.UpdateTimeRemaining(_testTimer);
        ClockScript.Instance.UpdateNameOfPhaseTxt("Service");
    }
    void DoPostService() // rating calculations
    {
        OnEndService?.Invoke(); 
        TurnOnEndOfDayReceipt();
    }

#endregion

#region EOD_Rating

    void TurnOnEndOfDayReceipt()
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
        CustomersServed = _endOfDayReceipt.totalcustomerServed;
        _endOfDayReceipt.GiveTotalCustomerServed();
    }
    void DoCustomerRating()
    {
        float customerScore = IsGameOver ? 0f : GetAverageOf(_customerSRScores);
        int indexCustomerRating = _endOfDayReceipt.ReturnScoretoIndexRating(customerScore);
        
        _endOfDayReceipt.GiveCustomerRating(indexCustomerRating);
    }
    void DoKitchenRating() 
    {   
        int indexKitchenRating = IsGameOver ? 4 :
            _endOfDayReceipt.ReturnScoretoIndexRating(KitchenCleaningManager.Instance.KitchenScore);
    
        _endOfDayReceipt.GiveKitchenRating(indexKitchenRating);
    }
    void DoPostServiceRating() // FINAL SCORE 
    {
        _finalScore = IsGameOver ? 4 : (KitchenCleaningManager.Instance.KitchenScore + 
                                        GetAverageOf(_customerSRScores)) / 2f;   
        
        int indexPostServiceRating = _endOfDayReceipt.ReturnScoretoIndexRating(_finalScore);

        _endOfDayReceipt.GiveRestaurantRating(indexPostServiceRating);
        ChangeDifficuty(indexPostServiceRating);
    }

#endregion

#region Helpers

    void ChangeDifficuty(int score)
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
            StartCoroutine(GameOver());
            return;
        }

        // press continue in the UI button to go to the next shift
            // fade in fade out
            // change back to pre service

        // player lifts the side counter door and goes back to training scene
            // 
    }
    float GetAverageOf(List<float> list) 
    {
        // prevents a div/0 case
        if (list.Count < 1) return -1f;
        
        float n = 0f;

        for (int i = 0; i < list.Count; i++) 
            n += list[i];

        return n / list.Count;
    }

#endregion
}
