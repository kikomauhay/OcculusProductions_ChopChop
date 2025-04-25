using UnityEngine.XR.Interaction.Toolkit;
using System.Collections;
using UnityEngine;

public class ToggleFaucet : XRBaseInteractable
{
    [SerializeField] private GameObject _water;

    private bool _enabled = false;

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
        if (_water == null || _enabled) return;
        
        _enabled = true;
        _water.SetActive(!_water.activeSelf);
        SoundManager.Instance.PlaySound("toggle faucet", SoundGroup.APPLIANCES);
        
        StartCoroutine(Cooldown());
        base.OnSelectEntered(args);
    }

    IEnumerator Cooldown()
    {
        yield return new WaitForSeconds(2f);
        _enabled = false;
    }
}
