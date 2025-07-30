using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine;
using System;

public class Bell : XRBaseInteractable
{
    #region Members
    private GameManager _gameMgr;
    private OnBoardingHandler _onbHandler;
    private SpawnManager _spawnMgr;

    [SerializeField] private bool _isDeveloperMode;
    [SerializeField] private Animator _bellShake;

    #endregion

    #region Unity

    protected override void OnEnable()
    {
        base.OnEnable();
        hoverEntered.AddListener(BellTrigger);
    }
    protected override void OnDisable()
    {
        base.OnDisable();
        hoverEntered.RemoveListener(BellTrigger);
    }
    private void Start() 
    {
        _bellShake = GetComponent<Animator>();
        _gameMgr = GameManager.Instance;
        _onbHandler = OnBoardingHandler.Instance;
        _spawnMgr = SpawnManager.Instance;
    }
    private void Update()
    {
        if (!_isDeveloperMode) return;

        if (Input.GetKeyUp(KeyCode.Backspace))
        {
            Keyboard_BellTrigger();
        }
    }

    #endregion

    #region Private Functions

    private void BellTrigger(HoverEnterEventArgs args)
    {
        if (_gameMgr.CurrentShift == GameShift.Training)
        {
            // when you press the bell in TRS, the tutorial stops and you immediately go to MGS
            // Debug.LogWarning("Tutorial disabled!");

            // UX for the scene change
            SoundManager.Instance.PlaySound("change shift");

            // other triggers to remove any tutorial logic
            _onbHandler.OnTutorialEnd?.Invoke();
            OnBoardingHandler.Instance.Disable();
            StartCoroutine(SceneHandler.Instance.LoadScene("MainGameScene"));
            _gameMgr.ChangeShift(GameShift.PreService);
            
            // Debug.LogWarning("Loading to MGS");
        }
        else if (_gameMgr.CurrentShift != GameShift.Default) // player can now go back to training after pressing the bell
        {
            _gameMgr.ChangeShift(GameShift.Training);
            _gameMgr.OnEndService?.Invoke();
            SoundManager.Instance.PlaySound("change shift");
            StartCoroutine(SceneHandler.Instance.LoadScene("TrainingScene"));
            
            // Debug.LogWarning("Loading to TRS");
        }
    }
    private void Keyboard_BellTrigger()
    {
        // Debug.LogWarning("Bell Shake");
        // _bellShake.SetTrigger("PlayBellShake");

        if (_gameMgr.CurrentShift == GameShift.Training)
        {
            // when you press the bell in TRS, the tutorial stops and you immediately go to MGS
            OnBoardingHandler.Instance.Disable();
            Debug.LogWarning("Tutorial disabled!");

            // UX for the scene change
            SoundManager.Instance.PlaySound("change shift");
            StartCoroutine(SceneHandler.Instance.LoadScene("MainGameScene"));

            // other triggers to remove any tutorial logic
            _gameMgr.ChangeShift(GameShift.PreService);
            _onbHandler.OnTutorialEnd?.Invoke();
            // Debug.LogWarning("Loading to MGS");
        }
        else // player can now go back to training after pressing the bell
        {
            _gameMgr.ChangeShift(GameShift.Training);
            SoundManager.Instance.PlaySound("change shift");
            StartCoroutine(SceneHandler.Instance.LoadScene("TrainingScene"));

            // Debug.LogWarning("Loading to TRS");
        }
    }
    #endregion
}
