using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine;

public class Bell : XRBaseInteractable
{
    #region Members
    private GameManager _gameMgr;

    [SerializeField] private bool _isDeveloperMode;

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
    private void Start() => _gameMgr = GameManager.Instance;
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
            OnBoardingHandler.Instance.Disable();
            Debug.LogWarning("Tutorial disabled!");

            // UX for the scene change
            SoundManager.Instance.PlaySound("change shift");
            StartCoroutine(SceneHandler.Instance.LoadScene("MainGameScene"));


            _gameMgr.ChangeShift(GameShift.PreService);
            Debug.LogWarning("Loading to MGS");
        }
        else if (_gameMgr.CurrentShift == GameShift.Service)
        {
            _gameMgr.ChangeShift(GameShift.Training);
            SoundManager.Instance.PlaySound("change shift");
            StartCoroutine(SceneHandler.Instance.LoadScene("TrainingScene"));
            Debug.LogWarning("Loading to TRS");
        }
    }
    private void Keyboard_BellTrigger()
    {
        if (_gameMgr.CurrentShift == GameShift.Training)
        {
            // when you press the bell in TRS, the tutorial stops and you immediately go to MGS
            OnBoardingHandler.Instance.Disable();
            Debug.LogWarning("Tutorial disabled!");

            // UX for the scene change
            SoundManager.Instance.PlaySound("change shift");
            StartCoroutine(SceneHandler.Instance.LoadScene("MainGameScene"));

            _gameMgr.ChangeShift(GameShift.PreService);
            _gameMgr.DisableTutorial();
            Debug.LogWarning("Loading to MGS");
        }
        else if (_gameMgr.CurrentShift == GameShift.Service)
        {
            _gameMgr.ChangeShift(GameShift.Training);
            SoundManager.Instance.PlaySound("change shift");
            StartCoroutine(SceneHandler.Instance.LoadScene("TrainingScene"));
            Debug.LogWarning("Loading to TRS");
        }
    }
    

    #endregion
}
