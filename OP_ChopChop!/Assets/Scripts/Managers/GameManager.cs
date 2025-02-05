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
    public Action OnFoodDisposed;
    public Action<float, float> OnDishCreated;

#endregion

#region Members 

    // Player References
    [SerializeField] GameObject _player;
    public GameObject Player => _player; 

    int _foodServed;

    float _totalFoodScore, _totalCustomerSR;


#endregion

#region Methods

    protected override void Awake() => base.Awake(); 
    protected override void OnApplicationQuit() => base.OnApplicationQuit();

    private void Start()
    {
        _totalFoodScore = 0f;
        _totalCustomerSR = 0f;
    }

    #endregion

    #region Rating_Calculations

    /// <summary>
    /// 
    /// 1. Calculate the Food Score, then add it to the foodCounter
    /// 2. Get Customer SR
    /// 
    /// </summary>


    void AddFoodScore(float ing1Score, float ing2Score) => _totalFoodScore += (ing1Score  + ing2Score) / 2f;
    void AddCustomerSR(float rawCustomerSR, float timeToFinish) => _totalCustomerSR += (rawCustomerSR + timeToFinish) / 2f;


    float CalculateTotalFoodScore(float foodScore)
    {
        if (_foodServed == 0)
        {
            Debug.LogError("No food was served!");
            return 0f;
        }
        return foodScore / _foodServed;
    }

    float CalculateTotalCustomerSR(float customerSR) 
    {
        if (_foodServed == 0)
        {
            Debug.LogError("No customer was served!");
            return 0f;
        }
        return customerSR / _foodServed;
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