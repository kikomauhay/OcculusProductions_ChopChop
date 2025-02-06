using UnityEngine;



public class TunaIngredient : FishIngredient
{

    protected override void Start()
    {
        base.Start();

        _type = IngredientType.TUNA;
        name = "Tuna";
    }
}
