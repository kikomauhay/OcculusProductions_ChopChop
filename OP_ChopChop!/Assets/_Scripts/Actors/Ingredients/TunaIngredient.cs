using UnityEngine;

public class TunaIngredient : Ingredient
{
    protected override void Start()
    {
        base.Start();
        name = "Tuna Ingredient";
    }
    protected override void OnDestroy() => base.OnDestroy();

}
