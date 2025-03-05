using UnityEngine;
using System;

[CreateAssetMenu(menuName = "Ingredient")]
public class IngredientStats : ScriptableObject
{   
    public StorageType StorageType => _type;
    public Material[] Materials => _stateMaterials; // material for good, bad, expired

    // DECAY VARIABLES
    public readonly float DecaySpeed = 4f;
    public readonly float StoredRate  = 0.8f; 
    public readonly float NormalRate = 2f; 
    public readonly float ContaminatedRate = 25f; 

#region Private

    [SerializeField] StorageType _type;
    
    [SerializeField, Tooltip("0 = good, 1 = expired, 2 = contaminated")] 
    Material[] _stateMaterials;
    
#endregion
}


[Serializable]
public struct Timer { public int Rate, Speed; }