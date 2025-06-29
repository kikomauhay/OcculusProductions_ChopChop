using UnityEngine.InputSystem;
using System.Collections;
using UnityEngine;
using System;

public class OnBoardingHandler : Singleton<OnBoardingHandler>
{
    #region Properties

    public System.Action OnTutorialEnd { get; set; }
    public int CurrentStep { get; private set; }
    public bool IsTutorialPlaying { get; private set; }

    #endregion
    #region SerializeField  

    [Header("Highlihght Objects"), Tooltip("This is sequentually organized.")]
    [SerializeField] private OutlineMaterial[] _highlightObjects;
    [SerializeField, Space(5f)] private GameObject _dirtyCollider; // should be deactivated by default

    [Header("Panels")]
    [SerializeField] private GameObject _friendlyTipPanel;
    [SerializeField] private GameObject _slicingPanel, _moldingPanel;
    [SerializeField] private PlayerHUD _playerHUD;

    [Header("Input Button Reference")]
    [SerializeField] public InputActionReference Continue;

    [Header("Debugging")]
    [SerializeField] private bool _isDeveloperMode;

    #endregion
    #region Private 

    private bool _isTutorialPlaying;
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
        "Order one (1) Tuna Slab from Shop Screen near the freezers.",
        "Store the Tuna Slab in the freezer and get the Salmon Slab.",
        "Chop Chop! the salmon slab!",
        "Mold the rice three (3) times.",
        "Combine the rice mold and the salmon slice!",
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
        IsTutorialPlaying = false;

        if (_isDeveloperMode)
            Debug.Log($"{this} is developer mode: {_isDeveloperMode}");
    }
    private void Update() => Test();

    #endregion
    #region Tutorial

    public void PlayOnboarding()
    {
        if (_isTutorialPlaying)
        {
            ReadOverlapError();
            return;
        }

        _isTutorialPlaying = true;

        // plays the VOICE LINE and displays the INSTRUCTION for the tutorial
        SoundManager.Instance.PlayOnboarding(_voiceLines[CurrentStep]);
        _playerHUD.txtTopHUDUpdate(_instructions[CurrentStep]);

        // some onboarding steps have extra actions
        DoExtraOnboarding(CurrentStep);
        StartCoroutine(CO_ToggleHighlight());

        Debug.Log($"{this} is playing Onb 0{CurrentStep}");
    }

    public void AddOnboardingIndex()
    {
        if (_isTutorialPlaying)
        {
            CurrentStep++;
            _isTutorialPlaying = false;
        }
    }

    #endregion
    #region Helpers

    private void ReadOverlapError() => Debug.LogError("Onboarding is already playing!");
    public void Disable()
    {
        OnTutorialEnd?.Invoke();
        gameObject.SetActive(false);
        SoundManager.Instance.StopOnboarding();
        _playerHUD.enabled = false;
    }

    private void DoExtraOnboarding(int mode)
    {
        switch (mode)
        {
            case 0: SpawnManager.Instance.SpawnTutorialCustomer(true); break;
            case 3: StartCoroutine(CO_EnableSlicingPanel()); break;
            case 4: StartCoroutine(CO_EnableMoldingPanel()); break;
            case 6: StartCoroutine(CO_SpawnBenny()); break;
            case 7: _dirtyCollider.SetActive(true); break;

            case 8:
                GameManager.Instance.EnableEOD();
                StartCoroutine(CO_EnableFriendlyTipPanel());
                GameManager.Instance.TutorialDone = true;

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
        if (Input.GetKeyDown(KeyCode.Escape))
            Debug.Log($"{this} current step: 0{CurrentStep}");

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            CurrentStep = 0;
            _isTutorialPlaying = false;
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            CurrentStep = 1;
            _isTutorialPlaying = false;
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            CurrentStep = 2;
            _isTutorialPlaying = false;
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            CurrentStep = 3;
            _isTutorialPlaying = false;
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            CurrentStep = 4;
            _isTutorialPlaying = false;
        }
        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            CurrentStep = 5;
            _isTutorialPlaying = false;
        }
        if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            CurrentStep = 6;
            _isTutorialPlaying = false;
        }
        if (Input.GetKeyDown(KeyCode.Alpha8))
        {
            CurrentStep = 7;
            _isTutorialPlaying = false;
        }
        if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            CurrentStep = 8;
            _isTutorialPlaying = false;
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
        _playerHUD.enabled = false;
    }
    private IEnumerator CO_ToggleHighlight()
    {
        // highlights the object for a set amount of time
        _highlightObjects[CurrentStep].EnableHighlight();
        yield return new WaitForSeconds(HIGHLIGHT_TIMER);

        // Idk when I should disable the highlight 
        _highlightObjects[CurrentStep].DisableHighlight();
    }

    private IEnumerator CO_SpawnBenny()
    {
        // 1 sec longer so that Atrium can despawn properly
        yield return new WaitForSeconds(10f);
        SpawnManager.Instance.SpawnTutorialCustomer(false);
    }

    #endregion
}