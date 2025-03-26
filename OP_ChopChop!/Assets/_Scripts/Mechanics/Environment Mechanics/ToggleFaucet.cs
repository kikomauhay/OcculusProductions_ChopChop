using System.Collections;
using System.Collections.Generic;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine;

public class ToggleFaucet : XRBaseInteractable
{
    [SerializeField] GameObject _water;
    bool _faucetToggle;

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
        _faucetToggle = false;
    }

    void FaucetSwitch(SelectEnterEventArgs args)
    {
        if (_water != null)
        {
            _water.SetActive(!_water.activeSelf);
        }
        base.OnSelectEntered(args);
    }
}
