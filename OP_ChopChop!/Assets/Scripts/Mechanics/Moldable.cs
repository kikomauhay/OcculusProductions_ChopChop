using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class Moldable : MonoBehaviour
{
    public ActionBasedController left;
    public ActionBasedController right;

    [SerializeField]
    GameObject perfectMold;
    [SerializeField]
    GameObject overMold;

    private float totalGripValue = 0f;
    private float minThreshold = 30f;
    private float maxThreshold = 35f;

    // Update is called once per frame

    private void OnTriggerEnter(Collider other)
    {
        AddGrip();

        if (totalGripValue >= minThreshold && totalGripValue < maxThreshold)
        {
            //delete current prefab and then instantiate perfect mold here
            Vector3 currentPosition = this.transform.position;
            Quaternion currentRotation = this.transform.rotation;

            Destroy(this.gameObject);
            Instantiate(perfectMold, currentPosition, currentRotation);
        }
        else if (totalGripValue >= maxThreshold)
        {
            //delete current prefab and then instantiate perfect mold here
            Vector3 currentPosition = this.transform.position;
            Quaternion currentRotation = this.transform.rotation;

            Destroy(this.gameObject);
            Instantiate(overMold, currentPosition, currentRotation);
        }
    }

    void AddGrip()
    {
        if(CheckGrip(left))
        {
            Debug.Log("left hands is gripping");
            GetGripValue(right);
            Debug.Log("Total Grip Value: " + totalGripValue);
        }
    }

    private bool CheckGrip(ActionBasedController controller)
    {
        return controller.selectAction.action.ReadValue<float>() > 0.5f;
    }

    private float GetGripValue(ActionBasedController controller)
    {
        float gripValue = controller.selectAction.action.ReadValue<float>();
        Debug.Log("Current Grip Value: " + gripValue);
        totalGripValue += gripValue;
        return gripValue;
    }
}
