using UnityEngine;

public class NigiriDish : Dish
{
    // it's empty now because all of the variables needed is in the parent script

    void Start()
    {
        OrderDishType = DishType.NIGIRI_SALMON; // test

        name = "Nigiri Dish";
        Debug.LogWarning($"{name}: {OrderDishType}");
    }
}
