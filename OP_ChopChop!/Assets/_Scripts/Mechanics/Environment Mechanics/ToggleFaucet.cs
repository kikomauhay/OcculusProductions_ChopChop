using UnityEngine.XR.Interaction.Toolkit;
using System.Collections;
using UnityEngine;

public class ToggleFaucet : XRBaseInteractable
{
    #region Members

    [SerializeField] private GameObject _water;
    [SerializeField] private bool _isTutorial;

    private NEW_TutorialComponent _tutorialComponent;
    private int _toggleCount = 0;
    private bool _tutorialDone, _enabled = false;

    #endregion

    #region Methods

    protected override void OnEnable()
    {
        base.OnEnable();
        selectEntered.AddListener(FaucetSwitch);
    }
    protected override void OnDisable()
    {
        base.OnDisable();
        selectEntered.RemoveListener(FaucetSwitch);
    }
    private void Start()
    {
        if (interactionManager == null) 
            interactionManager = FindObjectOfType<XRInteractionManager>();

        _tutorialComponent = GetComponent<NEW_TutorialComponent>();
        _water.gameObject.SetActive(false);
        _enabled = false;
    }

    private void FaucetSwitch(SelectEnterEventArgs args)
    {
        if (_water == null) return;
        
        if (_enabled) return;
        
        _enabled = true;
        _water.SetActive(!_water.activeSelf);
        SoundManager.Instance.PlaySound("toggle faucet");
        StartCoroutine(CO_Cooldown());
        base.OnSelectEntered(args);

        if (!_tutorialComponent.IsInteractable) return;

        if (_tutorialDone) return;

        if (_isTutorial)
        {
            _toggleCount++;
            
            if (_toggleCount >= 2)
            {
                _tutorialDone = true;
                OnBoardingHandler.Instance.AddOnboardingIndex();
                OnBoardingHandler.Instance.PlayOnboarding();
            }
        }
    }

    #endregion
    
    private IEnumerator CO_Cooldown()
    {
        yield return new WaitForSeconds(2f);
        _enabled = false;
    }
}
