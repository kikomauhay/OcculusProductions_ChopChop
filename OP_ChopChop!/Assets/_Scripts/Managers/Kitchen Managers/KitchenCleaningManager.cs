using System.Collections;
using UnityEngine;
using System;

public class KitchenCleaningManager : Singleton<KitchenCleaningManager>
{
    // Standardized script for activating colliders on the hand, will draft it up and will ask help from isagani to clean up the code later
    public Action OnCleanedArea, OnStartDecayAgain;
    public float KitchenScore { get; private set; } // overall cleanliness meter of the kitchen

    [SerializeField] Collider[] _handWashColliders; 
    [SerializeField] GameObject[] _kitchenWashColliders;
    float _decayTimer, _decayRate, _cleanlinessThreshold; 
    bool _canClean;


#region Unity_Methods

    protected override void Awake() => base.Awake();
    protected override void OnApplicationQuit() 
    {
        base.OnApplicationQuit();
        Reset();
    }
    void Start()
    {
        OnCleanedArea += IncreaseCleanRate;
        GameManager.Instance.OnStartService += StartKitchenDecay;
        GameManager.Instance.OnEndService += StopAllCoroutines;

        KitchenScore = 100f;
        _decayTimer = 5f;
        _decayRate = 5f;
        _cleanlinessThreshold = 80f; // kitchen needs to go below this score to start cleaning 
        _canClean = false;           // prevents the player from cleaning too much

        // ToggleAllColliders();
    }
    void Reset() 
    {
        OnCleanedArea -= IncreaseCleanRate;   
        GameManager.Instance.OnStartService -= StartKitchenDecay;
        GameManager.Instance.OnEndService -= StopAllCoroutines;
    }

#endregion

#region Private_Functions

    void StartKitchenDecay() => StartCoroutine(DecayKitchen());
    void ToggleKitchenColliders()
    {
        foreach (GameObject gameObject in _kitchenWashColliders)
            gameObject.SetActive(!gameObject.activeSelf);
    }
    void ToggleHandWashColliders()
    {
        foreach (Collider col in _handWashColliders)
            col.enabled = !col.enabled;
    }

#endregion

    void IncreaseCleanRate() 
    {
        if (!_canClean) // no need to focus too much on cleaning
        {
            Debug.LogWarning("You can't clean yet!");
            return;
        }

        // enables the decay mechanic to start again
        if (KitchenScore < 1)
            StartCoroutine(DecayKitchen());
        
        KitchenScore += UnityEngine.Random.Range(5, 10); // test value for now

        if (KitchenScore > 100)
            KitchenScore = 100;
    }

    IEnumerator DecayKitchen()
    {
        KitchenScore = 100;

        yield return new WaitForSeconds(10f);
                
        while (KitchenScore > 0) 
        {
            yield return new WaitForSeconds(_decayTimer);
            KitchenScore -= _decayRate;

            if (KitchenScore < 70F && !_canClean)
            {
                _canClean = true;
                ToggleKitchenColliders();
            }
            else if (KitchenScore > _cleanlinessThreshold) 
            {
                _canClean = false;
                ToggleKitchenColliders();
            }
        }
    }
}
