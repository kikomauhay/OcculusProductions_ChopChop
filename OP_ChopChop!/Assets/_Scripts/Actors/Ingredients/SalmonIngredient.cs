public class SalmonIngredient : Ingredient
{
    protected override void Start()
    {
        base.Start();
        name = "Salmon Ingredient";
    }

    protected override void OnDestroy() => base.OnDestroy();
    
}