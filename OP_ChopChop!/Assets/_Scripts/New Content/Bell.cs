using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine;

public class Bell : XRBaseInteractable
{

    private GameManager _gameMgr;

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
        if (Input.GetKeyUp(KeyCode.Backspace))
        {
            Keyboard_BellTrigger();
        }
    }

    #endregion

    #region Private Functions

    private void BellTrigger(HoverEnterEventArgs args)
    {
        // to prevent going back to training mid service
        if (_gameMgr.CurrentShift == GameShift.Service) return;

        if (_gameMgr.CurrentShift == GameShift.Training)
        {
            OnBoardingHandler.Instance.Disable();
            Debug.LogWarning("Tutorial disabled!");

            SoundManager.Instance.PlaySound("change shift");
            StartCoroutine(SceneHandler.Instance.LoadScene("MainGameScene"));

            _gameMgr.ChangeShift(GameShift.PreService);
            _gameMgr.TutorialDone = true;
            Debug.LogWarning("Loading to MGS");
        }
        else
        {
            _gameMgr.ChangeShift(GameShift.Training);
            SoundManager.Instance.PlaySound("change shift");
            StartCoroutine(SceneHandler.Instance.LoadScene("TrainingScene"));
            Debug.LogWarning("Loading to TRS");
        }

        ShopManager.Instance.ClearList();
        SoundManager.Instance.StopOnboarding(); // in case there is any ongoing tutorial lines
    }
    private void Keyboard_BellTrigger()
    {
       // to prevent going back to training mid service
        if (_gameMgr.CurrentShift == GameShift.Service) return;

        if (_gameMgr.CurrentShift == GameShift.Training)
        {
            OnBoardingHandler.Instance.Disable();
            Debug.LogWarning("Tutorial disabled!");

            SoundManager.Instance.PlaySound("change shift");
            StartCoroutine(SceneHandler.Instance.LoadScene("MainGameScene"));

            _gameMgr.ChangeShift(GameShift.PreService);
            _gameMgr.TutorialDone = true;
            Debug.LogWarning("Loading to MGS");
        }
        else
        {
            _gameMgr.ChangeShift(GameShift.Training);
            SoundManager.Instance.PlaySound("change shift");
            StartCoroutine(SceneHandler.Instance.LoadScene("TrainingScene"));
            Debug.LogWarning("Loading to TRS");
        }

        ShopManager.Instance.ClearList();
        SoundManager.Instance.StopOnboarding(); // in case there is any ongoing tutorial lines
    }

#endregion
}
