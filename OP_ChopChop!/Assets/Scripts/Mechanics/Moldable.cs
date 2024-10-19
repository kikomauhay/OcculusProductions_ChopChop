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
    private float minThreshold = 3f;
    private float maxThreshold = 3.2f;
    private bool IsMolded = false;
    private bool IsHoldingRice = false;

    private void Update()
    {
        if (IsHoldingRice)
        {
            Debug.Log("molding");
            return;
        }

        AddGrip();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<Rice>())
        {
            Debug.Log("Trigger On");
            if (IsMolded)
                return;
            IsHoldingRice = true;

            if (totalGripValue >= minThreshold && totalGripValue < maxThreshold)
            {
                MoldInstantiate(perfectMold);
            }
            else if (totalGripValue >= maxThreshold)
            {
                MoldInstantiate(overMold);
            }
        }
    }

    void AddGrip()
    {
        if(CheckGrip(left) && CheckGrip(right))
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
        totalGripValue += gripValue * Time.deltaTime * 100f;
        return gripValue;
    }

    private void MoldInstantiate(GameObject _moldable)
    {
        Vector3 currentPosition = this.transform.position;
        Quaternion currentRotation = this.transform.rotation;

        Destroy(this.gameObject);
        Instantiate(_moldable, currentPosition, currentRotation);
        IsMolded = true;
    }
}
