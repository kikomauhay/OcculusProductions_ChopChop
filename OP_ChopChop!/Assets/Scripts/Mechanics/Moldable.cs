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
    float MoldLimit;
   
    private float MoldCounter;

    private void Awake()
    {
        left = ControllerManager.instance.leftController;
        right = ControllerManager.instance.rightController;
    }
    private void Update()
    {
        Debug.Log(MoldCounter);
        if (MoldCounter >= MoldLimit)
        {
            MoldInstantiate(perfectMold);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(CheckGrip(left))
        {
            Debug.Log("Left Detected");
            Rice rice = other.GetComponent<Rice>();
            if (rice != null && CheckGrip(right))
            {
                Debug.Log("Molding");
                MoldCounter++;
            }
        }
    }

    private bool CheckGrip(ActionBasedController controller)
    {
        return controller.selectAction.action.ReadValue<float>() > 0.5f;
    }

    private void MoldInstantiate(GameObject _moldable)
    {
        Vector3 currentPosition = this.transform.position;
        Quaternion currentRotation = this.transform.rotation;
        Destroy(this.gameObject);
        Instantiate(_moldable, currentPosition, currentRotation);
    }
}
