using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary> -WHAT DOES THIS SCRIPT DO-
///
/// Acts as the brains of the game.
/// Stores all the scores of the food served and customerSR.
/// Rates the Restaurant Quality once the shift ends.
///
/// </summary>

public class GameManager : Singleton<GameManager>
{

#region Members

    // Events
    public Action OnFoodDisposed;
    public Action<float> OnCustomerServed, OnCustomerScored;

    // Player references
    [SerializeField] GameObject _player;
    public GameObject Player => _player;

    // Scores to keep track of
    List<float> _foodScores, _customerSRScores;

#endregion

#region Methods

    protected override void Awake() => base.Awake();
    protected override void OnApplicationQuit() 
    {
        base.OnApplicationQuit();
        Reset();
    }
    void Reset()
    {
        OnCustomerServed -= AddToFoodScore;
        OnCustomerScored -= AddToCustomerSRScore;
    }

    void Start()
    {
        _foodScores = new List<float>();
        _customerSRScores = new List<float>();

        OnCustomerServed += AddToFoodScore;
        OnCustomerScored += AddToCustomerSRScore;
    }

    void Update() => test();

    void test() {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            float rnd = UnityEngine.Random.Range(80f, 100f);
            _foodScores.Add(rnd);

            Debug.Log($"Score: {rnd}; Count: {_foodScores.Count}");
        }        

        if (Input.GetKeyDown(KeyCode.Space))
            Debug.Log($"Total Score: {GetAverageOf(_foodScores)}");
    }  
#endregion

#region Rating_Calculations

    /// </summary> -RATING PROCESS-
    /// 
    /// 1. Calculate the Food Score, then add it to the proper list
    /// 2. Calculate Customer SR, then add it to the proper list
    /// 3. At the end of the day, calculate the restaurant
    /// 
    /// </summary>

    float GetAverageOf(List<float> list) 
    {
        // prevents a div/0 case
        if (list.Count < 1) return 0f;

        float n = 0f;

        for (int i = 0; i < list.Count; i++) 
            n += list[i];

        return n / _foodScores.Count;
    }

    float CalculateRestartantRating(float avgCustomerSR, float avgFoodScore) // this only triggers once it's the shift ends 
    {
    ///<summary> -RATING FORMULA ISSUES-
    /// 
    /// so far, we're using a simple formula
    /// there are a lot of variables that we need to look into
    /// so for now, we won't cover the waste counter & cleanliness of the resto
    /// 
    /// This is an idea for the final calculations, but some of it hasn't been implemented yet
    /// restaurant rate = avg (CleanlinessRate + 
    ///                        Avg of Customers' Satisfaction Rating + 
    ///                        Food Score of All dishes served - 
    ///                        DisposeRate)
    /// </summary>
    
        return (avgCustomerSR + avgFoodScore) / 2f; // will make this more complex after midterms
    }

#endregion

#region Event_Methods 

    // other GameObjects don't call the GameManager's methods, the just call the events for it

    void AddToFoodScore(float foodScore) => _foodScores.Add(foodScore);
    void AddToCustomerSRScore(float srScore) => _customerSRScores.Add(srScore);

#endregion
}