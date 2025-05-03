using System.Collections;
using UnityEngine;

/// <summary>
/// 
/// This will act as the PLATED version of the food
/// This is also the prefab that will be served to the customers
/// 
/// </summary>

[RequireComponent(typeof(Trashable))]
public abstract class Dish : MonoBehaviour
{
#region Readers

    public float DishScore { get; set; }
    public bool IsContaminated { get; private set; } = false;
    public bool IsExpired { get; private set; } = false;
    public DishType OrderDishType { get; set; }
    
#endregion

    [SerializeField] float _decayTimer; // 10s default (can be longer)
    [SerializeField] Material _expiredMat, _contaminatedMat;

#region Unity_Methods

    // protected void Awake() => GetComponent<Plate>().BoxTrigger.enabled = false;
    protected void Start() => StartCoroutine(Decay());    
    protected virtual void OnCollisionEnter(Collision other)
    {
        // dish + ingredient
        if (other.gameObject.GetComponent<Ingredient>() != null)
        {
            Ingredient ing = other.gameObject.GetComponent<Ingredient>();

            if ((!IsContaminated || !IsExpired) && ing.IsFresh)
                ing.Contaminate();

            else if ((IsContaminated || IsExpired) && !ing.IsFresh)
                HitTheFloor();
        }

        // dish + food
        if (other.gameObject.GetComponent<Food>() != null)
        {
            Food food = other.gameObject.GetComponent<Food>();
            
            if ((!IsContaminated || !IsExpired) &&
               (food.IsExpired || food.IsContaminated))
            {
                food.Contaminate();
            }

            else if ((IsContaminated || IsExpired) && 
                    (!food.IsExpired || !food.IsContaminated))
            {
                HitTheFloor();
            }
        }

        // dish + another dish
        if (other.gameObject.GetComponent<Dish>() != null)
        {
            Dish dish = other.gameObject.GetComponent<Dish>();

            // contamination logic
            if ((!IsContaminated || !IsExpired) && 
                (dish.IsContaminated || dish.IsExpired))
            {
                dish.HitTheFloor();
            }

            else if ((IsContaminated || IsExpired) && 
                     (!dish.IsExpired || !dish.IsContaminated))
            {
                HitTheFloor();
            }
        }
    }

#endregion

    public void HitTheFloor()
    {
        if (IsExpired) return;

        IsContaminated = true;
        
        if (GetComponent<Plate>().IsClean)
            GetComponent<Plate>().HitTheGround();

        GetComponentInChildren<MeshRenderer>().material = _contaminatedMat;
    }
    protected IEnumerator Decay() // Expiration logic
    {
        yield return new WaitForSeconds(_decayTimer);

        if (!IsContaminated && !IsExpired)
        {
            IsExpired = true;
            GetComponentInChildren<MeshRenderer>().material = _expiredMat;
        }
    } 
}
