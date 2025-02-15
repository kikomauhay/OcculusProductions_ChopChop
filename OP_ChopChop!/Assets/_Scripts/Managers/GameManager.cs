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

    // Events
    public Action OnFoodDisposed, OnCustomerLeft;
    public Action<float> OnCustomerServed;

    // Player references
    [SerializeField] GameObject _player;
    public GameObject Player => _player;

    // Scores to keep track of
    List<float> _foodScores, _customerSRScores;

    int _customersLeftCounter;

#endregion

#region Unity_Methods

    protected override void Awake() => base.Awake();
    protected override void OnApplicationQuit() 
    {
        base.OnApplicationQuit();
        Reset();
    }
    void Reset()
    {
        OnCustomerServed -= AddToFoodScore;
        OnCustomerLeft -= IncrementCustomersLeft;
    }

    void Start()
    {
        _foodScores = new List<float>();
        _customerSRScores = new List<float>();

        OnCustomerServed += AddToFoodScore;
        OnCustomerLeft += IncrementCustomersLeft;
    }
    void Update() => test();

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

    void EndOfDayCalculations()
    {
        /* UI CODE 
            - shows the total customers served
            - shows the amt of customers that left
            - shows how much money you gained
        */ 

        // restaurant rating 
        float finalScore = (CleanManager.Instance.KitchenScore + 
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

#region Event_Methods 

    // other GameObjects don't call the GameManager's methods, the just call the events for it

    void AddToFoodScore(float foodScore) => _foodScores.Add(foodScore);
    void AddToCustomerSRScore(float srScore) => _customerSRScores.Add(srScore);
    void IncrementCustomersLeft() => _customersLeftCounter++;

#endregion
}