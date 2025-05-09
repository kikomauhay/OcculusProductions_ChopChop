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
#region Members

#region Properties

    public IngredientState IngredientState { get; private set; }
    public IngredientType IngredientType => _ingredientType;
    public float FreshnessRate => _freshnessRate;
    public int SliceIndex => _sliceIndex;
    public bool IsFresh => _isFresh;

#endregion
#region SerializeField

    [Header("Ingredient Attributes")]
    [SerializeField, Range(0f, 100f)] protected float _freshnessRate;
    [SerializeField] protected IngredientType _ingredientType; // will be used by the child classes
    [SerializeField] protected int _sliceIndex;
    [SerializeField] protected bool _isFresh;

    [Header("State Materials"), Tooltip("0 = good, 1 = moldy, 2 = rotten")]
    [SerializeField] protected Material[] _materials;
    [SerializeField] protected Material _dirtyOSM;

    [Header("Debugging")]
    [SerializeField] protected bool _isDevloperMode;
    
#endregion
#region Protected

    protected Vector3 _startPosition;
    protected MeshRenderer _rend;

#endregion
#region Private
    
    private const float GRACE_PERIOD = 10f;
    private const float DECAY_SPEED = 4f;
    private const float STORED_RATE = 0.8f; 
    private const float NORMAL_RATE = 2f; 
    private const float DIRTY_RATE = 25f;

#endregion

#endregion

#region Methods 

#region Unity

    protected void Awake()
    {
        _rend = GetComponent<MeshRenderer>();

        if (_materials.Length != 3)
            Debug.LogWarning("Missing elements in materials");

        Debug.Log($"{name} developer mode: {_isDevloperMode}");
    }
    protected virtual void Start() 
    {
        // ingredients will only decay once the shift has started  
        GameManager.Instance.OnStartService += StartDecaying;
        GameManager.Instance.OnEndService += SetRotten;

        IngredientState = IngredientState.DEFAULT;   
        _startPosition = transform.position;
        _freshnessRate = 100f;   
        
        if (_isDevloperMode) return;

        // in case some ingredients are spawned during Service time
        if (GameManager.Instance.CurrentShift == GameShift.Service)
            StartDecaying();        
    }
    protected virtual void OnDestroy() 
    {
        if (_isDevloperMode) return;

        GameManager.Instance.OnStartService -= StartDecaying;
        GameManager.Instance.OnEndService -= SetRotten;
    }
    protected abstract void OnTriggerEnter(Collider other);

#endregion
#region Public

    public void Trashed()
    {
        IngredientState = IngredientState.MOLDY;
        _isFresh = false;
        _freshnessRate = 0f;
        
        ChangeMaterial();
    }
    public void SetRotten()
    {
        IngredientState = IngredientState.ROTTEN;
        _isFresh = false;
        _freshnessRate = 0f;

        ChangeMaterial();
    } 
    public void SetMoldy()
    {
        IngredientState = IngredientState.MOLDY;
        _isFresh = false;
        _freshnessRate = 0f;

        ChangeMaterial();
    }
    public void Stored() => IngredientState = IngredientState.STORED;
    public void Unstored()
    {
        StartCoroutine(Delay(2f));
        IngredientState = IngredientState.DEFAULT;
    }

#endregion
#region Helpers

    protected virtual void ChangeMaterial() 
    {
        switch (IngredientState)
        {
            case IngredientState.MOLDY:
                _rend.materials = new Material[] { _materials[1], _dirtyOSM };
                break;

            case IngredientState.ROTTEN:
                _rend.materials = new Material[] { _materials[2], _dirtyOSM };
                break;

            case IngredientState.DEFAULT:
                _rend.materials = new Material[] { _materials[0] };
                break;

            default: break;
        }
    }
    protected void StartDecaying() => StartCoroutine(DecayIngredient());

#endregion

#endregion

#region Enumerators

    protected IEnumerator DecayIngredient() 
    {
        yield return new WaitForSeconds(GRACE_PERIOD);
        
        while (FreshnessRate > 0f)
        {
            yield return new WaitForSeconds(DECAY_SPEED);

            // reduces freshness rate based on the ingredient's state
            switch (IngredientState)
            {
                case IngredientState.DEFAULT: 
                    _freshnessRate -= NORMAL_RATE;
                    break;

                case IngredientState.STORED:
                    _freshnessRate -= STORED_RATE;
                    break;

                case IngredientState.MOLDY:
                    _freshnessRate -= DIRTY_RATE;
                    break;

                default: break;
            }

            if (_freshnessRate < 1)
            {
                _freshnessRate = 0f;
                IngredientState = IngredientState.ROTTEN;
                SoundManager.Instance.PlaySound("fish dropped");
                ChangeMaterial();
            }
        }
    }
    protected IEnumerator Delay(float time)
    {
        if (time < 0f)
        {
            Debug.LogError("Given time was a negative number!");
            yield break;
        }

        yield return new WaitForSeconds(time);
    }

#endregion
}

#region Enumerations

    public enum IngredientState // IN A CERTAIN ORDER (DON'T RE-ORDER)
    { 
        DEFAULT, 
        ROTTEN, 
        MOLDY, 
        STORED
    }
    public enum IngredientType // IN A CERTAIN ORDER (DON'T RE-ORDER)
    {    
        RICE, 
        SALMON,
        TUNA
    }

#endregion