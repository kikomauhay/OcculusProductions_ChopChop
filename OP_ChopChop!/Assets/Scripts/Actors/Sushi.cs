using UnityEngine;

public enum DishType { NIGIRI_SALMON, NIGIRI_TUNA, MAKI_SALMON, MAKI_TUNA, }

public class Sushi : MonoBehaviour
{
    [SerializeField] DishType _dishType;
    public DishType DishType => _dishType;
}
