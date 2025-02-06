using System;
using UnityEngine;



public class SalmonIngredient : FishIngredient
{
    protected override void Start()
    {
        base.Start();

        _type = IngredientType.SALMON;
        name = "Salmon";
    }

}
