using System.Collections;
using System.Net.Http.Headers;
using UnityEngine;

[RequireComponent(typeof(Trashable))]
public abstract class Equipment : MonoBehaviour 
{

#region Members

    public bool IsClean => _isClean;
    public Material DirtyMaterial => _dirtyMat;

    [SerializeField] protected bool _isClean;
    [SerializeField] protected Material _dirtyOSM, _cleanMat, _dirtyMat;
    protected Vector3 _startPosition;
    protected Renderer _rend;

    // DIRTY MECHANIC
    [SerializeField] protected int _maxUsageCounter; // max counter before it gets dirty
    protected int _usageCounter;                     // counter to know how many times equipment has been used
    private bool _coroutineRunning;

    [Header("Debugging")]
    [SerializeField] protected bool _isDeveloperMode;

#endregion

#region Unity

    protected virtual void Awake()
    {
        _rend = GetComponent<Renderer>();
    }
    protected virtual void Start() 
    {
        GameManager.Instance.OnStartService += ResetPosition;
        
        _isClean = true;
        _coroutineRunning = false;
        _startPosition = transform.position;

        _usageCounter = 0;

        if (_maxUsageCounter == 0)
            Debug.LogError($"Max use for {gameObject.name} is 0");
    }
    protected virtual void OnDestroy() 
    {
        ResetPosition();

        if (!_isDeveloperMode)
            GameManager.Instance.OnStartService -= ResetPosition;
    }
    protected virtual void OnTriggerEnter(Collider other) // CLEANING MECHANIC
    {
        Sponge sponge = other.gameObject.GetComponent<Sponge>();
        
        if (sponge != null)
            DoCleaning(sponge);
    }
    protected virtual void OnTriggerExit(Collider other)
    {
        Sponge sponge = other.gameObject.GetComponent<Sponge>();
        
        if (sponge == null) return;

        if (_coroutineRunning)
        {
            StopCoroutine(CO_Clean(sponge));
            _coroutineRunning = false;
        }
    }
    protected virtual void OnCollisionEnter(Collision other) // CROSS-CONTAMINATION LOGIC
    {
        /*
        if (GetComponent<Board>() != null) return;

        // equipment + another equipment
        if (other.gameObject.GetComponent<Equipment>() != null)
        {
            Equipment eq = other.gameObject.GetComponent<Equipment>();
            
            if (!IsClean && eq.IsClean) 
                eq.Contaminate();
            
            else if (IsClean && !eq.IsClean) 
                Contaminate();            
        }

        // equipment + ingredient
        if (other.gameObject.GetComponent<Ingredient>() != null)
        {
            Ingredient ing = other.gameObject.GetComponent<Ingredient>();

            if (!IsClean && ing.IsFresh) 
                ing.Contaminate();
            
            else if (IsClean && !ing.IsFresh) 
                Contaminate();
        }

        // equipment + food
        if (other.gameObject.GetComponent<Food>() != null)
        {
            Food food = other.gameObject.GetComponent<Food>();

            // contamination logic
            if (!IsClean && (food.IsContaminated || food.IsExpired)) 
                food.Contaminate();
            
            else if (IsClean && (!food.IsExpired || !food.IsContaminated)) 
                Contaminate();
        }

        // equipment + dish
        if (other.gameObject.GetComponent<Dish>() != null)
        {
            Dish dish = other.gameObject.GetComponent<Dish>();

            // contamination logic
            if (!IsClean && (dish.IsContaminated || dish.IsExpired))
                dish.HitTheFloor();

            else if (IsClean && (!dish.IsExpired || !dish.IsContaminated))
                Contaminate();
        }
        */
    }

#endregion

#region Public

#region Virtual

    public virtual void HitTheGround()
    {
        SetDirty();
        ResetPosition();
    }
    public virtual void Trashed()
    {
        SetDirty();
        ResetPosition();
    }

#endregion

    public void IncrementUseCounter()
    {
        _usageCounter++;

        if (_usageCounter == _maxUsageCounter) 
            SetDirty();        
    }
    public void SetDirty()
    {
        _usageCounter = _maxUsageCounter;
        _isClean = false;
        _rend.materials = new Material[] { _dirtyMat, _dirtyOSM };
    }    

#endregion

#region Helpers

    protected void ResetPosition() 
    {
        transform.position = _startPosition;
        transform.rotation = Quaternion.identity;
    }
    protected IEnumerator CO_Clean(Sponge sponge)
    {
        _coroutineRunning = true;
        yield return new WaitForSeconds(2f);
        SetClean(sponge);
    }
    protected void SetClean(Sponge sponge)
    {
        _usageCounter = 0;
        _isClean = true;
        _rend.materials = new Material[] { _cleanMat };
        _coroutineRunning = false;
        sponge.SetDirty();
    }
    protected virtual void DoCleaning(Sponge sponge)
    {
        SpawnManager.Instance.SpawnVFX(VFXType.BUBBLE, transform, 5f);
        
        if (_coroutineRunning)
            StopCoroutine(CO_Clean(sponge));
        
        StartCoroutine(CO_Clean(sponge));
    }

#endregion
}
