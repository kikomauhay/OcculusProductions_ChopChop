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

    // can make this longer if needed
    [SerializeField] float _decayTimer; // 10s default
    [SerializeField] Material _expiredMat, _contaminatedMat;
    [SerializeField] GameObject _food;

#endregion

    protected void Awake() => GetComponent<Plate>().BoxTrigger.enabled = false;
    protected void Start() => StartCoroutine(Decay());


    // contaminate logic
    public void HitTheFloor()
    {
        if (IsExpired) return;

        IsContaminated = true;
        
        if (GetComponent<Plate>().IsClean)
            GetComponent<Plate>().HitTheFloor();

        _food.GetComponent<MeshRenderer>().material = _contaminatedMat; 
        Debug.LogWarning("contaminated!");
    }
    protected IEnumerator Decay()
    {
        yield return new WaitForSeconds(_decayTimer);
        
        if (IsContaminated) yield break;

        // expire logic
        IsExpired = true;
        _food.GetComponent<MeshRenderer>().material = _expiredMat;   

        Debug.LogWarning("expired!");
    } 

    public IEnumerator EnableBoxCollider()
    {
        yield return new WaitForSeconds(1f);
        GetComponent<Plate>().BoxTrigger.enabled = true;
    }
}
