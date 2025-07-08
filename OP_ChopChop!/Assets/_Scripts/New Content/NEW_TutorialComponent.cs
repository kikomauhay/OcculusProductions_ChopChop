using UnityEngine;

public class NEW_TutorialComponent : MonoBehaviour
{
    public bool IsInteractable => _isInteractable;

    [SerializeField] private bool _isInteractable;
    [SerializeField] private int _tutorialIndex;

    private OnBoardingHandler _onbHandler;

    private void Start()
    {
        _onbHandler = OnBoardingHandler.Instance;

        if (GameManager.Instance.CurrentShift == GameShift.Service)
            EnableInteraction();
    }

    public void DisableInteraction() => _isInteractable = false;
    public void EnableInteraction() => _isInteractable = true;

    /*
    public void PlayOnboarding()
    {
        if (!_isInteractable)
        {
            Debug.LogError($"Is Interactable: {_isInteractable}");
            return;
        }
        if (_tutorialIndex != handler.CurrentStep)
        {
            Debug.LogError("Wrong Tutorial step!");
            return;
        }

        // plays the Onboarding
        handler.PlayOnboarding();
        Debug.LogWarning($"Playing Onboarding 0{handler.CurrentStep}");
    }
    */
}
