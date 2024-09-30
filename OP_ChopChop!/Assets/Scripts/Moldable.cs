using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class Moldable : MonoBehaviour
{
    public XRController left;
    public XRController right;

    [SerializeField]
    GameObject perfectMold;
    [SerializeField]
    GameObject overMold;

    private float totalGripValue = 0f;
    private float minThreshold = 2.5f;
    private float maxThreshold = 3f;

    private GameObject unmoldedRice;

    // Update is called once per frame
    void Update()
    {
        totalGripValue = 0f;
        AddGrip();

        if(totalGripValue >= minThreshold && totalGripValue < maxThreshold)
        {
            //delete current prefab and then instantiate perfect mold here
        }
        else if (totalGripValue >= maxThreshold)
        {
            //delete current prefab and then instantiate perfect mold here
        }
    }

    void AddGrip()
    {
        if(CheckGrip(left))
        {
            GetGripValue(right);
            Debug.Log("Total Grip Value: " + totalGripValue);
        }
    }

    private bool CheckGrip(XRController controller)
    {
        return controller.inputDevice.TryGetFeatureValue(CommonUsages.gripButton, out bool CheckGrip) && CheckGrip;
    }

    private float GetGripValue(XRController controller)
    {
        controller.inputDevice.TryGetFeatureValue(CommonUsages.grip, out float gripValue);
        Debug.Log("Current Grip Value: " + gripValue);
        totalGripValue += gripValue;
        return gripValue;
    }
}
