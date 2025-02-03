using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class Moldable : MonoBehaviour
{
    public ActionBasedController Left, Right;

    [SerializeField]
    GameObject _perfectMold;
    [SerializeField]
    GameObject _smokeVFX;
    [SerializeField]
    float _moldLimit;
   
    private float _moldCounter;

    private void Awake()
    {
        Left = ControllerManager.Instance.LeftController;
        Right = ControllerManager.Instance.RightController;
    }
    private void Update()
    {
        Debug.Log(_moldCounter);
        if (_moldCounter >= _moldLimit)
        {
            MoldInstantiate(_perfectMold);
        }
    }

    private void OnTriggerEnter(Collider _other)
    {
        if(CheckGrip(Left))
        {
            Debug.Log("Left Detected");
            if (Right != null && CheckGrip(Right))
            {
                Debug.Log("Molding");
                _moldCounter++;
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
