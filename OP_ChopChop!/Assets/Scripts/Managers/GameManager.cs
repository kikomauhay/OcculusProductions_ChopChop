using UnityEngine;
using System;

/// <summary>
///
/// Acts as the head of the game.
/// Also rates the restaurant once it reaches the end of the day.
///
/// </summary>


public class GameManager : Singleton<GameManager>
{

#region Events
    public Action OnCustomerSpawned;
    public Action OnFoodDisposed, OnFoodServed;

#endregion

#region Members 

    // Player References
    [SerializeField] GameObject _player;
    public GameObject Player => _player; 

    int _customerCount, _foodServed;


#endregion

#region Methods

    protected override void Awake() => base.Awake(); 
    protected override void OnApplicationQuit() => base.OnApplicationQuit();   

#endregion

#region Rating_Calculations

/// <summary>
/// 
/// 1. Calculate the Food Score, then add it to the foodCounter
/// 2. Get Customer SR
/// 
/// </summary>

    float CalculateFoodScore(Ingredient ing1, Ingredient ing2) 
    {
        // once the dish is made, this gets both the freshness rates of the ingredients
        // this is only made for nigiris atm, idk if the maki has more than 2 ingredients
        
        return (ing1.FreshnessRate + ing2.FreshnessRate) / 2f;
    }

    float CalculateCustomerSR(float rawCustomerSR, float timeToFinish) 
    {

        return 0f;
    }

    float CalculateRestartantRating(float avgCustomerSR, float avgFoodScore) // this only triggers once it's the shift ends 
    {
    /// <summary> rating formula issues
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
    
        return (avgCustomerSR + avgFoodScore) / 2f;
    }
    
#endregion

#region Event_Methods 

    // GameObjects don't call the GameManager's things, just calls the events for it



#endregion

}