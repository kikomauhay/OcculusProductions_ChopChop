using UnityEngine;

public class TunaIngredient : Ingredient
{
    protected override void Start()
    {
        base.Start();

        if (_interactable != null)
            HandManager.Instance.RegisterGrabbable(_interactable);
    }

    protected override void OnTriggerEnter(Collider other) {}
}
