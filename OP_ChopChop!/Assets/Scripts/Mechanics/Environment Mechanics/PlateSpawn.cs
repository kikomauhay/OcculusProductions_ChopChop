using UnityEngine.XR.Interaction.Toolkit;
using System.Collections;
using UnityEngine;

public class PlateSpawn : MonoBehaviour
{
    public ActionBasedController Left, Right;

    [SerializeField] GameObject _plate;
    [SerializeField] Collider _spwnCollider;
    [SerializeField] float _timer;

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
        else if (CheckGrip(Right) && other.gameObject.GetComponent<ActionBasedController>())
        {
            _interactor = Right.GetComponent<XRDirectInteractor>();
        }

        if (_interactor != null)
        {
            Debug.Log("Triggered, Plate Spawned");
            GameObject _spawnedPlate = InstantiatePlate(_plate);
            _spwnCollider.enabled = false;
            AttachToHand(_spawnedPlate,_interactor);
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
    private GameObject InstantiatePlate(GameObject _plate)
    {
        Vector3 _currentPosition = this.transform.position;
        Quaternion _currentRotation = this.transform.rotation;
        return Instantiate(_plate, _currentPosition, _currentRotation);
    }
    private void AttachToHand(GameObject _spawnedPlate, IXRSelectInteractor _interactor)
    {
        XRGrabInteractable _grabInteractable = _spawnedPlate.GetComponent<XRGrabInteractable>();
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
