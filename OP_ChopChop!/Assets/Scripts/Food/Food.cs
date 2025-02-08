using UnityEngine;

/// <summary>
/// 
/// This will act as the UNPLATED version of the food
/// This needs to be combined with a plate to make it into a dish
/// 
/// </summary>

public abstract class Food : MonoBehaviour
{
    [SerializeField] protected GameObject _dishPrefab;

    public float FoodScore { get; set; }


    protected void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<Plate>() == null) return;

        Vector3 pos = other.transform.position;
        Quaternion rot = other.transform.rotation;

        // UI effects when you spawn the dish
        SpawnManager.Instance.OnSpawnVFX?.Invoke(VFXType.SMOKE, pos, rot);
        CreateDish(pos, rot);

        Destroy(gameObject);
        Debug.Log("Created a dish"); // test
    }

    protected abstract void CreateDish(Vector3 pos, Quaternion rot);
}
