using UnityEngine;

public class Bell : StaticInstance<Bell> 
{
    private void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.name.Contains("LeftHand") ||
            !other.gameObject.name.Contains("RightHand"))
        {
            return;
        }

        if (GameManager.Instance.CurrentShift == GameShift.Training)
        {
            StartCoroutine(SceneHandler.Instance.LoadScene("MainGameScene"));

            OnBoardingHandler.Instance.Disable();
            Debug.LogWarning("Tutorial disabled!");

            GameManager.Instance.ChangeShift(GameShift.PreService);
            Debug.LogWarning("Loading to MGS");
        }
        else
        {
            StartCoroutine(SceneHandler.Instance.LoadScene("TrainingScene"));
            Debug.LogWarning("Loading to TRS");
        }

        ShopManager.Instance.ClearList();
        SoundManager.Instance.StopAllSounds(); // in case there is any ongoing tutorial lines
    }
}
