using UnityEngine;
using System;

/// <summary>

/// Acts as the head of the game.
/// Also rates the restaurant quality once the day is done.

/// </summary>


public class GameManager : Singleton<GameManager>
{

#region Events
    public Action OnCustomerSpawned, OnFoodThrown, OnFoodServed;

#endregion

#region Members 

    // Player References
    [SerializeField] GameObject _player;
    public GameObject Player => _player; 

    // Rating System
    public int CleanlinessRate { get; private set; } // the dirtier it gets, the less the value it wil be
    public int DisposeCount { get; private set; } // when anything is thrown, this value increases
    private float _avgCustomerSatisfactionRating;
    private float _avgFoodScore; 


    // Counters for average rates
    public int CustomerCounter { get; private set; }
    public int FoodServedCounter { get; private set; }

#endregion

#region Methods

    protected override void Awake() => base.Awake(); 
    protected override void OnApplicationQuit() 
    {
        base.OnApplicationQuit(); 
        Reset();
    }
    void Start() 
    {
        CleanlinessRate = 100;
        FoodServedCounter = 0;
        CustomerCounter = 0;
        DisposeCount = 0;

        _avgCustomerSatisfactionRating = 0f;
        _avgFoodScore = 0f;

        OnCustomerSpawned += AddCustomerCount;
        OnFoodThrown += AddDisposedFood;
        OnFoodServed += AddFoodServedCount;
    }
    void Reset() // unsubsribing from events to prevent null referencing 
    {
        OnCustomerSpawned -= AddCustomerCount;
        OnFoodThrown -= AddDisposedFood;
        OnFoodServed -= AddFoodServedCount;
    }

    void RateRestaurant() 
    {
    /// <summary> rating formula issues
    /// 
    /// so far, we're using a simple formula
    /// there are a lot of variables that we need to look into
    /// 
    /// </summary>
    



        // restaurant rate = avg (CleanlinessRate + 
        //                        Avg of Customers' Satisfaction Rating + 
        //                        Food Score of All dishes served - 
        //                        DisposeRate)
    }
    
#endregion

#region Event_Methods 

    // GameObjects don't call the GameManager's things, just calls the events for it

    void AddCustomerCount() => CustomerCounter++;
    void AddFoodServedCount() => FoodServedCounter++;
    void AddDisposedFood() => DisposeCount++;
    


#endregion

}