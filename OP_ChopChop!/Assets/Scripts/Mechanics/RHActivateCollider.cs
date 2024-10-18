using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class RHActivateCollider : MonoBehaviour
{
    [SerializeField]
    Collider MidCollider;

    public ActionBasedController left;
    public ActionBasedController right;

    private void Update()
    {
        if(CheckGrip(left) && CheckGrip(right))
        {
            MidCollider.enabled = true;
        }
        else
        {
            MidCollider.enabled = false;
        }
    }

    private bool CheckGrip(ActionBasedController controller)
    {
        return controller.selectAction.action.ReadValue<float>() > 0.5f;
    }
}
