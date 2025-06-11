using UnityEngine;

public class NEW_TutorialComponent : MonoBehaviour
{
    public bool IsInteractable => _isInteractable;

    [SerializeField] private bool _isInteractable;

    public void DisableInteraction() => _isInteractable = false;
    public void EnableInteraction() => _isInteractable = true;
}
