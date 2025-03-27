using UnityEngine;

[CreateAssetMenu(menuName = "Ingredient")]
public class IngredientStats : ScriptableObject
{   
    public readonly float DecaySpeed = 4f;
    public readonly float StoredRate  = 0.8f; 
    public readonly float NormalRate = 2f; 
    public readonly float ContaminatedRate = 25f;
}