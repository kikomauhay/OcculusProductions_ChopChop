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

    [SerializeField] protected GameObject _dishPrefab;
    
    protected void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<Plate>() == null) return;

        Plate plate = other.gameObject.GetComponent<Plate>();

        if (plate.IsDirty)
        {
            Debug.LogError("The plate is contaminating the food");
            return;
        }

        Vector3 pos = other.transform.position;
        Quaternion rot = other.transform.rotation;

        // UI effects when you spawn the dish
        SpawnManager.Instance.OnSpawnVFX?.Invoke(VFXType.SMOKE, pos, rot);
        CreateDish(pos, rot);

        Destroy(gameObject);
        Destroy(other.gameObject);
        Debug.Log("Created a dish"); // test
    }

    protected abstract void CreateDish(Vector3 pos, Quaternion rot);
}
