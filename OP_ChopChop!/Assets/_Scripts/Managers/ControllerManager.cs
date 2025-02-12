using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class ControllerManager : Singleton<ControllerManager>
{
    public ActionBasedController LeftController, RightController;

    protected override void Awake() => base.Awake();
    protected override void OnApplicationQuit() => base.OnApplicationQuit();
}
