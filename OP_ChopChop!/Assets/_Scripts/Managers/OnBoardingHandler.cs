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
    [SerializeField] private bool _isDeveloperMode;

    private bool _canSkip, _tutorialPlaying;
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
        _tutorialPlaying = false;
        CurrentStep = 1;

        Debug.Log($"{name} Developer Mode: {_isDeveloperMode}"); 
        Debug.Log($"Initial Step: {CurrentStep}"); 
    }
    private void Update()
    {
        if (_canSkip == true)
        {
            Continue.action.Enable();
            Continue.action.performed += SkipTutorial;
        }

        test();
    }

#region Testing

    private void test()
    {
        if (!_isDeveloperMode) return;

        if (Input.GetKeyDown(KeyCode.Return)) Debug.LogWarning($"Current step: {CurrentStep}");
        // if (Input.GetKeyDown(KeyCode.Space)) SkipTutorial();

        if (Input.GetKeyDown(KeyCode.Alpha1)) StartCoroutine(Onboarding01());
        if (Input.GetKeyDown(KeyCode.Alpha2)) StartCoroutine(Onboarding02());
        if (Input.GetKeyDown(KeyCode.Alpha3)) StartCoroutine(Onboarding03());
        if (Input.GetKeyDown(KeyCode.Alpha4)) StartCoroutine(Onboarding04());
        if (Input.GetKeyDown(KeyCode.Alpha5)) StartCoroutine(Onboarding05());
        if (Input.GetKeyDown(KeyCode.Alpha6)) StartCoroutine(Onboarding06());
        if (Input.GetKeyDown(KeyCode.Alpha7)) StartCoroutine(Onboarding07());
        if (Input.GetKeyDown(KeyCode.Alpha8)) StartCoroutine(Onboarding08());
        if (Input.GetKeyDown(KeyCode.Alpha9)) StartCoroutine(Onboarding09());
    }
    
#endregion
#endregion

#region Onboarding

    public IEnumerator Onboarding01() // STARTING TUTORIAL
    {
        if (CurrentStep != 1)
        {
            Debug.LogError("Wrong step!");
            yield break;
        }
        if (_tutorialPlaying)
        {
            Debug.LogError("Tutorial is already playing!");
            yield break;
        }

        _canSkip = true;
        _tutorialPlaying = true;
        SoundManager.Instance.PlaySound("onb 01");
        Debug.LogWarning($"Playing Onboarding 01");

        // spawns Atrium & highlights the Faucet Knob
        SpawnManager.Instance.SpawnTutorialCustomer(true);
        _faucetKnob.GetComponent<OutlineMaterial>().EnableHighlight();

        yield return new WaitForSeconds(20f);
        Continue.action.Enable();
        _canSkip = false;
        _tutorialPlaying = false;
        CurrentStep++;
    }
    public IEnumerator Onboarding02() // INGREDIENT ORDERING TUTORIAL
    {
        if (CurrentStep != 2)
        {
            Debug.LogError("Wrong step!");
            yield break;
        }
        if (_tutorialPlaying)
        {
            Debug.LogError("Tutorial is already playing!");
            yield break;
        }

        _canSkip = true;
        _tutorialPlaying = true;
        SoundManager.Instance.PlaySound("onb 02");
        Debug.LogWarning($"Playing Onboarding 02");

        // highlights the Order Screen & removes the highlight of the Faucet Knob
        _faucetKnob.GetComponent<OutlineMaterial>().DisableHighlight();
        _orderScreen.GetComponent<OutlineMaterial>().EnableHighlight();

        yield return new WaitForSeconds(12f);
        _canSkip = false;
        _tutorialPlaying = false;
        CurrentStep++;
    }
    public IEnumerator Onboarding03() // FREEZER TUTORIAL
    {
        if (CurrentStep != 3)
        {
            Debug.LogError("Wrong step!");
            yield break;
        }
        if (_tutorialPlaying)
        {
            Debug.LogError("Tutorial is already playing!");
            yield break;
        }

        _canSkip = true;
        _tutorialPlaying = false;
        SoundManager.Instance.PlaySound("onb 03");
        Debug.LogWarning($"Playing Onboarding 03");

        // highlights the freezer and removes the highlight of the Order Screen
        _orderScreen.GetComponent<OutlineMaterial>().DisableHighlight();
        _freezer.GetComponentInChildren<OutlineMaterial>().EnableHighlight();

        yield return new WaitForSeconds(21f);
        _canSkip = false;
        _tutorialPlaying = false;
        CurrentStep++;
    }
    public IEnumerator Onboarding04() // CHOPPING TUTORIAL       
    {
        if (CurrentStep != 4)
        {
            Debug.LogError("Wrong step!");
            yield break;
        }
        if (_tutorialPlaying)
        {
            Debug.LogError("Tutorial is already playing!");
            yield break;
        }

        _canSkip = true;
        _tutorialPlaying = true;
        SoundManager.Instance.PlaySound("onb 04");
        Debug.LogWarning($"Playing Onboarding 04");

        // sets up the stuff for slicing & removes the highlight of the freezer
        _freezer.GetComponentInChildren<OutlineMaterial>().DisableHighlight();
        _knife.GetComponentInChildren<OutlineMaterial>().EnableHighlight();
        StartCoroutine(EnableSlicingPanel());

        yield return new WaitForSeconds(13f);
        _canSkip = false;
        _tutorialPlaying = false;
        CurrentStep++;
    }
    public IEnumerator Onboarding05() // MOLDING TUTORIAL             
    {
        if (CurrentStep != 5)
        {
            Debug.LogError("Wrong step!");
            yield break;
        }
        if (_tutorialPlaying)
        {
            Debug.LogError("Tutorial is already playing!");
            yield break;
        }

        _canSkip = true;
        _tutorialPlaying = true;
        SoundManager.Instance.PlaySound("onb 05");
        Debug.LogWarning($"Playing Onboarding 05");

        // removes the highlight in the knife 
        _knife.GetComponentInChildren<OutlineMaterial>().DisableHighlight();
        StartCoroutine(EnableMoldingPanel());

        yield return new WaitForSeconds(10f);
        _canSkip = false;
        _tutorialPlaying = false;
        CurrentStep++;
    }
    public IEnumerator Onboarding06() // FOOD COMBINATION TUTORIAL
    {
        if (CurrentStep != 6)
        {
            Debug.LogError("Wrong step!");
            yield break;
        }
        if (_tutorialPlaying)
        {
            Debug.LogError("Tutorial is already playing!");
            yield break;
        }

        _canSkip = true;
        _tutorialPlaying = true;
        SoundManager.Instance.PlaySound("onb 06");
        Debug.LogWarning($"Playing Onboarding 06");

        yield return new WaitForSeconds(14f);
        _canSkip = false;
        _tutorialPlaying = false;
        CurrentStep++;
    }
    public IEnumerator Onboarding07() // SECOND CUSTOMER TUTORIAL
    {
        if (CurrentStep != 7)
        {
            Debug.LogError("Wrong step!");
            yield break;
        }
        if (_tutorialPlaying)
        {
            Debug.LogError("Tutorial is already playing!");
            yield break;
        }

        _canSkip = true;
        _tutorialPlaying = true;
        SoundManager.Instance.PlaySound("onb 07");
        Debug.LogWarning($"Playing Onboarding 07");

        // spawns the Tuna Sashimi customer
        SpawnManager.Instance.SpawnTutorialCustomer(false);

        yield return new WaitForSeconds(29f);
        _canSkip = false;
        _tutorialPlaying = false;
        CurrentStep++;
    }
    public IEnumerator Onboarding08() // CLEANING TUTORIAL
    {
        if (CurrentStep != 8)
        {
            Debug.LogError("Wrong step!");
            yield break;
        }
        if (_tutorialPlaying)
        {
            Debug.LogError("Tutorial is already playing!");
            yield break;
        }

        _canSkip = true;
        _tutorialPlaying = true;
        SoundManager.Instance.PlaySound("onb 08");
        Debug.LogWarning($"Playing Onboarding 08");

        // highlights the sponge and enables the dirty area
        _sponge.GetComponent<OutlineMaterial>().EnableHighlight();
        _dirtyCollider.SetActive(true);

        yield return new WaitForSeconds(17f);
        _canSkip = false;
        _tutorialPlaying = false;
        CurrentStep++;
    }
    public IEnumerator Onboarding09() // POST-SERVICE TUTORIAL
    {
        if (CurrentStep != 9)
        {
            Debug.LogError("Wrong step!");
            yield break;
        }
        if (_tutorialPlaying)
        {
            Debug.LogError("Tutorial is already playing!");
            yield break;
        }

        _canSkip = true;
        _tutorialPlaying = true;
        SoundManager.Instance.PlaySound("onb 09");
        Debug.LogWarning($"Playing Onboarding 09");

        // shows the EOD to the player and 
        GameManager.Instance.EnableEOD();
        _menuScreen.GetComponent<OutlineMaterial>().EnableHighlight();
        _menuScreen.GetComponent<OutlineMaterial>().DisableHighlight();

        yield return new WaitForSeconds(8f);
        StartCoroutine(EnableFriendlyTipPanel());

        yield return new WaitForSeconds(52f);
        _canSkip = false;
        _tutorialPlaying = false;
        CurrentStep++;

        Debug.LogWarning("Done with Onboarding!");
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
    public void SkipTutorial(InputAction.CallbackContext context)
    // public void SkipTutorial()
    {
        if (!_canSkip)
        {
            Debug.LogError("You cannot skip at the moment!");
            return;
        }

        SoundManager.Instance.StopSound();
        // Debug.Log($"Current step: 0{CurrentStep}");
        // Debug.Log($"Skipped Onboarding 0{CurrentStep}");

        // stops the onboarding coroutine based on the CurrentStep
        switch (CurrentStep)
        {
            case 0: return;

            case 1:
                StopCoroutine(Onboarding01());
                Debug.LogWarning("Onboarding01 stopped!");
                break;

            case 2:
                StopCoroutine(Onboarding02());
                Debug.LogWarning("Onboarding02 stopped!");
                break;

            case 3:
                StopCoroutine(Onboarding03());
                Debug.LogWarning("Onboarding03 stopped!");
                break;

            case 4:
                StopCoroutine(Onboarding04());
                StopCoroutine(EnableSlicingPanel());
                _slicingPanel.SetActive(false);
                Debug.LogWarning("Onboarding04 stopped!");
                break;

            case 5:
                StopCoroutine(Onboarding05());
                StopCoroutine(EnableMoldingPanel());
                _moldingPanel.SetActive(false);
                Debug.LogWarning("Onboarding05 stopped!");
                break;

            case 6:
                StopCoroutine(Onboarding06());
                Debug.LogWarning("Onboarding06 stopped!");
                break;

            case 7:
                StopCoroutine(Onboarding07());
                Debug.LogWarning("Onboarding07 stopped!");
                break;

            case 8:
                StopCoroutine(Onboarding08());
                Debug.LogWarning("Onboarding08 stopped!");
                break;

            case 9:
                StopCoroutine(Onboarding09());
                StopCoroutine(EnableFriendlyTipPanel());
                _friendlyTipPanel.SetActive(false);
                Debug.LogWarning("Onboarding09 stopped!");
                Debug.LogWarning("Done with Onboarding!");
                break;

            default: return;
        }

        _canSkip = false;
        _tutorialPlaying = false;
        CurrentStep++;

        // Debug.Log($"New current step: 0{CurrentStep}");
        Continue.action.Disable();
        Continue.action.performed -= SkipTutorial;
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

