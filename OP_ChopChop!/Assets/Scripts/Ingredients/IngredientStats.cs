using UnityEngine;
using System;

[CreateAssetMenu(menuName = "Ingredient")]
public class IngredientStats : ScriptableObject
{   
    public FreshnessRating Rating;
    public int FreshnessRate = 100;

    [Header("Timers")]
    public Timer Decay;
    public Timer Contaminated, Stored;

    public bool HasExpired, IsContaminated, IsProperlyStored;
}

public enum FreshnessRating { FRESH, LESS_FRESH, EXPIRED }

[Serializable]
public struct Timer { public int Rate, Speed; }