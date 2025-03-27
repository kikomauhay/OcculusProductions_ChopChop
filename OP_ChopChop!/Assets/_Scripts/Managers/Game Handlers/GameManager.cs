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
        _endOfDayReceipt = MainMenuHandler.Instance.gameObject?.
                           GetComponentInChildren<RestaurantReceipt>();

        CustomersServed = 0;

        CanPause = true;
        IsPaused = false;

        _customerSRScores = new List<float>(); 
        ChangeShift(GameShift.PRE_SERVICE);

        Debug.Log(_endOfDayReceipt.gameObject.activeSelf);
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
    }
    public void DeductMoney(float amt)
    {
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
        StartCoroutine(TestShiftCountdown(30f, GameShift.SERVICE)); 
    
    void DoService() // customer spawning + cooking, serving, & cleaning
    {
        OnStartService?.Invoke(); // all ingredients start decaying

        // 5 min timer once the shift ends
        // StartCoroutine(StartShiftCountdown());

        // sample 2 mins for testing
        StartCoroutine(TestShiftCountdown(120f, GameShift.POST_SERVICE)); 
    }

    void DoPostService()
    {
        OnEndService?.Invoke(); // forces to expire all remaining ingredients
        TurnOnEndOfDayReceipt();

        // goes back to pre-service to test
        StartCoroutine(TestShiftCountdown(20f, GameShift.PRE_SERVICE));
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
        MainMenuHandler.Instance.TogglePlayIcon(false); //turns OFF the play button to display the receipt screen
        MainMenuHandler.Instance.ToggleEODPanel();
        MainMenuHandler.Instance.ToggleLiveWallpaper();

        CanPause = false;

        _endOfDayReceipt.gameObject.SetActive(true);

        // DoCustomerRating();
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

#region Testing

    // TESTING
    float testTimer = 2f;

    void test() 
    {
        if (Input.GetKeyDown(KeyCode.Escape)) 
        {
            float rndm = UnityEngine.Random.Range(80f, 100f);
            _customerSRScores.Add(rndm);

            Debug.Log($"Score: {rndm}; Count: {_customerSRScores.Count}");
        }        

        if (Input.GetKeyDown(KeyCode.Alpha1)) ChangeShift(GameShift.PRE_SERVICE);

        if (Input.GetKeyDown(KeyCode.Alpha2)) ChangeShift(GameShift.SERVICE);

        if (Input.GetKeyDown(KeyCode.Return) && CurrentShift == GameShift.POST_SERVICE)
            ChangeShift(GameShift.TRAINING);
            
        // throw new NullReferenceException("test");
        //Debug.Log($"Total Score: {GetAverageOf(_foodScores)}");
    }  
    IEnumerator TestShiftCountdown(float timer, GameShift shift)
    {
        yield return new WaitForSeconds(timer);
        ChangeShift(shift);
    }

#endregion
}
