using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine;

public class Trash : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        GameObject obj = other.gameObject;

        if (obj.GetComponent<Trashable>() == null) return;

        if (obj.GetComponent<Sponge>() != null)
        {
            obj.GetComponent<Sponge>().ResetPosition();
            return;
        }
        
        switch(obj.GetComponent<Trashable>().TrashTypes)
        {
            case TrashableType.INGREDIENT:
                DestroyIngredient(obj.GetComponent<Ingredient>());
                break;

            case TrashableType.FOOD:
                DestroyFood(obj.GetComponent<Food>());
                break;

            case TrashableType.EQUIPMENT:
                DoEquipmentLogic(obj.GetComponent<Equipment>());
                break;

            default: break;
        }
    }

#region Functions

    private void DestroyIngredient(Ingredient ing)
    {
        Destroy(ing.gameObject);
        ing.Trashed();
    }

    private void DestroyFood(Food food)
    {
        Destroy(food.gameObject);
        SoundManager.Instance.PlaySound("dispose food", SoundGroup.FOOD);
    }

    private void DoEquipmentLogic(Equipment eq)
    {
        eq.HitTheGround();
        eq.GetComponent<Rigidbody>().velocity = Vector3.zero;
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
