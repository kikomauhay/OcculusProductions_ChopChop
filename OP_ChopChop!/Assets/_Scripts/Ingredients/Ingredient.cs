using System.Collections;
using UnityEngine;

/// <summary>
/// 
/// Acts as the base class for the different ingredients
/// All children of Ingredent.cs only needs the stats before being destroyed
/// 
/// </summary>

public enum IngredientType { RICE, TUNA, SALMON, SEAWEED } // will expand later
public enum IngredientState { DEFAULT, EXPIRED, CONTAMINATED, TRASHED, STORED }

public abstract class Ingredient : MonoBehaviour
{
#region Members

    [Header("Ingredient Components"), Tooltip("Anything the ingredient needs.")]
    [SerializeField] protected IngredientStats _ingredientStats;
    [SerializeField] protected IngredientType _ingredientType; // will be used by the child classes

    public IngredientStats IngredientStats => _ingredientStats;
    public IngredientType IngredientType => _ingredientType;

    public IngredientState IngredientState { get; protected set; }
    public TrashableType TrashableType { get; private set; }
    public FreshnessRating Rating { get; private set; }
    public float FreshnessRate { get; private set; }
    public bool IsProperlyStored { get; set; } // is changed outside the script

#endregion

#region Members

    protected virtual void Start() 
    {
        IngredientState = IngredientState.DEFAULT; // changes inside this script
        TrashableType = TrashableType.INGREDIENT;  // won't change at all
        Rating = FreshnessRating.FRESH;            // changes inside the enumerator
        
        FreshnessRate = 100; // the higher the score, the better

        CheckRate();
        StartCoroutine(Decay());
    }
    
    public void ThrowInTrash() // idk if we need this
    {
        // removes the food from the game entirely
        // could add more punishment later on 

        IngredientState = IngredientState.TRASHED;
        FreshnessRate = 0f;
        SoundManager.Instance.PlaySound("dispose food");
        CheckRate();
    }
    protected void CheckRate() 
    {
        // material of the ingredient changes based on the freshness rate
        // the lower the number, the worse it is

        Material m;

        if (FreshnessRate < 70f) 
        {
            Rating = FreshnessRating.EXPIRED;
            m = _ingredientStats.Materials[2];    
        }
        else if (FreshnessRate > 87f) 
        {
            Rating = FreshnessRating.FRESH;
            m = _ingredientStats.Materials[0];
        }
        else
        {
            Rating = FreshnessRating.LESS_FRESH;
            m = _ingredientStats.Materials[1];
        }

        if (m != null)
            GetComponent<MeshRenderer>().material = m;
    }

    public void ContaminateFood()
    {
        Debug.LogWarning($"{name} has been contaminated!");
        IngredientState = IngredientState.CONTAMINATED;
    }

#endregion

    protected IEnumerator Decay() 
    {        
        while (IngredientState != IngredientState.EXPIRED)
        {
            // rate & speed will changes depending on the IngredientState
            int rate = 0, speed = 0; 

            switch (this.IngredientState) 
            {
                case IngredientState.CONTAMINATED:
                    rate = _ingredientStats.Contaminated.Rate;
                    speed = _ingredientStats.Contaminated.Speed;
                    break;

                case IngredientState.STORED:
                    rate = _ingredientStats.Stored.Rate;
                    speed = _ingredientStats.Stored.Speed;
                    break;
                
                case IngredientState.DEFAULT: // just outside the fridge AND not contaminated
                    rate = _ingredientStats.Decay.Rate;
                    speed = _ingredientStats.Decay.Speed;
                    break;
                
                case IngredientState.EXPIRED: break;
                default:                      break;
            }
            
            yield return new WaitForSeconds(speed);
            FreshnessRate -= rate;

            // test
            Debug.Log($"Freshness of {name} has been reduced to {FreshnessRate}");

            if (FreshnessRate < 1f) 
            {
                FreshnessRate = 0f;
                IngredientState = IngredientState.EXPIRED;
            }
            CheckRate();
        }
        Destroy(gameObject); // test, remove this once it's properly set up
    }
}

