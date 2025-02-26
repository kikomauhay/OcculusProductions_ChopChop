using UnityEngine;

/// <summary>
/// 
/// This will act as the UNPLATED version of the food
/// This needs to be combined with a plate to make it into a dish
/// 
/// </summary>

public abstract class Food : MonoBehaviour
{
    public float FoodScore { get; set; } 
    public DishType FoodType { get; set; }
    public TrashableType TrashType => _trashType;

    [SerializeField] protected GameObject _dishPrefab;
    [SerializeField] protected TrashableType _trashType; 
    
    protected void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<Plate>() == null) return;

        if (other.gameObject.GetComponent<Plate>().IsDirty)
        {
            Debug.LogError("The plate is contaminating the food");
            // add code to infect the food
            return;
        }

        SpawnManager.Instance.SpawnVFX(VFXType.SMOKE, other.transform);
        CreateDish(other.transform);

        Destroy(gameObject);
        Destroy(other.gameObject);
    }

    public abstract void CreateDish(Transform t);
}
