using UnityEngine;
    
public class Sushi : MonoBehaviour
{
    [SerializeField] DishPlatter _dishType;
    public DishPlatter DishType => _dishType;
}
