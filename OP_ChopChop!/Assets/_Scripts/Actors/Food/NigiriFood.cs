using UnityEngine;

public class NigiriFood : Food
{
    public override void CreateDish(Transform t)
    {
        GameObject dishToSpawn = SpawnManager.Instance.SpawnObject(_dishPrefab, t, 
                                                                   SpawnObjectType.DISH);
        
        Dish dish = dishToSpawn.GetComponent<Dish>();

        dish.DishScore = FoodScore;
        dish.OrderDishType = FoodType;
    }
}
