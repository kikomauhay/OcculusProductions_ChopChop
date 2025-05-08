using UnityEngine;

/// <summary>
/// 
/// This will act as the UNPLATED version of the food
/// This needs to be combined with a plate to make it into a dish
/// 
/// </summary>

[RequireComponent(typeof(Trashable))]
public class UPD_Food : MonoBehaviour
{

#region Properties
    public FoodCondition Condition => _foodCondition;
    public DishPlatter OrderType => _orderType;
    public float Score => _foodScore;

#endregion

#region Private

    [Header("Food Attrbutes")]
    [SerializeField] private FoodCondition _foodCondition;
    [SerializeField] private DishPlatter _orderType;
    [SerializeField] private float _foodScore; 

    [Header("Food Materials")]
    [SerializeField] private Material _dirtyOSM;
    [SerializeField] private Material _rottenMat, _moldyMat;

#endregion

#region Unity

    private void Start()
    {
        if (_orderType == DishPlatter.EMPTY)
            Debug.LogError("OrderType is set to empty!");

        _foodCondition = FoodCondition.CLEAN;
    }
    protected virtual void OnCollisionEnter(Collision other)
    {
        /*
        // food + ingredient
        if (other.gameObject.GetComponent<Ingredient>() != null)
        {
            Ingredient ing = other.gameObject.GetComponent<Ingredient>();

            if ((!IsContaminated || !IsExpired) && ing.IsFresh)
                ing.Contaminate();

            else if ((IsContaminated || IsExpired) && !ing.IsFresh)
                Contaminate();
        }

        // food + another food
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
                Contaminate();
            }
        }

        // food + dish
        if (other.gameObject.GetComponent<Dish>() != null)
        {
            Dish dish = other.gameObject.GetComponent<Dish>();

            // contamination logic
            if ((!IsContaminated || !IsExpired) &&
                (dish.IsContaminated || dish.IsExpired))
            {
                dish.HitTheFloor();
            }
            {
            }

            else if ((IsContaminated || IsExpired) &&
                     (!dish.IsExpired || !dish.IsContaminated))
            {
                Contaminate();
            }
        }
        */
    }

#endregion

#region Public

    public void SetRotten()
    {
        if (_foodCondition == FoodCondition.MOLDY) 
        {
            Debug.LogError($"{gameObject.name} is already moldy!");
            return;
        }
        
        _foodCondition = FoodCondition.ROTTEN; 
        GetComponent<MeshRenderer>().materials = new Material[] { _moldyMat, 
                                                                  _dirtyOSM };
    }
    public void SetMoldy()
    {
        if (_foodCondition == FoodCondition.ROTTEN) 
        {
            Debug.LogError($"{gameObject.name} is already rotten!");
            return;
        }

        _foodCondition = FoodCondition.ROTTEN; 
        GetComponent<MeshRenderer>().materials = new Material[] { _rottenMat,
                                                                  _dirtyOSM };
    }
    public void SetScore(float score) => _foodScore = score;

#endregion
}
