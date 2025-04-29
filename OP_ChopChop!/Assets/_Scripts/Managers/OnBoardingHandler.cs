using System.Collections.Generic;
using UnityEngine.InputSystem;
using System.Collections;
using UnityEngine;

public class OnBoardingHandler : Singleton<OnBoardingHandler> 
{
#region Members

    public bool TutorialPlaying { get; private set; } = false;

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
        if (TutorialPlaying) 
        {
            Debug.LogWarning("Tutorial is currently playing!");
            yield break;
        }

        TutorialPlaying = true;
        SpawnManager.Instance.SpawnTutorialCustomer(true);
        SoundManager.Instance.PlaySound("onb 01", SoundGroup.TUTORIAL);
        yield return new WaitForSeconds(20f);
    
        _faucetKnob.GetComponent<OutlineMaterial>().EnableHighlight();
        Continue.action.Enable();
        TutorialPlaying = false;
    }
    public IEnumerator Onboarding02() // INGREDIENT ORDERING TUTORIAL
    {
        if (TutorialPlaying) 
        {
            Debug.LogWarning("Tutorial is currently playing!");
            yield break;
        }

        TutorialPlaying = true;
        _faucetKnob.GetComponent<OutlineMaterial>().DisableHighlight();
        SoundManager.Instance.PlaySound("onb 02", SoundGroup.TUTORIAL);
        yield return new WaitForSeconds(10f);

        _orderScreen.GetComponent<OutlineMaterial>().EnableHighlight();
        TutorialPlaying = false; 
    }
    public IEnumerator Onboarding03() // FREEZER TUTORIAL
    {
        if (TutorialPlaying) 
        {
            Debug.LogWarning("Tutorial is currently playing!");
            yield break;
        }

        TutorialPlaying = true;
        SoundManager.Instance.PlaySound("onb 03", SoundGroup.TUTORIAL);
        yield return new WaitForSeconds(3f);

        _freezer.GetComponentInChildren<OutlineMaterial>().EnableHighlight();
        TutorialPlaying = false;
    }   
    public IEnumerator Onboarding04() // CHOPPING TUTORIAL       
    {
        if (TutorialPlaying) 
        {
            Debug.LogWarning("Tutorial is currently playing!");
            yield break;
        }

        TutorialPlaying = true;
        _freezer.GetComponentInChildren<OutlineMaterial>().DisableHighlight();
        SoundManager.Instance.PlaySound("onb 04", SoundGroup.TUTORIAL);
        yield return new WaitForSeconds(5f);

        _knife.GetComponentInChildren<OutlineMaterial>().EnableHighlight();
        StartCoroutine(EnableSlicingPanel());
        TutorialPlaying = false;
    }
    public IEnumerator Onboarding05() // MOLDING TUTORIAL             
    {
        if (TutorialPlaying) 
        {
            Debug.LogWarning("Tutorial is currently playing!");
            yield break;
        }

        TutorialPlaying = true;
        _knife.GetComponentInChildren<OutlineMaterial>().DisableHighlight();
        SoundManager.Instance.PlaySound("onb 05", SoundGroup.TUTORIAL);
        yield return new WaitForSeconds(5f);

        _riceCooker.GetComponentInChildren<OutlineMaterial>().EnableHighlight();
        StartCoroutine(EnableMoldingPanel());
        TutorialPlaying = false;
    }
    public IEnumerator Onboarding06() // FOOD COMBINATION TUTORIAL    //WE ARE HEREEEE IN TERMS OF TESTING
    {
        if (TutorialPlaying) 
        {
            Debug.LogWarning("Tutorial is currently playing!");
            yield break;
        }

        TutorialPlaying = true;
        SoundManager.Instance.PlaySound("onb 06", SoundGroup.TUTORIAL);
        yield return new WaitForSeconds(10f);

        _plate.GetComponent<OutlineMaterial>().EnableHighlight();
        TutorialPlaying = false;
    }
    public IEnumerator Onboarding07() // SECOND CUSTOMER TUTORIAL      //!!!!! NOT TRIGGERING!!!!!!
    {
        if (TutorialPlaying) 
        {
            Debug.LogWarning("Tutorial is currently playing!");
            yield break;
        }

        TutorialPlaying = true;
        _plate.GetComponent<OutlineMaterial>().DisableHighlight();
        SoundManager.Instance.PlaySound("onb 07", SoundGroup.TUTORIAL);
        yield return new WaitForSeconds(12f);

        SpawnManager.Instance.SpawnTutorialCustomer(false);
        TutorialPlaying = false;
    }
    public IEnumerator Onboarding08() // CLEANING TUTORIAL     //THIS RUNS AFTER 06
    {
        if (TutorialPlaying) 
        {
            Debug.LogWarning("Tutorial is currently playing!");
            yield break;
        }

        TutorialPlaying = true;
        SoundManager.Instance.PlaySound("onb 08", SoundGroup.TUTORIAL);
        _sponge.GetComponent<OutlineMaterial>().EnableHighlight();
        yield return new WaitForSeconds(10f);

        _dirtyCollider.SetActive(true);
        TutorialPlaying = false;
    }
    public IEnumerator Onboarding09() // POST-SERVICE TUTORIAL
    {
        if (TutorialPlaying) 
        {
            Debug.LogWarning("Tutorial is currently playing!");
            yield break;
        }

        TutorialPlaying = true;
        GameManager.Instance.ShowEOD();
        SoundManager.Instance.PlaySound("onb 09", SoundGroup.TUTORIAL);
        _menuScreen.GetComponent<OutlineMaterial>().EnableHighlight();
        yield return new WaitForSeconds(8f);
        
        _menuScreen.GetComponent<OutlineMaterial>().DisableHighlight();
        StartCoroutine(EnableFriendlyTipPanel());
        TutorialPlaying = false;
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