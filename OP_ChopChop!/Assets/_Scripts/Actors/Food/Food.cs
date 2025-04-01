using UnityEngine;

/// <summary>
/// 
/// This will act as the UNPLATED version of the food
/// This needs to be combined with a plate to make it into a dish
/// 
/// </summary>

[RequireComponent(typeof(Trashable))]
public abstract class Food : MonoBehaviour
{
    public bool IsContaminated { get; private set; }
    public bool IsExpired { get; private set; }
    public float FoodScore { get; set; }
    public DishType FoodType { get; set; }

    [SerializeField] protected GameObject _dishPrefab;
    [SerializeField] protected Material _rottenMat, _contaminatedMat, _outlineTexture;

#region Unity

    protected virtual void Start()
    {
        FoodScore = 0f;
        IsContaminated = false;
        IsExpired = false;
    }
    protected void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<Plate>() == null) return;
        
        if (!other.gameObject.GetComponent<Plate>().IsClean) return;

        if (IsContaminated || IsExpired) return;

        // destorys both the food & plate to spawn in a new dish
        Destroy(other.gameObject);
        Destroy(gameObject);
        CreateDish(other.transform);

        // fancy UX effects
        SpawnManager.Instance.SpawnVFX(VFXType.SMOKE, other.transform, 1f);
        SoundManager.Instance.PlaySound("poof", SoundGroup.VFX);
    }
    protected virtual void OnCollisionEnter(Collision other)
    {
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

            else if ((IsContaminated || IsExpired) &&
                     (!dish.IsExpired || !dish.IsContaminated))
            {
                Contaminate();
            }
        }
    }

#endregion

    public abstract void CreateDish(Transform t);
    public void Contaminate()
    {
        if (IsExpired) return;

        IsContaminated = true;
        GetComponent<MeshRenderer>().materials = new Material[] { _contaminatedMat, 
                                                                  _outlineTexture };
    }
    public void Expire()
    {
        if (IsContaminated) return;

        IsExpired = true;
        GetComponent<MeshRenderer>().materials = new Material[] { _rottenMat,
                                                                  _outlineTexture };
    }
}
