using UnityEngine;

public class Board : Equipment {

    protected override void Start() 
    {
        base.Start();

        name = "Chopping Board";
        _maxUsageCounter = 10;
    }

    void Update() => test();

    void test()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            ToggleClean();
    }
}
