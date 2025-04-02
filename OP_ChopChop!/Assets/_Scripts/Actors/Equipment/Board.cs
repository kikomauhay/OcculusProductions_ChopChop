using UnityEngine;

public class Board : Equipment {

    public static Board Instance { get; private set; }

    protected override void Start() 
    {
        base.Start();

        Instance = this;

        name = "Chopping Board";
        _maxUsageCounter = 10;
    }
}
