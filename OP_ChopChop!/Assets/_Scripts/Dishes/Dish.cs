using UnityEngine;

/// <summary>
/// 
/// This will act as the PLATED version of the food
/// This is also the prefab that will be served to the customers
/// 
/// </summary>

public enum DishType 
{ 
    NIGIRI_SALMON, 
    NIGIRI_TUNA, 
    MAKI_SALMON, 
    MAKI_TUNA 
}

public abstract class Dish : MonoBehaviour
{
    public float DishScore { get; set; }
    public bool IsContaminated { get; set; }
    public DishType OrderDishType { get; set; }

    [SerializeField] protected DishType _orderDishType;
}
