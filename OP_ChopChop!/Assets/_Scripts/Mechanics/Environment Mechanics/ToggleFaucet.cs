using UnityEngine.XR.Interaction.Toolkit;
using System.Collections;
using UnityEngine;

public class ToggleFaucet : XRBaseInteractable
{
    [SerializeField] private GameObject _water;
    [SerializeField] private bool _isTutorial;

    private int _toggleCount = 0;
    private bool _tutorialDone, _enabled = false;

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

        _water.gameObject.SetActive(false);
        _enabled = false;
    }

    void FaucetSwitch(SelectEnterEventArgs args)
    {
        if (_water == null) return;
        
        if (_enabled) return;
        
        _enabled = true;
        _water.SetActive(!_water.activeSelf);
        SoundManager.Instance.PlaySound("toggle faucet", SoundGroup.APPLIANCES);
        StartCoroutine(Cooldown());
        base.OnSelectEntered(args);

        if (_tutorialDone) return;

        if (_isTutorial)
        {
            _toggleCount++;
            
            if (_toggleCount == 2)
            {
                _tutorialDone = true;
                StartCoroutine(OnBoardingHandler.Instance.Onboarding02());
            }
        }
    }

    IEnumerator Cooldown()
    {
        yield return new WaitForSeconds(2f);
        _enabled = false;
    }
}
