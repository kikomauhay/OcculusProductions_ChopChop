using UnityEngine;

public class NEW_TutorialComponent : MonoBehaviour
{
    #region Members

    public bool IsInteractable => _isInteractable;
    public int TutorialIndex => _tutorialIndex;

    [SerializeField] private bool _isInteractable;
    [SerializeField] private int _tutorialIndex;

    private OnBoardingHandler _onbHandler;

    #endregion
    #region Methods

    private void Start()
    {
        _onbHandler = OnBoardingHandler.Instance;

        if (GameManager.Instance.CurrentShift == GameShift.Service)
            EnableInteraction();
    }

    public void DisableInteraction() => _isInteractable = false;
    public void EnableInteraction() => _isInteractable = true;
    public bool IsCorrectIndex() => 
        OnBoardingHandler.Instance.CurrentStep == TutorialIndex;
    
    #endregion
}
