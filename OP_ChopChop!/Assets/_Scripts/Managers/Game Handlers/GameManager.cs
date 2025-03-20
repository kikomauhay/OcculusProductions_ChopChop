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

    public Action OnStartService, OnEndService;

    public GameShift CurrentShift { get; private set; } = GameShift.DEFAULT;
    public float AvailableMoney { get; private set; }
    public bool IsPaused { get; private set; }
    public bool CanPause { get; private set; }

    // SCORING VALUES
    List<float> _customerSRScores;

    public int CustomersServed;

#endregion

#region Unity_Methods

    protected override void Awake() => base.Awake();
    protected override void OnApplicationQuit() => base.OnApplicationQuit();
    void Start() 
    {
        // _customerSRScores = new List<float>() { 100f, 90f, 80f, 70f }; 
       
        AvailableMoney = 0f;
        CustomersServed = 0;
        CanPause = true;
        IsPaused = false;

        ChangeShift(GameShift.SERVICE);
    }
    IEnumerator StartShiftCountdown()
    {
        yield return new WaitForSeconds(10f);
        ChangeShift(GameShift.POST_SERVICE);
    }
    IEnumerator TestShifCountdown(float timer, GameShift shift)
    {
        yield return new WaitForSeconds(timer);
        ChangeShift(shift);
        Debug.ClearDeveloperConsole();
    }

#endregion

#region Public

    // GAME PAUSING
    public void TogglePause()
    {
        if (!CanPause) return;

        IsPaused = !IsPaused;

        if (IsPaused) 
            Time.timeScale = 0f;
        
        else 
            Time.timeScale = 1f;
    }

    // SCORING-RELATED
    public void AddToCustomerScores(float n) => _customerSRScores.Add(n);

    // CASH-RELATED
    public void AddMoney(float amt) 
    {
        if (amt < 0f) return;

        AvailableMoney += amt;

        // idk if theres's a max cap when it comes to money
    }
    public void DeductMoney(float amt)
    {
        if (amt < 0f) return;

        AvailableMoney -= amt;

        if (AvailableMoney < 0f)
            AvailableMoney = 0f;
    }
    
    // GAME SHIFT CHANGING
    public void ChangeShift(GameShift chosenShift)
    {
        if (chosenShift == CurrentShift) return;

        CurrentShift = chosenShift;

        switch (chosenShift)
        {   
            case GameShift.PRE_PRE_SERVICE:
                DoPrePreSerice();
                break;         

            case GameShift.PRE_SERVICE:
                DoPreSerice();
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

    void DoPrePreSerice() // spectator mode
    {
        // no ingredient decaying or equipment dirtying

        Debug.LogWarning("Player is viewing the world!");
        Debug.Log("Wait 5s");
        StartCoroutine(TestShifCountdown(5f, GameShift.PRE_SERVICE));        
    }
    void DoPreSerice()
    {
        // buying of ingredients
        // rice cooking preparation
        // storing ingredients
        // clean kitchen 

        Debug.LogWarning("Player is buying ingredients!");
        Debug.Log("Wait 5s");
        StartCoroutine(TestShifCountdown(5f, GameShift.SERVICE)); 
    }
    void DoService() // customer spawning + cooking, serving, & cleaning
    {
        OnStartService?.Invoke(); // all ingredients start decaying

        // StartCoroutine(StartShiftCountdown());

        /*
        Debug.LogWarning("Customer spawning & food serving!");
        Debug.Log("Wait 20s");
        StartCoroutine(TestShifCountdown(20f, GameShift.POST_SERVICE)); */
    }
    void DoPostService()
    {
        OnEndService?.Invoke(); // forces to expire all remaining ingredients

        // shop closes and you get the rating for the day
        // clean the remaining dishes

        DoPostServiceRating();

        Debug.LogWarning("It's the end of the day!");
        Debug.Log("Wait 10s");
        StartCoroutine(TestShifCountdown(10f, GameShift.PRE_PRE_SERVICE)); 
    }

#endregion

#region Resto_Rating

    void DoPostServiceRating()
    {
        // END-OF-DAY RESTAURANT RATING 
        float finalScore = (CleaningManager.Instance.KitchenScore + 
                            GetAverageOf(_customerSRScores)) / 2f;

        Debug.LogWarning($"Final score for the day: {finalScore}");

        /* UI CODE 
            - shows the total customers served
            - shows the amt of customers that left
            - show the LETTERED SCORE to the player 
            - shows how much money you gained
            - add a button where the player goes back to the pre pre service 
        */
    }
    float GetAverageOf(List<float> list) 
    {
        // prevents a div/0 case
        if (list.Count < 1) return float.NaN;
        
        float n = 0f;

        for (int i = 0; i < list.Count; i++) 
            n += list[i];

        return n / list.Count;
    }

#endregion

#region Testing

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
            ChangeShift(GameShift.PRE_PRE_SERVICE);
            
            // throw new NullReferenceException("test");
            //Debug.Log($"Total Score: {GetAverageOf(_foodScores)}");
    }  

    IEnumerator PrintState() 
    { 
        while (true) 
        {
            yield return new WaitForSeconds(2f);
            Debug.Log(CurrentShift);
        }
    }

#endregion

}
