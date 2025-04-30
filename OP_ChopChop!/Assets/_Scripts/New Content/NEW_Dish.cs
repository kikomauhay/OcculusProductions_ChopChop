using UnityEngine;

public class NEW_Dish : MonoBehaviour 
{
#region Properties

    public GameObject[] FoodItems => _foodItems;
    public DishType Type => _dishType;
    public float Score => _dishScore;


#endregion

#region Private

    [Header("0 = N. Salmon, 1 = N. Tuna, 2 = S. Salmon, 3 = S. Tuna")]
    [SerializeField] private GameObject[] _foodItems;

    [SerializeField, Range(0f, 100f)] private float _dishScore;
    private DishType _dishType;


#endregion

#region Unity

    private void Awake()
    {
        
    }
    private void Start() 
    {
        DisableFoodItems();
    }

#endregion


#region Public

    public void EnableFood(DishType type)
    {
        _dishType = type;
        _foodItems[(int)type].SetActive(true);
    }

    public void DisableFoodItems()
    {
        // disables all food in the array
        if (_foodItems.Length > 0)
            foreach (GameObject item in _foodItems)
                item.SetActive(false);
    }

#endregion

}