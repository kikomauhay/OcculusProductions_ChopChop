using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using System;
using Unity.Mathematics;
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

    public bool IsPaused { get; private set; }
    public bool CanPause { get; private set; }
    public float CurrentPlayerMoney { get; private set; }
    public const float MAX_MONEY = 9999f;

    const float FIVE_MINUTES = 300f; // shift duration for Service

    // SCORING VALUES
    List<float> _customerSRScores;
    public int CustomersServed; // will be used for difficulty increase 

    [SerializeField] float _startingPlayerMoney;
    [SerializeField] RestaurantReceipt _endOfDayReceipt;

#endregion

#region Unity_Methods

    protected override void Awake() 
    {
        base.Awake();
        CurrentPlayerMoney = _startingPlayerMoney;
    }
    protected override void OnApplicationQuit() => base.OnApplicationQuit();
    void Start() 
    {
        CustomersServed = 0;

        CanPause = true;
        IsPaused = false;

        _customerSRScores = new List<float>(); // { 100f, 90f, 80f, 80f }; 
        ChangeShift(GameShift.SERVICE);

        Debug.Log(CurrentPlayerMoney);
    }
    IEnumerator StartShiftCountdown()
    {
        yield return new WaitForSeconds(300f); // 5 minutes
        ChangeShift(GameShift.POST_SERVICE);
    }
    
#endregion

#region Public

    // GAME PAUSING
    public void TogglePause()
    {
        if (!CanPause) return;

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
    
    // GAME SHIFTING
    public void ChangeShift(GameShift chosenShift)
    {
        // can't go to the same shift again
        if (chosenShift == CurrentShift) return;

        CurrentShift = chosenShift;

        switch (chosenShift)
        {   
            case GameShift.TRAINING:
                DoTraining();
                break;         

            case GameShift.PRE_SERVICE:
                DoPreService();
                break;

            case GameShift.SERVICE:
                DoService();
                break; 

            case GameShift.POST_SERVICE:
                DoPostService();
                break; 

            default:
                Debug.LogError("Invalid state chosen");
                break;
        }
    }

#endregion

#region Game_Shifts

    void DoTraining() // sandbox mode
    {
        // no ingredient decaying or equipment dirtying
        OnTraining?.Invoke();
        CanPause = true;
    }
    void DoPreService() => 
        StartCoroutine(ShiftCountdown(5f, GameShift.SERVICE)); 
    void DoService() // customer spawning + cooking, serving, & cleaning
    {
        OnStartService?.Invoke(); // all ingredients start decaying

        // sample 2 mins for testing
        StartCoroutine(ShiftCountdown(120f, GameShift.POST_SERVICE)); 
        // StartCoroutine(ShiftCountdown(FIVE_MINUTES, GameShift.POST_SERVICE));
    }

    void DoPostService()
    {
        // forces to expire all remaining ingredients
        // remaining customers despawn + their order UI 
        OnEndService?.Invoke(); 

        // enables EOD receipt in the right side of the game
        TurnOnEndOfDayReceipt();

        // goes back to pre-service to test
        StartCoroutine(ShiftCountdown(20f, GameShift.PRE_SERVICE));
    }
    IEnumerator ShiftCountdown(float timer, GameShift shift)
    {
        yield return new WaitForSeconds(timer);
        SoundManager.Instance.PlaySound("change shift", SoundGroup.GAME);
        ChangeShift(shift);
    }

#endregion

#region Resto_Rating
    void DoCustomerRating()
    {
        float customerScore = GetAverageOf(_customerSRScores);

        int indexCustomerRating = _endOfDayReceipt.ReturnScoretoIndexRating(customerScore);

        _endOfDayReceipt.GiveCustomerRating(indexCustomerRating);
    }
    void DoKitchenRating()
    {   
        float kitchenScore = KitchenCleaningManager.Instance.KitchenScore;

        int indexKitchenRating = _endOfDayReceipt.ReturnScoretoIndexRating(kitchenScore);

        _endOfDayReceipt.GiveKitchenRating(indexKitchenRating);
    }
    void DoPostServiceRating()
    {
        // END-OF-DAY RESTAURANT RATING 
        float finalScore = (KitchenCleaningManager.Instance.KitchenScore + 
                            GetAverageOf(_customerSRScores)) / 2f;      

    
        int indexPostServiceRating = _endOfDayReceipt.ReturnScoretoIndexRating(finalScore);

        _endOfDayReceipt.GiveRestaurantRating(indexPostServiceRating);
    }
    void TurnOnEndOfDayReceipt()
    {
        CanPause = false;
        
        // enables the EOD receipt
        MainMenuHandler.Instance.ToggleEODPanel();
        _endOfDayReceipt = MainMenuHandler.Instance.gameObject?.
                           GetComponentInChildren<RestaurantReceipt>();

        // turns OFF the play button & live wallpaper
        MainMenuHandler.Instance.TogglePlayIcon(false); 
        MainMenuHandler.Instance.ToggleLiveWallpaper();

        // Gives the lettered-score to the EOD receipt  
        DoCustomerRating();
        DoKitchenRating();
        DoPostServiceRating();

        CustomersServed = _endOfDayReceipt.totalcustomerServed;
        _endOfDayReceipt.GiveTotalCustomerServed();
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
