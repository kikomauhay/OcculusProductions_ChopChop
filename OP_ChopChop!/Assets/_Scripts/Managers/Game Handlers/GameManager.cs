using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEditor;
using UnityEngine.UIElements;

/// <summary> -WHAT DOES THIS SCRIPT DO-
///
/// Acts as the brains of the game.
/// Stores all the scores of the food served and customerSR.
/// Rates the Restaurant Quality once the shift ends.
///
/// </summary>

/// </summary> -RATING PROCESS-
/// 
/// 1. Calculate the Food Score, then add it to the proper list
/// 2. Calculate Customer SR, then add it to the proper list
/// 3. At the end of the day, calculate the overall restaurant rate
/// 
/// </summary>

public class GameManager : Singleton<GameManager>
{
#region Members

    public Action OnStartGame, OnCustomerLeft;
    public Action OnPhaseChanged; 
    public Action<float> OnCustomerServed, OnMoneyChanged;

    public float AvailableMoney { get; private set; }
    public GamePhase CurrentPhase { get; private set; }  

    // Scores to keep track of
    List<float> _foodScores, _customerSRScores;

    int _customersThatLeftCounter;

#endregion

#region Unity_Methods

    protected override void Awake() 
    {
        base.Awake();

        OnCustomerServed += AddToFoodScore;
        OnCustomerLeft += IncrementCustomersLeft;
        OnMoneyChanged += ChangeMoney;
    }
    void Start() 
    {
        _foodScores = new List<float>();
        _customerSRScores = new List<float>();
        
        _customersThatLeftCounter = 0;
        AvailableMoney = 0f;

        ChangePhase(GamePhase.PRE_PRE_SERVICE);
    }
    void Update() => test();
    
    void Reset()
    {
        OnCustomerServed -= AddToFoodScore;
        OnCustomerLeft -= IncrementCustomersLeft;
        OnMoneyChanged -= ChangeMoney;
    }
    protected override void OnApplicationQuit() 
    {
        base.OnApplicationQuit();
        Reset();
    }


    public void ChangePhase(GamePhase phase)
    {
        if (phase == CurrentPhase) 
        {
            Debug.LogError("You can't go to the same phase again!");
            return;
        }

        switch (phase)
        {
            case GamePhase.PRE_PRE_SERVICE:
                DoPrePreService();
                break;
            
            case GamePhase.PRE_SERVICE:
                DoPreSerice();
                break;

            case GamePhase.SERVICE:
                DoService();
                break; 

            case GamePhase.POST_SERVICE:
                DoPostService();
                break; 

            default:
                Debug.LogError("Invalid state chosen");
                break;
        }

        CurrentPhase = phase;
        OnPhaseChanged?.Invoke();
    }


#endregion

#region Phase_Methods

    void DoPrePreService()
    {
        // view the different equipment 
    }
    void DoPreSerice()
    {
        // buying of ingredients
        // rice cooking preparation
    }
    void DoService()
    {
        OnStartGame?.Invoke();

        // customers spawn in 
        // cooking, serving, and serving
    }
    void DoPostService()
    {
        // shop closes and you get the rating for the day
        // finish washing the dishes
    }

#endregion


    void test() {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            float rndm = UnityEngine.Random.Range(80f, 100f);
            _foodScores.Add(rndm);

            Debug.Log($"Score: {rndm}; Count: {_foodScores.Count}");
        }        

        if (Input.GetKeyDown(KeyCode.Space))
            throw new NullReferenceException("test");
            //Debug.Log($"Total Score: {GetAverageOf(_foodScores)}");
    }  

#region Rating_Methods

    void EndOfDayCalculations()
    {
        /* UI CODE 
            - shows the total customers served
            - shows the amt of customers that left
            - shows how much money you gained
        */ 

        // restaurant rating 
        float finalScore = (CleaningManager.Instance.KitchenScore + 
                            GetAverageOf(_foodScores) + 
                            GetAverageOf(_customerSRScores)) / 3f;

        Debug.Log(finalScore);

        // add code to show a lettered score based on finalScore;
    }
    float GetAverageOf(List<float> list) 
    {
        // prevents a div/0 case
        if (list.Count < 1) return 0f;

        float n = 0f;

        for (int i = 0; i < list.Count; i++) 
            n += list[i];

        return n / _foodScores.Count;
    }

#endregion

#region Event_Methods 

    // other GameObjects call the GameManager's events to trigger these methods

    void AddToFoodScore(float foodScore) => _foodScores.Add(foodScore);
    void AddToCustomerSRScore(float srScore) => _customerSRScores.Add(srScore);
    void IncrementCustomersLeft() => _customersThatLeftCounter++;
    void ChangeMoney(float amt) 
    {
        AvailableMoney += amt;

        if (AvailableMoney < 0f)
        {
            AvailableMoney = 0f;
            Debug.LogWarning("You have no more money!");
        }
    }

#endregion

}