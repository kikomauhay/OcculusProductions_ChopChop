using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Trashable))]
public abstract class Equipment : MonoBehaviour 
{
#region Members

    public bool IsClean { get; private set; }

    [SerializeField] protected Material _cleanMat, _dirtyMat;
    protected Vector3 _startPosition;

    // DIRTY MECHANIC
    protected int _usageCounter; // counter to know how many times equipment has been used
    protected int _maxUsageCounter; // max counter before it gets dirty

#endregion

#region Unity_Methods

    protected virtual void Start() 
    {
        _startPosition = transform.position;
        _usageCounter = 0;

        IsClean = true;
        GetComponent<MeshRenderer>().material = _cleanMat;

        GameManager.Instance.OnStartService += ResetPosition;
    }
    protected void OnDestroy() 
    {
        ResetPosition();
        GameManager.Instance.OnStartService -= ResetPosition;
    }
    protected virtual void OnTriggerEnter(Collider other)
    {
        Sponge sponge = other.gameObject.GetComponent<Sponge>();

        if (sponge == null) return;

        if (!sponge.IsWet) return;

        // sponge contaminates the equipment
        if (IsClean && !sponge.IsClean)
        {
            GetComponent<MeshRenderer>().material = _dirtyMat;
            _usageCounter = _maxUsageCounter;
            IsClean = false;
            return;
        }

        if (!IsClean && sponge.IsClean)
        {
            DoCleaning();
            sponge.IncrementUseCounter();
        }
    }
    protected void OnTriggerExit(Collider other)
    {
        if (other.gameObject.GetComponent<Sponge>() != null)
            StopCoroutine(CleanEquipment());
    }
    protected virtual void OnCollisionEnter(Collision other)
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
    }

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

        if (_usageCounter >= _maxUsageCounter)
            Contaminate();
    }
    public void Contaminate()
    {
        _usageCounter = _maxUsageCounter;
        IsClean = false;
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
        SpawnManager.Instance.SpawnVFX(VFXType.BUBBLE, transform, 3f);
        
        StopCoroutine(CleanEquipment());
        StartCoroutine(CleanEquipment());
    }
    protected IEnumerator CleanEquipment()
    {
        yield return new WaitForSeconds(2f);
        GetCleaned();
    }
    void GetCleaned()
    {
        _usageCounter = 0;
        IsClean = true;
        GetComponent<MeshRenderer>().material = _cleanMat;
    }

    #endregion
}
