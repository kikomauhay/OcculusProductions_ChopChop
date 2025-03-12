using UnityEngine;

public class ChefHat : StaticInstance<ChefHat> {

    public bool HatWorn { get; private set; } = false;

    protected override void Awake() => base.Awake();
    protected override void OnApplicationQuit() => base.OnApplicationQuit(); 

    public void StartService() 
    {
        HatWorn = true;
        GameManager.Instance.ChangeShift(GameShift.SERVICE);
    }
}
