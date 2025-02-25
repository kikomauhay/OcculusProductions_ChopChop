using UnityEngine;
using System;

[CreateAssetMenu(menuName = "Ingredient")]
public class IngredientStats : ScriptableObject
{   
    public StorageType StorageType => _type;
    public Material[] Materials => _stateMaterials; // material for good, bad, expired
            
    public Timer Decay => _decay; // default rate
    public Timer Contaminated => _contaminated;
    public Timer Stored => _stored;

#region Private
    [SerializeField] StorageType _type;
    [SerializeField] Material[] _stateMaterials;

    [Header("Timers")]
    [SerializeField] Timer _decay;
    [SerializeField] Timer _contaminated, _stored;
    
#endregion
}


[Serializable]
public struct Timer { public int Rate, Speed; }