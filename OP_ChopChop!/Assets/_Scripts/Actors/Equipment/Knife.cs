using UnityEngine;

public class Knife : Equipment
{
    protected override void Start() 
    {
        base.Start();

        name = "Knife";
        _maxUsageCounter = 15;
    }
}
