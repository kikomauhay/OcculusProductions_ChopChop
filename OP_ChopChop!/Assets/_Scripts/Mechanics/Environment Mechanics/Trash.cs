using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Trash : MonoBehaviour
{
    IXRSelectInteractor _mainInteractor;

    void OnTriggerEnter(Collider other)
    {
        GameObject obj = other.gameObject;

        if (obj.GetComponent<Trashable>() == null) return;
        
        switch(obj.GetComponent<Trashable>().TrashTypes)
        {
            case TrashableType.INGREDIENT:
                DestroyIngredient(obj.GetComponent<Ingredient>());
                break;
            case TrashableType.FOOD:
                break;
            case TrashableType.EQUIPMENT:
                break;
        }
    }

    #region Functions

    void DestroyIngredient(Ingredient ing)
    {
        Destroy(gameObject);
    }

    void DoFoodLogic(Food food)
    {
        food.SetContaminated();
        Debug.LogWarning($"{food.gameObject.name} has been contaminated!");
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
