using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class RiceSpawn : MonoBehaviour
{
    public ActionBasedController Left;
    public ActionBasedController Right;

    [SerializeField]
    GameObject _unmoldedRice;

    [SerializeField]
    Collider _instantiateCollider;

    [SerializeField]
    float _timer;

    private void Awake()
    {
        Left = ControllerManager.Instance.LeftController;
        Right = ControllerManager.Instance.RightController;
    }

    private void OnTriggerEnter(Collider other)
    {
        IXRSelectInteractor _interactor = null;
        if (CheckGrip(Left) && other.gameObject.GetComponent<ActionBasedController>())
        {
            _interactor = Left.GetComponent<XRDirectInteractor>(); 
        }
        if (CheckGrip(Right) && other.gameObject.GetComponent<ActionBasedController>())
        {
            _interactor = Right.GetComponent<XRDirectInteractor>();
        }
        if (_interactor != null)
        {
            Debug.Log("Triggered, Rice spawned");
            GameObject _spawnedRice = InstantiateRice(_unmoldedRice);
            _instantiateCollider.enabled = false;
            AttachToHand(_spawnedRice, _interactor);
            IResetTrigger();
        }
        else
        {
            Debug.Log("Interactor null");
        }
    }

    private bool CheckGrip(ActionBasedController _controller)
    {
        return _controller.selectAction.action.ReadValue<float>() > 0.5f;
    }

    private GameObject InstantiateRice(GameObject _rice)
    {
        Vector3 _currentPosition = this.transform.position;
        Quaternion _currentRotation = this. transform.rotation;
        return Instantiate(_rice, _currentPosition, _currentRotation);
    }

    private void AttachToHand(GameObject _spawnedRice, IXRSelectInteractor _interactor)
    {
        XRGrabInteractable _grabInteractable = _spawnedRice.GetComponent<XRGrabInteractable>();
        XRInteractionManager _interactionManager = _grabInteractable.interactionManager as XRInteractionManager;
        if (_interactionManager == null 
            && _interactor is MonoBehaviour interactorObject)
        {
            _interactionManager = interactorObject.GetComponentInParent<XRInteractionManager>();
        }
        if (_grabInteractable != null 
            && _interactionManager != null)
        {
            _interactionManager.SelectEnter(_interactor, _grabInteractable);
        }
        else
        {
            Debug.LogError("Spawned object does not have an XRGrabInteractable component.");
        }
    }

    private void ResetCollider()
    {
        _instantiateCollider.enabled = true;
    }

    private IEnumerator IResetTrigger()
    {
        yield return new WaitForSeconds(_timer);
        ResetCollider();
    }
}
