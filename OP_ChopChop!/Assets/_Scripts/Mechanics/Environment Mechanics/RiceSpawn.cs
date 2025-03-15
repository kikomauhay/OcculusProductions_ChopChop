using UnityEngine.XR.Interaction.Toolkit;
using System.Collections;
using UnityEngine;

public class RiceSpawn : MonoBehaviour
{
    public ActionBasedController Left, Right;

    IXRSelectInteractor _mainInteractor;

    [SerializeField] GameObject _ricePrefab;
    [SerializeField] Collider _spwnCollider;
    [SerializeField] float _resetTimer;

    void Awake()
    {
        Left = ControllerManager.Instance.LeftController;
        Right = ControllerManager.Instance.RightController;
    }

    void OnTriggerEnter(Collider other)
    {
        IXRSelectInteractor interactor = null;

        if (CheckGrip(Left) && other.gameObject.GetComponent<ActionBasedController>())
        {
            interactor = Left.GetComponent<XRDirectInteractor>(); 
            _mainInteractor = interactor;
        }
        if (CheckGrip(Right) && other.gameObject.GetComponent<ActionBasedController>())
        {
            interactor = Right.GetComponent<XRDirectInteractor>();
            _mainInteractor = interactor;
        }

        if (_mainInteractor != null)
            _mainInteractor.selectEntered.AddListener(RiceEvent);
        
        else
            Debug.LogError("Interactor is null");
        
    }
    void OnTriggerExit(Collider other)
    {
        _mainInteractor.selectEntered.RemoveListener(RiceEvent);
        _mainInteractor = null;
    }
    void RiceEvent(SelectEnterEventArgs args)
    { 
        Debug.LogWarning("Triggered, Rice spawned");

        GameObject _newRice = SpawnManager.Instance.SpawnObject(_ricePrefab,
                                                                    transform,
                                                                    SpawnObjectType.INGREDIENT); 
        
        _spwnCollider.enabled = false;
        AttachToHand(_newRice, _mainInteractor);
        ResetTrigger();
    }

    private bool CheckGrip(ActionBasedController _controller)
    {
        return _controller.selectAction.action.ReadValue<float>() > 0.5f;
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

    private void ResetCollider() => _spwnCollider.enabled = true;
    

    private IEnumerator ResetTrigger()
    {
        yield return new WaitForSeconds(_resetTimer);
        ResetCollider();
    }
}
