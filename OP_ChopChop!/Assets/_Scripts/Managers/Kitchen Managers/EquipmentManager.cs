using UnityEngine;


public class EquipmentManager : Singleton<EquipmentManager>
{
    public GameObject Knife, MeatBoard;

    protected override void Awake() => base.Awake();
    protected override void OnApplicationQuit() => base.OnApplicationQuit();   
}
