using UnityEngine;

public class Board : Equipment {

    protected override void Start() 
    {
        base.Start();

        name = "Chopping Board";
        _maxUsageCounter = 10;
    }
}
