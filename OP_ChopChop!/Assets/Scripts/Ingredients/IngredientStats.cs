using UnityEngine;
using System;

public enum FreshnessRating { FRESH, LESS_FRESH, EXPIRED }

[CreateAssetMenu(menuName = "Ingredient")]
public class IngredientStats : ScriptableObject
{   
    public StorageType StorageType => _type;
    public Material[] Materials => _materials;
            
    public Timer Decay => _decay; // default rate
    public Timer Contaminated => _contaminated;
    public Timer Stored => _stored;

#region Private
    [SerializeField] StorageType _type;
    [SerializeField] Material[] _materials;

    [Header("Timers")]
    [SerializeField] Timer _decay;
    [SerializeField] Timer _contaminated, _stored;
    
#endregion
}


[Serializable]
public struct Timer { public int Rate, Speed; }