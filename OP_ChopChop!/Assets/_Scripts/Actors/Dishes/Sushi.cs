using UnityEngine;
    
public class Sushi : MonoBehaviour
{
    [SerializeField] DishOrder _dishType;
    public DishOrder DishType => _dishType;
}
