using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class RiceSpawn : MonoBehaviour
{
    public ActionBasedController left;
    public ActionBasedController right;

    [SerializeField]
    GameObject UnmoldedRice;

    [SerializeField]
    Collider InstantiateCollider;

    [SerializeField]
    float Timer;

    private void Awake()
    {
        left = ControllerManager.instance.leftController;
        right = ControllerManager.instance.rightController;
    }

    private void OnTriggerEnter(Collider other)
    {
        IXRSelectInteractor interactor = null;
        if (CheckGrip(left) && other.gameObject.GetComponent<ActionBasedController>())
        {
            interactor = left.GetComponent<XRDirectInteractor>(); 
        }
        if (CheckGrip(right) && other.gameObject.GetComponent<ActionBasedController>())
        {
            interactor = right.GetComponent<XRDirectInteractor>();
        }
        if (interactor != null)
        {
            Debug.Log("Triggered, Rice spawned");
            GameObject SpawnedRice = InstantiateRice(UnmoldedRice);
            InstantiateCollider.enabled = false;
            AttachToHand(SpawnedRice, interactor);
            IResetTrigger();
        }
        else
        {
            Debug.Log("Interactor null");
        }
    }

    private bool CheckGrip(ActionBasedController controller)
    {
        return controller.selectAction.action.ReadValue<float>() > 0.5f;
    }

    private GameObject InstantiateRice(GameObject _rice)
    {
        Vector3 currentPosition = this.transform.position;
        Quaternion currentRotation = this. transform.rotation;
        return Instantiate(_rice, currentPosition, currentRotation);
    }

    private void AttachToHand(GameObject spawnedRice, IXRSelectInteractor interactor)
    {
        XRGrabInteractable grabInteractable = spawnedRice.GetComponent<XRGrabInteractable>();
        XRInteractionManager interactionManager = grabInteractable.interactionManager as XRInteractionManager;
        if (interactionManager == null 
            && interactor is MonoBehaviour interactorObject)
        {
            interactionManager = interactorObject.GetComponentInParent<XRInteractionManager>();
        }
        if (grabInteractable != null 
            && interactionManager != null)
        {
            interactionManager.SelectEnter(interactor, grabInteractable);
        }
        else
        {
            Debug.LogError("Spawned object does not have an XRGrabInteractable component.");
        }
    }

    private IEnumerator IResetTrigger()
    {
        yield return new WaitForSeconds(Timer);
        ResetCollider();
    }

    private void ResetCollider()
    {
        InstantiateCollider.enabled = true;
    }
}
