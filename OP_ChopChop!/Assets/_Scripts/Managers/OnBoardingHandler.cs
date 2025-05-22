using System.Collections.Generic;
using UnityEngine.InputSystem;
using System.Collections;
using UnityEngine;

public class OnBoardingHandler : Singleton<OnBoardingHandler> 
{
#region Members

    public System.Action OnTutorialEnd { get; set; }
    public int CurrentStep { get; private set; }

#region SerializeField  

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

    private bool _canSkip;
    private const float PANEL_TIMER = 15f;
    
#endregion
#endregion

#region Unity

    protected override void OnApplicationQuit() => base.OnApplicationQuit();
    protected override void Awake() 
    {
        base.Awake();

        _dirtyCollider.SetActive(false); 
        _canSkip = false;
        CurrentStep = 0;
    }
    private void Update()
    {
        /* if Button.Pressed() AND canSkip == true:
                SkipTutorial()
        */
    }

#endregion

#region Onboarding

    public IEnumerator Onboarding01() // STARTING TUTORIAL
    {
        if (CurrentStep != 0) yield break;

        _canSkip = true;
        SoundManager.Instance.PlaySound("onb 01");
        Debug.LogWarning($"Playing Onboarding 0{CurrentStep + 1}");
        
        // spawns Atrium & highlights the Faucet Knob
        SpawnManager.Instance.SpawnTutorialCustomer(true);
        _faucetKnob.GetComponent<OutlineMaterial>().EnableHighlight();

        yield return new WaitForSeconds(20f);
        Continue.action.Enable();
        _canSkip = false;
        CurrentStep++;
    }
    public IEnumerator Onboarding02() // INGREDIENT ORDERING TUTORIAL
    {
        if (CurrentStep != 1) yield break;

        _canSkip = true;
        SoundManager.Instance.PlaySound("onb 02");
        Debug.LogWarning($"Playing Onboarding 0{CurrentStep + 1}");
        
        // highlights the Order Screen & removes the highlight of the Faucet Knob
        _faucetKnob.GetComponent<OutlineMaterial>().DisableHighlight();
        _orderScreen.GetComponent<OutlineMaterial>().EnableHighlight(); 
        
        yield return new WaitForSeconds(12f);
        _canSkip = false;
        CurrentStep++;
    }
    public IEnumerator Onboarding03() // FREEZER TUTORIAL
    {
        if (CurrentStep != 2) yield break;

        _canSkip = true;
        SoundManager.Instance.PlaySound("onb 03");
        Debug.LogWarning($"Playing Onboarding 0{CurrentStep + 1}");

        // highlights the freezer and removes the highlight of the Order Screen
        _orderScreen.GetComponent<OutlineMaterial>().DisableHighlight();
        _freezer.GetComponentInChildren<OutlineMaterial>().EnableHighlight();
        
        yield return new WaitForSeconds(21f);
        _canSkip = false;
        CurrentStep++;
    }   
    public IEnumerator Onboarding04() // CHOPPING TUTORIAL       
    {
        if (CurrentStep != 3) yield break;

        _canSkip = true;
        SoundManager.Instance.PlaySound("onb 04");
        Debug.LogWarning($"Playing Onboarding 0{CurrentStep + 1}");

        // sets up the stuff for slicing & removes the highlight of the freezer
        _freezer.GetComponentInChildren<OutlineMaterial>().DisableHighlight();
        _knife.GetComponentInChildren<OutlineMaterial>().EnableHighlight();
        StartCoroutine(EnableSlicingPanel());

        yield return new WaitForSeconds(13f);
        _canSkip = false;
        CurrentStep++;
    }
    public IEnumerator Onboarding05() // MOLDING TUTORIAL             
    {
        if (CurrentStep != 4) yield break;

        _canSkip = true;
        SoundManager.Instance.PlaySound("onb 05");
        Debug.LogWarning($"Playing Onboarding 0{CurrentStep + 1}");
        
        // removes the highlight in the knife 
        _knife.GetComponentInChildren<OutlineMaterial>().DisableHighlight();
        StartCoroutine(EnableMoldingPanel());

        yield return new WaitForSeconds(10f);
        _canSkip = false;
        CurrentStep++;
    }
    public IEnumerator Onboarding06() // FOOD COMBINATION TUTORIAL
    {
        if (CurrentStep != 5) yield break;

        _canSkip = true;
        SoundManager.Instance.PlaySound("onb 06");
        Debug.LogWarning($"Playing Onboarding 0{CurrentStep + 1}");
        
        yield return new WaitForSeconds(14f);
        _canSkip = false;
    }
    public IEnumerator Onboarding07() // SECOND CUSTOMER TUTORIAL
    {
        if (CurrentStep != 6) yield break;

        _canSkip = true;
        SoundManager.Instance.PlaySound("onb 07");
        Debug.LogWarning($"Playing Onboarding 0{CurrentStep + 1}");

        // spawns the Tuna Sashimi customer
        SpawnManager.Instance.SpawnTutorialCustomer(false);
        
        yield return new WaitForSeconds(29f);
        _canSkip = false;
        CurrentStep++;
    }
    public IEnumerator Onboarding08() // CLEANING TUTORIAL
    {
        if (CurrentStep != 7) yield break;

        _canSkip = true;
        SoundManager.Instance.PlaySound("onb 08");
        Debug.LogWarning($"Playing Onboarding 0{CurrentStep + 1}");
        
        // highlights the sponge and enables the dirty area
        _sponge.GetComponent<OutlineMaterial>().EnableHighlight();
        _dirtyCollider.SetActive(true);

        yield return new WaitForSeconds(1f);
        _canSkip = false;
        CurrentStep++;
    }
    public IEnumerator Onboarding09() // POST-SERVICE TUTORIAL
    {
        if (CurrentStep != 8) yield break;

        _canSkip = true;
        SoundManager.Instance.PlaySound("onb 09");
        Debug.LogWarning($"Playing Onboarding 0{CurrentStep + 1}");
        
        // shows the EOD to the player and 
        GameManager.Instance.EnableEOD();
        _menuScreen.GetComponent<OutlineMaterial>().EnableHighlight();
        _menuScreen.GetComponent<OutlineMaterial>().DisableHighlight();

        yield return new WaitForSeconds(8f);
        StartCoroutine(EnableFriendlyTipPanel());

        yield return new WaitForSeconds(52f);
        _canSkip = false;
        CurrentStep++;
    }

#endregion

#region Helpers

    public void Disable()
    { 
        if (_plates.Count > 0)
            foreach (GameObject p in _plates)
                p.SetActive(false);        

        OnTutorialEnd?.Invoke();
        gameObject.SetActive(false);
    }
    private void SkipTutorial()
    {    
        if (!_canSkip) return;

        SoundManager.Instance.StopSound();
        Debug.Log($"Skipped Onboarding 0{CurrentStep}");
        
        // stops the onboarding coroutine based on the CurrentStep
        switch (CurrentStep)
        {
            case 0: 
                StopCoroutine(Onboarding01()); 
                Debug.LogWarning("Coroutine stopped!");
                break;
                
            case 1: 
                StopCoroutine(Onboarding02()); 
                Debug.LogWarning("Coroutine stopped!");
                break;
            
            case 2: 
                StopCoroutine(Onboarding03()); 
                Debug.LogWarning("Coroutine stopped!");
                break;
            
            case 3: 
                StopCoroutine(Onboarding04()); 
                Debug.LogWarning("Coroutine stopped!");
                break;
            
            case 4: 
                StopCoroutine(Onboarding05()); 
                Debug.LogWarning("Coroutine stopped!");
                break;
            
            case 5: 
                StopCoroutine(Onboarding06()); 
                Debug.LogWarning("Coroutine stopped!");
                break;
            
            case 6: 
                StopCoroutine(Onboarding07()); 
                Debug.LogWarning("Coroutine stopped!");
                break;
            
            case 7: 
                StopCoroutine(Onboarding08()); 
                Debug.LogWarning("Coroutine stopped!");
                break;
            
            case 8: 
                StopCoroutine(Onboarding09()); 
                Debug.LogWarning("Coroutine stopped!");
                break;

            default: break;
        }
        
        _canSkip = false;
        CurrentStep++;
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

