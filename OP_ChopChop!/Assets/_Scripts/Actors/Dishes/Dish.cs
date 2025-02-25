using UnityEngine;

/// <summary>
/// 
/// This will act as the PLATED version of the food
/// This is also the prefab that will be served to the customers
/// 
/// </summary>


public abstract class Dish : MonoBehaviour
{
    public float DishScore { get; set; }
    public bool IsContaminated { get; set; }
    public DishType OrderDishType { get; set; }
}
