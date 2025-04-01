using System.Collections;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(Trashable))]
public abstract class Equipment : MonoBehaviour 
{
    public bool IsClean => _isClean;

#region Members

    [SerializeField] protected bool _isClean;
    [SerializeField] protected Material _cleanMat, _dirtyMat;
    protected Vector3 _startPosition;

    // DIRTY MECHANIC
    protected int _usageCounter; // counter to know how many times equipment has been used
    protected int _maxUsageCounter; // max counter before it gets dirty

    private bool _coroutineRunning;

#endregion

#region Unity_Methods

    protected virtual void Start() 
    {
        GameManager.Instance.OnStartService += ResetPosition;
        
        _coroutineRunning = false;
        _startPosition = transform.position;

        // ternary operator syntax -> condition ? val_if_true : val_if_false
        _usageCounter = _isClean ? 0 : _maxUsageCounter;
        GetComponent<MeshRenderer>().material = _isClean ? _cleanMat : _dirtyMat;
    }
    protected void OnDestroy() 
    {
        ResetPosition();
        GameManager.Instance.OnStartService -= ResetPosition;
    }
    protected virtual void OnTriggerEnter(Collider other) // CLEANING MECHANIC
    {
        if (other.gameObject.GetComponent<Sponge>() != null) 
        {
            Sponge sponge = other.gameObject.GetComponent<Sponge>();
            
            if (sponge.IsClean) 
            {
                DoCleaning();
                sponge.IncrementUseCounter();
                return;
            }   

            // sponge contaminates the equipment
            SoundManager.Instance.PlaySound("wrong", SoundGroup.GAME);
            Contaminate();
        }
    }
    protected void OnTriggerExit(Collider other)
    {
        if (other.gameObject.GetComponent<Sponge>() == null) return;

        if (_coroutineRunning)
        {
            StopCoroutine(CleanEquipment());
            _coroutineRunning = false;
        }
    }
    
    /* protected virtual void OnCollisionEnter(Collision other)
    {
        // equipment + another equipment
        if (other.gameObject.GetComponent<Equipment>() != null)
        {
            Equipment eq = other.gameObject.GetComponent<Equipment>();
            
            if (!IsClean && eq.IsClean) 
                eq.Contaminate();
            
            if (IsClean && !eq.IsClean) 
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
    } */

#endregion

#region Public

    public virtual void HitTheFloor()
    {
        _usageCounter = _maxUsageCounter;
        IncrementUseCounter();
        ResetPosition();
    }
    public void IncrementUseCounter()
    {
        _usageCounter++;

        Debug.Log($"{name} use counter: {_usageCounter}/{_maxUsageCounter}");

        if (_usageCounter >= _maxUsageCounter)
            Contaminate();
    }
    public void Contaminate()
    {
        _usageCounter = _maxUsageCounter;
        _isClean = false;
        GetComponent<MeshRenderer>().material = _dirtyMat;
    }

#endregion

    protected void ResetPosition() 
    {
        transform.position = _startPosition;
        transform.rotation = Quaternion.identity;
    }
    
#region Cleaning

    protected virtual void DoCleaning()
    {
        SpawnManager.Instance.SpawnVFX(VFXType.BUBBLE, transform, 5f);
        
        if (_coroutineRunning)
            StopCoroutine(CleanEquipment());
        
        StartCoroutine(CleanEquipment());
    }
    protected IEnumerator CleanEquipment()
    {
        _coroutineRunning = true;
        yield return new WaitForSeconds(2f);
        GetCleaned();
    }
    protected void GetCleaned()
    {
        _usageCounter = 0;
        _isClean = true;
        GetComponent<MeshRenderer>().material = _cleanMat;
        _coroutineRunning = false;
    }

#endregion
}
