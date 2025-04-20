using UnityEngine.XR.Interaction.Toolkit;
using System.Collections;
using UnityEngine;
public class ToggleFaucet : XRBaseInteractable
{
    [SerializeField] private GameObject _water;
    [SerializeField] private bool _isTutorial;

    private bool _enabled = false;
    private int _toggleCounter;

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
        {
            interactionManager = FindObjectOfType<XRInteractionManager>();
        }
        _water.gameObject.SetActive(false);
        _enabled = false;
        _toggleCounter = 0;
    }

    void FaucetSwitch(SelectEnterEventArgs args)
    {
        if (_water == null || _enabled) return;
        
        _enabled = true;
        _water.SetActive(!_water.activeSelf);
        SoundManager.Instance.PlaySound("toggle faucet", SoundGroup.APPLIANCES);
        
        StartCoroutine(Cooldown());
        base.OnSelectEntered(args);

        if (_isTutorial)
        {
            _toggleCounter++;

            if (_toggleCounter == 2)
            {
                GetComponent<OutlineMaterial>().DisableHighlight();
                StartCoroutine(OnBoardingHandler.Instance.CallOnboarding(1));
            }
        }
    }

    IEnumerator Cooldown()
    {
        yield return new WaitForSeconds(2f);
        _enabled = false;
    }
}
