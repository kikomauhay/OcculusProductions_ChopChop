using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Trash : MonoBehaviour
{
    IXRSelectInteractor _mainInteractor;

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.GetComponent<Trashable>() != null)
        {
            if (other.gameObject.GetComponent<Trashable>()._trashTypes == TrashableType.INGREDIENT)
            {
                Destroy(other.gameObject);
                SoundManager.Instance.PlaySound("dispose food");
            }
            if (other.gameObject.GetComponent<Trashable>()._trashTypes == TrashableType.FOOD)
            {
                Debug.Log("Food has been thrown out");
          /*      GameObject _DirtyPlate = InstantiatePlate();
                AttachToHand(_DirtyPlate, _mainInteractor);*/
                
            }
            if (other.gameObject.GetComponent<Trashable>()._trashTypes == TrashableType.EQUIPMENT)
            {
                //Reset Equipment here
                //Set Reset Points
            }
        }
    }

    #region Functions

    // If we have spawn manager na, put the Instantiate codes into spawn manager na lang
    private GameObject InstantiatePlate(GameObject _plate, Vector3 _currentPosition, Quaternion _currentRotation)
    {
        return Instantiate(_plate,_currentPosition,_currentRotation);
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

    #endregion
}
