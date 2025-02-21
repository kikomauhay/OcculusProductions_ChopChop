using UnityEngine;

public class Sponge : Equipment
{
    public bool IsWet;

    protected override void Start()
    {
        base.Start();

        name = "Sponge";
/*        IsWet = false;*/
    }

    public void Dried() => IsWet = false;
    public void Wet() => IsWet = true;   
}
