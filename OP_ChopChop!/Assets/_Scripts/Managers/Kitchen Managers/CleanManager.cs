using System.Collections;
using UnityEngine;
using System;

public class CleanManager : Singleton<CleanManager>
{
    //Standardized script for activating colliders on the hand, will draft it up and will ask help from isagani to clean up the code later
    public Action OnCleanedArea, OnStartDecayAgain;
    public float KitchenScore { get; private set; } // overall cleanliness meter of the kitchen

    [SerializeField] Collider[] _handWashColliders, _kitchenWashColliders;
    float _decayTimer, _decayRate, _cleanlinessThreshold; 
    bool _canClean;


#region Unity_Methods

    protected override void Awake() => base.Awake();
    protected override void OnApplicationQuit() 
    {
        base.OnApplicationQuit();
        Reset();
    }
    void Reset() => OnCleanedArea -= IncreaseCleanRate;   
    
    void Start()
    {
        KitchenScore = 100;
        _decayTimer = 5f;
        _decayRate = 5f;
        _cleanlinessThreshold = 90f; // kitchen needs to go below this score to start cleaning 
        _canClean = false;           // prevents the player from cleaning too much

        OnCleanedArea += IncreaseCleanRate;

        StartCoroutine(DecayKitchen());
    }
    void Update() => test();

#endregion

    void test()
    {
        if (Input.GetKeyDown(KeyCode.Space)) 
        {
            ToggleHandWashColliders();
            ToggleKitchenColliders();
        }
        
        if (Input.GetKeyDown(KeyCode.Escape))
            IncreaseCleanRate();

        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            _canClean = !_canClean;
            Debug.Log($"Can clean: {_canClean}");
        }
    }   


#region Private_Functions
    void ToggleKitchenColliders()
    {
        foreach (Collider col in _kitchenWashColliders)
            col.enabled = !col.enabled;
        //Add vfx that spawns on trigger area
    }
#endregion

#region Public_Functions

    public void ToggleHandWashColliders()
    {
        foreach (Collider col in _handWashColliders)
            col.enabled = !col.enabled;
    }
    public void IncreaseCleanRate() 
    {
        if (!_canClean) // no need to focus too much on cleaning
        {
            Debug.LogError("You can't clean yet!");
            return;
        }

        // enables the decay mechanic to start again
        if (KitchenScore < 1)
            StartCoroutine(DecayKitchen());
        
        KitchenScore += UnityEngine.Random.Range(5, 10); // test value for now

        if (KitchenScore > 100)
            KitchenScore = 100;
    }

#endregion


    IEnumerator DecayKitchen()
    {
        yield return new WaitForSeconds(3f);

        Debug.LogWarning("Kitchen is getting dirty!");
        
        while (KitchenScore > 0) 
        {
            yield return new WaitForSeconds(_decayTimer);
            KitchenScore -= _decayRate;

            // no need to focus too much on cleaning
            if (KitchenScore < _cleanlinessThreshold && !_canClean)
            {
                _canClean = true;
                ToggleKitchenColliders();
                Debug.Log("All colliders should be enabled");
            }
            else if (KitchenScore > _cleanlinessThreshold) 
            {
                _canClean = false;
                ToggleKitchenColliders();
                Debug.Log("All colliders should be disabled");
            }

            Debug.LogWarning($"Can clean is {_canClean}");
            Debug.Log(KitchenScore); // test
        }
    }
}
