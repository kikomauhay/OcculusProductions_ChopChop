using System.Runtime.InteropServices;
using UnityEngine;

public class Sponge : Equipment
{
    public bool IsWet { get; private set; }

    protected override void Start()
    {
        base.Start();

        name = "Sponge";
        IsWet = true;
    }

    public void Dried() => IsWet = false;
    public void Wet() => IsWet = true;   

    public void ToggleWetness() => IsWet = !IsWet;
}
