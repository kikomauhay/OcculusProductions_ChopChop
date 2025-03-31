using System.Collections.Generic;
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

    public Action OnStartService, OnEndService, OnTraining;

    public GameShift CurrentShift { get; private set; } = GameShift.DEFAULT;
    
    // DIFFICULTY
    public GameDifficulty Difficulty { get; private set; }
    public int MaxCustomerCount { get; set; }


    public bool IsPaused { get; private set; }
    public float CurrentPlayerMoney { get; private set; }
    public const float MAX_MONEY = 9999f;

    const float FIVE_MINUTES = 300f; // shift duration for Service

    // SCORING VALUES
    List<float> _customerSRScores;
    public int CustomersServed; // will be used for difficulty increase
    float _finalScore;

    [SerializeField] float _testTimer;
    [SerializeField] float _startingPlayerMoney;
    [SerializeField] RestaurantReceipt _endOfDayReceipt;

#endregion

#region Unity_Methods

    protected override void OnApplicationQuit() => base.OnApplicationQuit();
    protected override void Awake() // set starting money
    {
        base.Awake();

        CurrentPlayerMoney = _startingPlayerMoney;
        CustomersServed = 0;
        IsPaused = false;
        Difficulty = GameDifficulty.EASY;

        // these should be empty when testing is done
        _customerSRScores = new List<float>() { 100f, 90f, 80f, 80f };
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

    void DoPreService() => // change to 3 mins when done testing
        StartCoroutine(ShiftCountdown(30f, GameShift.SERVICE));         
    
    void DoService() // customer spawning + cooking, serving, & cleaning
    {
        OnStartService?.Invoke(); // all ingredients start decaying
        _finalScore = 0;

        // change to 5 mins when done testing
        StartCoroutine(ShiftCountdown(120f, GameShift.POST_SERVICE)); 
    }
    void DoPostService() // rating calculations
    {
        OnEndService?.Invoke(); 
        TurnOnEndOfDayReceipt();
        StartCoroutine(SceneHandler.Instance.LoadScene("TrainingScene"));
    }

#endregion

#region EOD_Rating

    void TurnOnEndOfDayReceipt()
    {
        // enables the EOD receipt
        MainMenuHandler.Instance.ToggleEODPanel();
        _endOfDayReceipt = MainMenuHandler.Instance.gameObject?.
                        GetComponentInChildren<RestaurantReceipt>();

        // disableas both the play button & live wallpaper
        MainMenuHandler.Instance.TogglePlayIcon(false);
        MainMenuHandler.Instance.ToggleLiveWallpaper();

        // prints the important stuff in the EOD receipt  
        DoCustomerRating();
        DoKitchenRating();
        DoPostServiceRating();

        // add the customers served in the EOD receipt
        CustomersServed = _endOfDayReceipt.totalcustomerServed;
        _endOfDayReceipt.GiveTotalCustomerServed();

        // IMPLEMENT CODE TO CHANGE DIFFICULTY BASED ON _finalScore
    }
    void DoCustomerRating()
    {
        float customerScore = GetAverageOf(_customerSRScores);
        int indexCustomerRating = _endOfDayReceipt.ReturnScoretoIndexRating(customerScore);
        _endOfDayReceipt.GiveCustomerRating(indexCustomerRating);
    }
    void DoKitchenRating() 
    {   
        int indexKitchenRating = 
            _endOfDayReceipt.ReturnScoretoIndexRating(KitchenCleaningManager.Instance.KitchenScore);
        _endOfDayReceipt.GiveKitchenRating(indexKitchenRating);
    }
    void DoPostServiceRating() // FINAL SCORE 
    {
        _finalScore = (KitchenCleaningManager.Instance.KitchenScore + 
                       GetAverageOf(_customerSRScores)) / 2f;   
        
        int indexPostServiceRating = _endOfDayReceipt.ReturnScoretoIndexRating(_finalScore);
        _endOfDayReceipt.GiveRestaurantRating(indexPostServiceRating);
    }

#endregion

    float GetAverageOf(List<float> list) 
    {
        // prevents a div/0 case
        if (list.Count < 1) return -1f;
        
        float n = 0f;

        for (int i = 0; i < list.Count; i++) 
            n += list[i];

        return n / list.Count;
    }
}
