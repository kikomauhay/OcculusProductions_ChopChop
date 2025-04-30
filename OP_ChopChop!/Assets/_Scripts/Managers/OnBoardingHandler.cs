using System.Collections.Generic;
using UnityEngine.InputSystem;
using System.Collections;
using UnityEngine;

public class OnBoardingHandler : Singleton<OnBoardingHandler> 
{
#region Members

    public System.Action OnTutorialEnd;

    [Header("Objects"), Tooltip("This is sequentually organized.")]
    [SerializeField] private GameObject _faucetKnob; 
    [SerializeField] private GameObject _orderScreen, _freezer, _knife;
    [SerializeField] private GameObject _riceCooker, _plate, _sponge, _menuScreen;
    [SerializeField, Space(5f)] private GameObject _dirtyCollider; // should be deactivated by default

    [Header("Panels")]
    [SerializeField] private GameObject _friendlyTipPanel;
    [SerializeField] private GameObject _slicingPanel, _moldingPanel;

    [Header("Input Button Reference")]
    [SerializeField] public InputActionReference Continue;

    [Header("Debugging")]
    [SerializeField] private List<GameObject> _plates;
    [SerializeField] private bool _doneWashing = false;

    private const float PANEL_TIMER = 15f;

#endregion

#region Unity

    protected override void OnApplicationQuit() => base.OnApplicationQuit();
    protected override void Awake() => base.Awake();

#endregion   

#region Onboarding

    public IEnumerator Onboarding01() // STARTING TUTORIAL
    {
        SpawnManager.Instance.SpawnTutorialCustomer(true);
        SoundManager.Instance.PlaySound("onb 01", SoundGroup.TUTORIAL);
        yield return new WaitForSeconds(20f);
    
        _faucetKnob.GetComponent<OutlineMaterial>().EnableHighlight();
        Continue.action.Enable();
    }
    public IEnumerator Onboarding02() // INGREDIENT ORDERING TUTORIAL
    {
        _faucetKnob.GetComponent<OutlineMaterial>().DisableHighlight();
        SoundManager.Instance.PlaySound("onb 02", SoundGroup.TUTORIAL);
        yield return new WaitForSeconds(10f);

        _orderScreen.GetComponent<OutlineMaterial>().EnableHighlight(); 
    }
    public IEnumerator Onboarding03() // FREEZER TUTORIAL
    {
        _orderScreen.GetComponent<OutlineMaterial>().DisableHighlight();
        SoundManager.Instance.PlaySound("onb 03", SoundGroup.TUTORIAL);
        yield return new WaitForSeconds(3f);

        _freezer.GetComponentInChildren<OutlineMaterial>().EnableHighlight();
    }   
    public IEnumerator Onboarding04() // CHOPPING TUTORIAL       
    {
        _freezer.GetComponentInChildren<OutlineMaterial>().DisableHighlight();
        SoundManager.Instance.PlaySound("onb 04", SoundGroup.TUTORIAL);
        yield return new WaitForSeconds(5f);

        _knife.GetComponentInChildren<OutlineMaterial>().EnableHighlight();
        StartCoroutine(EnableSlicingPanel());
    }
    public IEnumerator Onboarding05() // MOLDING TUTORIAL             
    {
        _knife.GetComponentInChildren<OutlineMaterial>().DisableHighlight();
        SoundManager.Instance.PlaySound("onb 05", SoundGroup.TUTORIAL);
        yield return new WaitForSeconds(5f);

        _riceCooker.GetComponentInChildren<OutlineMaterial>().EnableHighlight();
        StartCoroutine(EnableMoldingPanel());
    }
    public IEnumerator Onboarding06() // FOOD COMBINATION TUTORIAL    //WE ARE HEREEEE IN TERMS OF TESTING
    {
        _riceCooker.GetComponent<OutlineMaterial>().EnableHighlight();
        SoundManager.Instance.PlaySound("onb 06", SoundGroup.TUTORIAL);
        yield return new WaitForSeconds(10f);
    }
    public IEnumerator Onboarding07() // SECOND CUSTOMER TUTORIAL      //!!!!! NOT TRIGGERING!!!!!!
    {
        SoundManager.Instance.PlaySound("onb 07", SoundGroup.TUTORIAL);
        yield return new WaitForSeconds(2f);

        SpawnManager.Instance.SpawnTutorialCustomer(false);
    }
    public IEnumerator Onboarding08() // CLEANING TUTORIAL     //THIS RUNS AFTER 06
    {
        SoundManager.Instance.PlaySound("onb 08", SoundGroup.TUTORIAL);
        _sponge.GetComponent<OutlineMaterial>().EnableHighlight();
        yield return new WaitForSeconds(10f);

        _dirtyCollider.SetActive(true);
    }
    public IEnumerator Onboarding09() // POST-SERVICE TUTORIAL
    {
        // sponge.DisableHighlight is binded to an event when it's grabbed 
        
        SoundManager.Instance.PlaySound("onb 09", SoundGroup.TUTORIAL);
        _menuScreen.GetComponent<OutlineMaterial>().EnableHighlight();
        yield return new WaitForSeconds(8f);

        _menuScreen.GetComponent<OutlineMaterial>().DisableHighlight();
        StartCoroutine(EnableFriendlyTipPanel());
    }

#endregion

#region Helpers

    public void Disable()
    { 
        gameObject.SetActive(false);
        
        if (_plates.Count > 0)
        {
            foreach (GameObject p in _plates)
                Destroy(p);

            _plates.Clear();
        }

        OnTutorialEnd?.Invoke();
    }

#endregion    

#region Enumerators

    private IEnumerator EnableSlicingPanel()
    {
         _slicingPanel.SetActive(true);
         yield return new WaitForSeconds(PANEL_TIMER);
         _slicingPanel.SetActive(false);
    }
    private IEnumerator EnableMoldingPanel()
    {
         _moldingPanel.SetActive(true);
         yield return new WaitForSeconds(PANEL_TIMER);
         _moldingPanel.SetActive(false);
    }
    private IEnumerator EnableFriendlyTipPanel()
    {
        _friendlyTipPanel.SetActive(true);
        yield return new WaitForSeconds(PANEL_TIMER);
        _friendlyTipPanel.SetActive(false);
    }

#endregion
}