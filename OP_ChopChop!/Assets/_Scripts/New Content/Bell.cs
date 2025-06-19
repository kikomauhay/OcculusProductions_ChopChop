using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine;

public class Bell : XRBaseInteractable 
{

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

#endregion

#region PrivateFunctions

    private void BellTrigger(HoverEnterEventArgs args)
    {
        // to prevent going back to training mid service
        if (GameManager.Instance.CurrentShift == GameShift.Service) return;

        if (GameManager.Instance.CurrentShift == GameShift.Training)
        {
            OnBoardingHandler.Instance.Disable();
            Debug.LogWarning("Tutorial disabled!");

            SoundManager.Instance.PlaySound("change shift");
            StartCoroutine(SceneHandler.Instance.LoadScene("MainGameScene"));

            GameManager.Instance.ChangeShift(GameShift.PreService);
            GameManager.Instance.TutorialDone = true;
            Debug.LogWarning("Loading to MGS");
        }
        else
        {
            GameManager.Instance.ChangeShift(GameShift.Training);
            SoundManager.Instance.PlaySound("change shift");
            StartCoroutine(SceneHandler.Instance.LoadScene("TrainingScene"));
            Debug.LogWarning("Loading to TRS");
        }

        ShopManager.Instance.ClearList();
        SoundManager.Instance.StopOnboarding(); // in case there is any ongoing tutorial lines
    }

#endregion
}
