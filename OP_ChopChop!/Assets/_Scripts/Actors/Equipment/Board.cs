using UnityEngine;

public class Board : Equipment 
{
    public static Board Instance { get; private set; }

    protected override void Start() 
    {
        base.Start();

        Instance = this;
        name = "Chopping Board";
    }

    protected override void OnTriggerExit(Collider other)
    {
        base.OnTriggerExit(other);

        if (!IsClean) return;

        if (other.gameObject.GetComponent<Ingredient>() != null)
            IncrementUseCounter();
    }
}
