using UnityEngine;
    
public class Sushi : MonoBehaviour
{
    [SerializeField] DishType _dishType;
    public DishType DishType => _dishType;
}
