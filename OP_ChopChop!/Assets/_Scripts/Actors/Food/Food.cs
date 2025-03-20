using UnityEngine;

/// <summary>
/// 
/// This will act as the UNPLATED version of the food
/// This needs to be combined with a plate to make it into a dish
/// 
/// </summary>

public abstract class Food : MonoBehaviour
{
    public bool IsContaminated { get; private set; }
    public float FoodScore { get; set; }
    public DishType FoodType { get; set; }
    public TrashableType TrashType { get; protected set; }


    [SerializeField] protected GameObject _dishPrefab;

    protected virtual void Start()
    {
        FoodScore = 0f;
        IsContaminated = false;
    }

    protected void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<Plate>() == null) return;

        if (!other.gameObject.GetComponent<Plate>().IsClean)
        {
            Debug.LogError("The plate is contaminating the food");
            // add code to infect the food
            return;
        }

        SpawnManager.Instance.SpawnVFX(VFXType.SMOKE, other.transform, 1f);
        CreateDish(other.transform);

        Destroy(gameObject);
        Destroy(other.gameObject);
    }

    public abstract void CreateDish(Transform t);
    public void SetContaminated() => IsContaminated = true;    
}
