using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class Rollable : MonoBehaviour
{
    public ActionBasedController Left;
    public ActionBasedController Right;

    [SerializeField]
    GameObject _testPrefab;

    // Update is called once per frame
    private void OnTriggerEnter(Collider _other)
    {
        if (_other.GetComponent<ActionBasedController>() != null)
        {
            if(CheckGrip(Left) && CheckGrip(Right))
            {
                Roll();
            }
        }
    }

    void Roll()
    {
        // play animation and pause at intervals
        // instantiate new prefab for now
        Vector3 _currentPosition = this.transform.position;
        Quaternion _currentRotation = this.transform.rotation;

        Destroy(this.gameObject);
        Instantiate(_testPrefab, _currentPosition, _currentRotation);
    }

    private bool CheckGrip(ActionBasedController _controller)
    {
        return _controller.selectAction.action.ReadValue<float>() > 0.5f;
    }
}
