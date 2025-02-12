using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class RiceSpawn : MonoBehaviour
{
    public ActionBasedController Left;
    public ActionBasedController Right;

    IXRSelectInteractor _mainInteractor;

    [SerializeField]
    GameObject _unmoldedRice;

    [SerializeField]
    Collider _spwnCollider;

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
            _mainInteractor = _interactor;
        }
        if (CheckGrip(Right) && other.gameObject.GetComponent<ActionBasedController>())
        {
            _interactor = Right.GetComponent<XRDirectInteractor>();
            _mainInteractor = _interactor;
        }
        if (_mainInteractor != null)
        {
            _mainInteractor.selectEntered.AddListener(RiceEvent);
        }
        else
        {
            Debug.Log("Interactor null");
        }
    }
    private void OnTriggerExit(Collider other)
    {
        _mainInteractor.selectEntered.RemoveListener(RiceEvent);
        _mainInteractor = null;
    }


    private void RiceEvent(SelectEnterEventArgs args)
    { 
            Debug.Log("Triggered, Rice spawned");
            GameObject _spawnedRice = InstantiateRice(_unmoldedRice);
            _spwnCollider.enabled = false;
            AttachToHand(_spawnedRice, _mainInteractor);
            IResetTrigger();    
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
        _spwnCollider.enabled = true;
    }

    private IEnumerator IResetTrigger()
    {
        yield return new WaitForSeconds(_timer);
        ResetCollider();
    }
}
