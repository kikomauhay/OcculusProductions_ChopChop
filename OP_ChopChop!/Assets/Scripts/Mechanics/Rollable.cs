using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class Rollable : MonoBehaviour
{
    public ActionBasedController left;
    public ActionBasedController right;

    [SerializeField]
    GameObject testPrefab;

    // Update is called once per frame
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<ActionBasedController>() != null)
        {
            if(CheckGrip(left) && CheckGrip(right))
            {
                Roll();
            }
        }
    }

    void Roll()
    {
        // play animation and pause at intervals
        // instantiate new prefab for now
        Vector3 currentPosition = this.transform.position;
        Quaternion currentRotation = this.transform.rotation;

        Destroy(this.gameObject);
        Instantiate(testPrefab, currentPosition, currentRotation);
    }

    private bool CheckGrip(ActionBasedController controller)
    {
        return controller.selectAction.action.ReadValue<float>() > 0.5f;
    }
}
