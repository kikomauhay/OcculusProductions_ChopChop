using UnityEngine;

public class NigiriFood : Food
{
    public override void CreateDish(Transform t)
    {
        GameObject dishToSpawn = Instantiate(_dishPrefab, t.position, t.rotation);
        Dish dish = dishToSpawn.GetComponent<Dish>();

        dish.DishScore = FoodScore;
        dish.OrderDishType = FoodType;
    }
}
