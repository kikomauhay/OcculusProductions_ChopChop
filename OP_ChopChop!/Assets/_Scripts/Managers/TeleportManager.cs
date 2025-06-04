using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;

public class TeleportManager : Singleton<TeleportManager>
{
    #region Members
    [SerializeField] private GameObject[] rays;
    public InputActionReference RightTeleport, LeftTeleport;

    private bool raysAreActive = false;
    #endregion

    #region Unity
    protected override void Awake()
    {
        base.Awake();

        RightTeleport.action.Enable();
        LeftTeleport.action.Enable();

        RightTeleport.action.performed += ToggleRays;
        LeftTeleport.action.performed += ToggleRays;
    }

    protected override void OnApplicationQuit()
    {
        base.OnApplicationQuit();

        RightTeleport.action.Disable();
        LeftTeleport.action.Disable();

        RightTeleport.action.performed -= ToggleRays;
        LeftTeleport.action.performed -= ToggleRays;
    }

    void Start()
    {
        foreach (GameObject ray in rays)
        {
            ray.SetActive(false);
        }
    }
    #endregion

    #region Functions
    private void ToggleRays(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            foreach (GameObject ray in rays)
            {
                ray.SetActive(true);
            }
        }
        //Not running from this point
        else
        {
            foreach(GameObject ray in rays)
            {
                ray.SetActive(false);
            }
        }
    }
    #endregion
}

