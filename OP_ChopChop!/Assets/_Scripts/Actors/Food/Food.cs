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
    public TrashableType TrashType { get; protected set; }
    public DishType FoodType { get; set; }

    [SerializeField] protected GameObject _dishPrefab;
    [SerializeField] protected Material _rottenMat, _contaminatedMat;

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

        Destroy(other.gameObject);
        CreateDish(other.transform);
        Destroy(gameObject);

        SpawnManager.Instance.SpawnVFX(VFXType.SMOKE, other.transform, 1f);
        SoundManager.Instance.PlaySound("poof", SoundGroup.VFX);
    }

    public abstract void CreateDish(Transform t);
    public void Contaminate()
    {
        if (IsExpired) return;

        IsContaminated = true;
        GetComponent<MeshRenderer>().material = _contaminatedMat;
    }
    public void Expire()
    {
        if (IsContaminated) return;

        IsExpired = true;
        GetComponent<MeshRenderer>().material = _rottenMat;
    }
}
