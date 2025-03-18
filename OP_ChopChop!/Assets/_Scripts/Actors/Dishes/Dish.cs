using UnityEngine;

/// <summary>
/// 
/// This will act as the PLATED version of the food
/// This is also the prefab that will be served to the customers
/// 
/// </summary>


public abstract class Dish : MonoBehaviour
{

#region Readers

    public float DishScore { get; set; }
    public bool IsContaminated { get; set; }
    public DishType OrderDishType { get; set; }
    public IngredientState IngredientState { get; private set; }

#endregion


    [SerializeField] protected Material _freshMat, _rottenMat;

    void Start()
    {
        IngredientState = IngredientState.DEFAULT;
        IsContaminated = false;
     
        GetComponent<Renderer>().material = _freshMat;
    }

    public void ToggleContaminated()
    {
        IsContaminated = !IsContaminated;
        IngredientState = IngredientState.CONTAMINATED;

        GetComponentInParent<Plate>().ToggleClean();

        GetComponent<Renderer>().material = IsContaminated ?
                                            _freshMat : _rottenMat;
    }
}
