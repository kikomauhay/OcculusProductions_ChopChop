using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class RiceSpawn : MonoBehaviour
{
    public ActionBasedController left;
    public ActionBasedController right;

    [SerializeField]
    GameObject UnmoldedRice;

    private void OnTriggerEnter(Collider other)
    {
        if (CheckGrip(left) && other.gameObject.GetComponent<ActionBasedController>()
            || CheckGrip(right) && other.gameObject.GetComponent<ActionBasedController>())
        {
            InstantiateRice(UnmoldedRice);
        }
    }

    private bool CheckGrip(ActionBasedController controller)
    {
        return controller.selectAction.action.ReadValue<float>() > 0.5f;
    }

    private void InstantiateRice(GameObject _rice)
    {
        Vector3 currentPosition = this.transform.position;
        Quaternion currentRotation = this. transform.rotation;
        Instantiate(_rice, currentPosition, currentRotation);
    }
}
