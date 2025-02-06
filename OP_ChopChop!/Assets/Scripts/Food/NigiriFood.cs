using System.Collections.Generic;
using UnityEngine;

public class NigiriFood : Food
{
    [SerializeField] OrderType _orderType;
    public OrderType OrderType => _orderType;


    protected override void CreateDish(Vector3 pos, Quaternion rot)
    {
        GameObject dish = Instantiate(_dishPrefab, pos, rot);
        // dish.GetComponent<Dish>().DishScore =  ;
    }
}
