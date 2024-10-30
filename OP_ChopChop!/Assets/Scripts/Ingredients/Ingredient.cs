using System.Collections;
using UnityEngine;
using System;

public class Ingredient : MonoBehaviour 
{
    public Action OnRateChanged; // ED for UI changes later on

    public IngredientStats Stats => _stats;

    [SerializeField] GameObject[] _prefabs; // different stages of the ingredient
    [SerializeField] IngredientStats _stats;

    void Start()
    {
        name = _stats.name;
        CheckRate();
        StartCoroutine(Decay());
    }

    public void ToggleProperStorage() 
    { 
        _stats.IsProperlyStored = !_stats.IsProperlyStored; 
    }
    public void ThrewInTheTrash() 
    {
        _stats.IsTrashed = true;
        _stats.FreshnessRate = 0;
        CheckRate();
    }
    void CheckRate() 
    {
        if      (_stats.FreshnessRate < 70) _stats.Rating = FreshnessRating.EXPIRED;
        else if (_stats.FreshnessRate > 87) _stats.Rating = FreshnessRating.FRESH;
        else                                _stats.Rating = FreshnessRating.LESS_FRESH;

        OnRateChanged?.Invoke(); 
        // Debug.Log(_stats.Rating);
    }

    
    IEnumerator Decay() 
    {
        while (!_stats.HasExpired) 
        {
            int rate, speed;
            if (_stats.IsContaminated) 
            {
                rate = _stats.Contaminated.Rate;
                speed =  _stats.Contaminated.Speed;
            }
            else if (_stats.IsProperlyStored) 
            {
                rate = _stats.Stored.Rate;
                speed = _stats.Stored.Speed;
            }
            else
            {
                rate = _stats.Decay.Rate;
                speed = _stats.Decay.Speed;
            }
            Debug.Log($"Rate: {rate}; Speed: {speed}");


            yield return new WaitForSecondsRealtime(speed);
            _stats.FreshnessRate -= rate;
            Debug.Log($"Freshness of {name} has been reduced to {_stats.FreshnessRate}");

            if (_stats.FreshnessRate < 1) 
            {
                _stats.FreshnessRate = 0;
                _stats.HasExpired = true;
            }
            
            CheckRate();
        }

        // Debug.Log($"{name} has expired!");
        Destroy(gameObject); // test
        yield break;
    }
}

