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
            SoundManager.Instance.PlaySound("change shift");

            _onbHandler.OnTutorialEnd?.Invoke();
            OnBoardingHandler.Instance.Disable();
            StartCoroutine(SceneHandler.Instance.LoadScene("MainGameScene"));
            _gameMgr.ChangeShift(GameShift.PreService);
        }
        else if (_gameMgr.CurrentShift != GameShift.Default) // player can only go back to training when it's game over
        {
            if (!GameManager.Instance.IsGameOver)
            {
                SoundManager.Instance.PlaySound("wrong");
                return;
            }

            _gameMgr.ChangeShift(GameShift.Training);
            _gameMgr.OnEndService?.Invoke();
            SoundManager.Instance.PlaySound("change shift");
            StartCoroutine(SceneHandler.Instance.LoadScene("TrainingScene"));

        }
    }
    private void Keyboard_BellTrigger()
    {
        // Debug.LogWarning("Bell Shake");
        // _bellShake.SetTrigger("PlayBellShake");

        if (_gameMgr.CurrentShift == GameShift.Training)
        {
            OnBoardingHandler.Instance.Disable();
            SoundManager.Instance.PlaySound("change shift");
            StartCoroutine(SceneHandler.Instance.LoadScene("MainGameScene"));

            _gameMgr.ChangeShift(GameShift.PreService);
            _onbHandler.OnTutorialEnd?.Invoke();
        }
        else // player can only go back to training when it's game over
        {
            if (!_gameMgr.IsGameOver)
            {
                SoundManager.Instance.PlaySound("wrong");
                return;
            }

            _gameMgr.ChangeShift(GameShift.Training);
            SoundManager.Instance.PlaySound("change shift");
            StartCoroutine(SceneHandler.Instance.LoadScene("TrainingScene"));
        }
    }
    #endregion
}
