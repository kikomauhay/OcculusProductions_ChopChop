using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class ControllerManager : MonoBehaviour
{
    public static ControllerManager instance;
    public ActionBasedController leftController;
    public ActionBasedController rightController;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }
}
