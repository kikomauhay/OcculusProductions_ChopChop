using UnityEngine;

public class Onb_CleanMgr : StaticInstance<Onb_CleanMgr> 
{

    protected override void Awake() => base.Awake();
    protected override void OnApplicationQuit() => base.OnApplicationQuit();
    
}
