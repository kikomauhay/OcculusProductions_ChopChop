using UnityEngine;

public class NigiriFood : Food
{    
    protected override void CreateDish(Vector3 pos, Quaternion rot)
    {
        GameObject dishToSpawn = Instantiate(_dishPrefab, pos, rot);
        Dish dish = dishToSpawn.GetComponent<Dish>();

        dish.DishScore = FoodScore;
        dish.OrderDishType = FoodType;
    }
}
