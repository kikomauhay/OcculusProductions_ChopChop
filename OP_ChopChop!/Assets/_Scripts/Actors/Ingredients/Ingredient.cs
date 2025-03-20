using System.Collections;
using UnityEngine;
using System;

/// <summary>
/// 
/// Acts as the base class for the different ingredients
/// All children of Ingredent.cs only needs the stats before being destroyed
/// 
/// </summary>

public abstract class Ingredient : MonoBehaviour
{
#region Readers

    public IngredientStats IngredientStats => _ingredientStats;
    public IngredientType IngredientType => _ingredientType;

    // INGREDIENT ATTRIBUTES
    public IngredientState IngredientState { get; private set; } // changes inside this script
    public float FreshnessRate { get; private set; } // the higher the score, the better
    public bool IsProperlyStored { get; set; }       // is changed outside the script
    public bool IsFresh { get; private set; }        // changes inside the enumerator

#endregion

#region Members

    [Header("Ingredient Components")]
    [SerializeField] protected IngredientType _ingredientType; // will be used by the child classes
    [SerializeField] protected StateMaterials _stateMaterials;
    [SerializeField] protected IngredientStats _ingredientStats;

    protected Vector3 _startPosition;

#endregion

    protected virtual void Start() 
    {
        // ingredients will only decay once the shift has started  
        GameManager.Instance.OnStartService += StartDecaying;
        GameManager.Instance.OnEndService += ExpireIngredient;

        IngredientState = IngredientState.DEFAULT;   

        FreshnessRate = 100f;     
        IsFresh = true;           
        IsProperlyStored = false; 
        _startPosition = transform.position;

        ChangeMaterial();

        // in case some ingredients are spawned during Service time
        if (GameManager.Instance.CurrentShift == GameShift.SERVICE)
            StartDecaying();        
    }
    protected virtual void Reset() 
    {
        GameManager.Instance.OnStartService -= StartDecaying;
        GameManager.Instance.OnEndService -= ExpireIngredient;
        Reposition();
    }

#region State_Actions

    public void TrashIngredient() // idk if we need this
    {
        // removes the food from the game entirely
        // could add more punishment later on 

        IngredientState = IngredientState.CONTAMINATED;
        IsFresh = false;
        FreshnessRate = 0f;
        SoundManager.Instance.PlaySound("dispose food");
        ChangeMaterial();

        Debug.LogWarning($"{name} has been trashed!");


        // add Destroy() ??
    }
    public void ExpireIngredient()
    {
        IngredientState = IngredientState.EXPIRED;
        IsFresh = false;

        ChangeMaterial();
        
        Debug.LogWarning($"{name} has expired!");
    } 
    public void ContaminateIngredient()
    {
        IngredientState = IngredientState.CONTAMINATED;
        IsFresh = false;
        ChangeMaterial();
        Reposition();
        
        Debug.LogWarning($"{name} has been contaminated!");
    }

#endregion

    protected void ChangeMaterial() 
    {
        // the material of the ingredient changes based on the freshness rate
        // the lower the number, the worse it is

        Material m = null;

        switch (this.IngredientState)
        {
            case IngredientState.CONTAMINATED:
                m = _ingredientStats.Materials[2];
                break;

            case IngredientState.EXPIRED:
                m = _ingredientStats.Materials[1];
                break;

            case IngredientState.DEFAULT:
                m = _ingredientStats.Materials[0];
                break;

            default: break;
        }

        if (m != null)
            GetComponent<MeshRenderer>().material = m;
    }

    void StartDecaying() => StartCoroutine(DecayIngredient());
    public void Reposition() => transform.position = _startPosition;

#region Enumerators

    protected IEnumerator DecayIngredient() 
    {
        // grace period before actual decaying starts
        yield return new WaitForSeconds(10f);
        
        while (FreshnessRate > 0f)
        {
            yield return new WaitForSeconds(_ingredientStats.DecaySpeed);

            // reduces freshness rate based on the ingredient's state
            switch (IngredientState)
            {
                case IngredientState.DEFAULT: 
                    FreshnessRate = _ingredientStats.NormalRate;
                    break;

                case IngredientState.STORED:
                    FreshnessRate = _ingredientStats.StoredRate;
                    break;

                case IngredientState.CONTAMINATED:
                    FreshnessRate = _ingredientStats.ContaminatedRate;
                    break;

                default: break;
            }

            if (FreshnessRate < 1)
            {
                FreshnessRate = 0f;
                IngredientState = IngredientState.EXPIRED;
            }

            ChangeMaterial();
        }
    }

    #endregion

}

[Serializable]
public struct StateMaterials
{
    public Material[] ExpiredMats => _expiredMaterials;
    public Material[] FreshMats => _freshMaterials;
    public Material[] ContaminatedMats => _contaminatedMaterials;

    [Tooltip("0 = thick cut, 1 = thick strip, 2 = thin slice")]
    [SerializeField] Material[] _expiredMaterials, _freshMaterials, _contaminatedMaterials;
}