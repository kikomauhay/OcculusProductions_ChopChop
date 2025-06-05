using System.Collections.Generic;
using UnityEngine.InputSystem;
using System.Collections;
using UnityEngine;

public class OnBoardingHandler : Singleton<OnBoardingHandler>
{
    #region properties

    public System.Action OnTutorialEnd { get; set; }
    public int CurrentStep { get; private set; }
    public bool TutorialPlaying { get; private set; }

    #endregion
    #region SerializeField  

    [Header("Highlihght Objects"), Tooltip("This is sequentually organized.")]
    [SerializeField] private OutlineMaterial[] _highlightObjects;
    [SerializeField, Space(5f)] private GameObject _dirtyCollider; // should be deactivated by default

    [Header("Panels")]
    [SerializeField] private GameObject _friendlyTipPanel;
    [SerializeField] private GameObject _slicingPanel, _moldingPanel;

    [Header("Input Button Reference")]
    [SerializeField] public InputActionReference Continue;

    [Header("Debugging")]
    [SerializeField] private List<GameObject> _plates;
    [SerializeField] private bool _isDeveloperMode;

    #endregion
    #region Private 

    private bool _tutorialPlaying;
    private const float PANEL_TIMER = 30f, HIGHLIGHT_TIMER = 20f;
    private string[] _voiceLines = new string[9]
    {
        "onb 01", "onb 02", "onb 03",
        "onb 04", "onb 05", "onb 06",
        "onb 07", "onb 08", "onb 09"
    };
    private string[] _instructions = new string[9]
    {
        "Wash your hands at kitchen sink.",
        "Order one (1) Salmon Slab from Shop Screen near the freezers.",
        "Store the Salmon Slab and get the one inside the freezer.",
        "Chop Chop! the salmon slab!",
        "Obtain the perfect rice mold.",
        "Combine the rice mold and the fish slice!",
        "Serve the new customer!",
        "Cleaning time!",
        "Congratulations, you did it!!"
    };

    #endregion

    #region Unity

    protected override void OnApplicationQuit() => base.OnApplicationQuit();
    protected override void Awake()
    {
        base.Awake();

        _dirtyCollider.SetActive(false);
        CurrentStep = 0;
        TutorialPlaying = false;
    }
    private void Update() => Test(); 

    #endregion
    #region Tutorial

    public void PlayOnboarding()
    {
        if (_tutorialPlaying)
        {
            ReadOverlapError();
            return;
        }

        _tutorialPlaying = true;

        // plays the VOICE LINE and displays the INSTRUCTION for the tutorial
        SoundManager.Instance.PlayOnboarding(_voiceLines[CurrentStep]);
        PlayerHUD.Instance.txtTopHUDUpdate(_instructions[CurrentStep]);

        // some onboarding steps have extra actions
        DoExtraOnboarding(CurrentStep);

        StartCoroutine(CO_ToggleHighlight());
    }

    #endregion
    #region Helpers

    private void ReadOverlapError() => Debug.LogError("Onboarding is already playing!");
    public void Disable()
    {
        if (_plates.Count > 0)
            foreach (GameObject p in _plates)
                p.SetActive(false);

        OnTutorialEnd?.Invoke();
        gameObject.SetActive(false);
    }
    private void DoExtraOnboarding(int mode)
    {
        switch (mode)
        {
            case 0: SpawnManager.Instance.SpawnTutorialCustomer(true); break;
            case 3: StartCoroutine(CO_EnableSlicingPanel()); break;
            case 4: StartCoroutine(CO_EnableMoldingPanel()); break;
            case 6: SpawnManager.Instance.SpawnTutorialCustomer(false); break;
            case 7: _dirtyCollider.SetActive(true); break;

            case 8:
                GameManager.Instance.EnableEOD();
                StartCoroutine(CO_EnableFriendlyTipPanel());
                break;

            default: break;
        }
    }
    private void Test()
    {
        if (!_isDeveloperMode) return;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            PlayOnboarding();
            Debug.Log($"Current step: 0{CurrentStep + 1}");
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            CurrentStep++;
            Debug.LogWarning($"Incremented CurrentStep. New step: 0{CurrentStep + 1}");
        }
    }

    #endregion

    #region Enumerators

    private IEnumerator CO_EnableSlicingPanel()
    {
        _slicingPanel.SetActive(true);
        yield return new WaitForSeconds(PANEL_TIMER);
        _slicingPanel.SetActive(false);
    }
    private IEnumerator CO_EnableMoldingPanel()
    {
        _moldingPanel.SetActive(true);
        yield return new WaitForSeconds(PANEL_TIMER);
        _moldingPanel.SetActive(false);
    }
    private IEnumerator CO_EnableFriendlyTipPanel()
    {
        _friendlyTipPanel.SetActive(true);
        yield return new WaitForSeconds(PANEL_TIMER);
        _friendlyTipPanel.SetActive(false);
    }
    private IEnumerator CO_ToggleHighlight()
    {
        // highlights the object for a set amount of time
        _highlightObjects[CurrentStep].EnableHighlight();
        yield return new WaitForSeconds(HIGHLIGHT_TIMER);

        // Idk how to disable the highlight 
        _highlightObjects[CurrentStep].DisableHighlight();
        Debug.LogWarning("Disbled highlight");

        // progresses the onboarding to the next step
        if (_tutorialPlaying)
        {
            CurrentStep++;
            _tutorialPlaying = false;
            Debug.LogWarning($"New step: 0{CurrentStep + 1}");
        }
    }

    #endregion
}