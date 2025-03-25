using System.Collections;
using UnityEngine;
using System;

/// <summary>
/// 
/// Acts as the base class for the different ingredients
/// All children of Ingredent.cs only needs the stats before being destroyed
/// 
/// </summary>

[RequireComponent(typeof(Trashable))]
public abstract class Ingredient : MonoBehaviour
{
#region Readers

    public IngredientStats IngredientStats => _ingredientStats;
    public IngredientType IngredientType => _ingredientType;

    // INGREDIENT ATTRIBUTES
    public IngredientState IngredientState { get; private set; } // changes inside this script
    public float FreshnessRate { get; private set; } // the higher the score, the better
    public bool IsFresh { get; private set; }        // changes inside the enumerator

#endregion

#region Members

    public Action OnTrashed;

    [Header("Ingredient Components")]
    [SerializeField] protected IngredientType _ingredientType; // will be used by the child classes
    [SerializeField] protected IngredientStats _ingredientStats;

    [Tooltip("0 = good, 1 = comtaminated, 2 = expired")]
    [SerializeField] protected Material[] _stateMaterials;

    protected Vector3 _startPosition;

#endregion

#region Unity_Methods

    protected virtual void Start() 
    {
        // ingredients will only decay once the shift has started  
        GameManager.Instance.OnStartService += StartDecaying;
        GameManager.Instance.OnEndService += Expire;

        IngredientState = IngredientState.DEFAULT;   

        FreshnessRate = 100f;     
        IsFresh = true;
        _startPosition = transform.position;

        ChangeMaterial();

        // in case some ingredients are spawned during Service time
        if (GameManager.Instance.CurrentShift == GameShift.SERVICE)
            StartDecaying();        
    }
    protected virtual void Reset() 
    {
        GameManager.Instance.OnStartService -= StartDecaying;
        GameManager.Instance.OnEndService -= Expire;
        Reposition();
    }

#endregion

#region Ingredint_Methods

    public void Trashed()
    {
        OnTrashed?.Invoke();

        IngredientState = IngredientState.CONTAMINATED;
        IsFresh = false;
        FreshnessRate = 0f;
        SoundManager.Instance.PlaySound("dispose food", SoundGroup.FOOD);
        ChangeMaterial();
    }
    public void Expire()
    {
        IngredientState = IngredientState.EXPIRED;
        IsFresh = false;

        ChangeMaterial();
    } 
    public void Contaminate()
    {
        IngredientState = IngredientState.CONTAMINATED;
        IsFresh = false;
        ChangeMaterial();
        Reposition();
    }
    protected void ChangeMaterial() 
    {
        // the material of the ingredient changes based on the freshness rate
        // the lower the number, the worse it is

        Material m = null;

        switch (this.IngredientState)
        {
            case IngredientState.CONTAMINATED:
                m = _stateMaterials[1];
                break;

            case IngredientState.EXPIRED:
                m = _stateMaterials[2];
                break;

            case IngredientState.DEFAULT:
                m = _stateMaterials[0];
                break;

            default: break;
        }

        if (m != null)
            GetComponent<MeshRenderer>().material = m;
    }
    void StartDecaying() => StartCoroutine(DecayIngredient());
    public void Reposition() => transform.position = _startPosition;

#endregion

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
                ChangeMaterial();
            }
        }
    }

#endregion
}