using System.Collections;
using UnityEngine;

public class Ingredient : MonoBehaviour 
{
#region Members

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
    public void ThrowInTrash() 
    {
        // removes the food form the game entirely
        // could add more punishment later on 

        IsTrashed = true;
        FreshnessRate = 0;
        SoundManager.Instance.PlaySound("dispose food");
        CheckRate();
    }
    void CheckRate() 
    {
        // material of the ingredient changes based on how fresh it is
        // the lower the number, the worse it is

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
            // rate & speed will change depending on the state of the ingredient

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
            else // just outside the fridge and not contaminated
            {
                rate = _stats.Decay.Rate;
                speed = _stats.Decay.Speed;
            }
            Debug.Log($"Rate: {rate}; Speed: {speed}");


            yield return new WaitForSecondsRealtime(speed);
            FreshnessRate -= rate;

            // test
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

