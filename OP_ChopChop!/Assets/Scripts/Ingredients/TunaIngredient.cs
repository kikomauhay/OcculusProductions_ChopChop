public class TunaIngredient : FishIngredient
{

    protected override void Start()
    {
        base.Start();

        _ingredientType = IngredientType.TUNA;
        name = "Tuna";
    }
}
