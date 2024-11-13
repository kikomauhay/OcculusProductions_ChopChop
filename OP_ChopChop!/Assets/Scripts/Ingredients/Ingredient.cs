using System.Collections;
using UnityEngine;

public class Ingredient : MonoBehaviour 
{
#region Getters
    public FreshnessRating Rating { get; private set; }
    public IngredientStats Stats => _stats;
    public int FreshnessRate { get; private set; }
    public bool IsExpired { get; private set; } 
    public bool IsContaminated { get; private set; }
    public bool IsTrashed { get; private set; }
    public bool IsProperlyStored { get; set; }
#endregion
    [SerializeField] IngredientStats _stats;

    void Start()
    {
        name = _stats.name;
        FreshnessRate = 100;
        Rating = FreshnessRating.FRESH;

        IsTrashed = false;
        IsExpired = false;
        IsContaminated = false;
        IsProperlyStored = false;
        
        CheckRate();
        StartCoroutine(Decay());
    }
    public void ThrewInTheTrash() 
    {
        IsTrashed = true;
        FreshnessRate = 0;
        CheckRate();
    }
    void CheckRate() 
    {
        Material m;

        if (FreshnessRate < 70) 
        {
            Rating = FreshnessRating.EXPIRED;
            m = _stats.Materials[2];    
        }
        else if (FreshnessRate > 87) 
        {
            Rating = FreshnessRating.FRESH;
            m = _stats.Materials[0];
        }
        else
        {
            Rating = FreshnessRating.LESS_FRESH;
            m = _stats.Materials[1];
        }

        if (m != null)
            GetComponent<MeshRenderer>().material = m;
    }

#region Enumerators
    IEnumerator Decay() 
    {
        while (!IsExpired) 
        {
            int rate, speed;

            if (IsContaminated) 
            {
                rate = _stats.Contaminated.Rate;
                speed =  _stats.Contaminated.Speed;
            }
            else if (IsProperlyStored) 
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
            FreshnessRate -= rate;
            Debug.Log($"Freshness of {name} has been reduced to {FreshnessRate}");

            if (FreshnessRate < 1) 
            {
                FreshnessRate = 0;
                IsExpired = true;
            }
            CheckRate();
        }
        Destroy(gameObject); // test
    }
#endregion
}

