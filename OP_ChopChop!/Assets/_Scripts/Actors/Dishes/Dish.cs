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
    public DishType OrderDishType { get; set; }

#endregion

    public void Contaminate()
    {
        IsContaminated = true;
        GetComponent<Plate>().ToggleClean();
        GetComponentInChildren<Food>().Contaminate();
    }
}
