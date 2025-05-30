using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine;

public class SceneHandler : Singleton<SceneHandler>
{
#region Readers

    public bool IsFading { get; private set; }
    public bool CanPause { get; private set; }

#endregion

#region Members

    [SerializeField] private FadeScreen _fadeScreen;
    [SerializeField] private string _sceneName;

#endregion

#region Unity

    protected override void Awake() => base.Awake();    
    protected override void OnApplicationQuit() => base.OnApplicationQuit();
    
    private void Start() 
    {
        CanPause = true;
        IsFading = false;
                
        SceneManager.LoadSceneAsync(_sceneName, LoadSceneMode.Additive);
        GameManager.Instance.ChangeShift(_sceneName == "TrainingScene" ? 
                                         GameShift.Training : 
                                         GameShift.PreService);
    }

#endregion

    public IEnumerator LoadScene(string sceneName)
    {
        _fadeScreen.gameObject.SetActive(true);
        IsFading = true;
        _fadeScreen.FadeOut();
        yield return new WaitForSeconds(_fadeScreen.FadeDuration);
        IsFading = false;

        if (sceneName == "MainGameScene") 
        {
            SceneManager.UnloadSceneAsync("TrainingScene");
            SceneManager.LoadSceneAsync("MainGameScene", LoadSceneMode.Additive);
        }
        else if (sceneName == "TrainingScene")
        {
            SceneManager.UnloadSceneAsync("MainGameScene");
            SceneManager.LoadSceneAsync("TrainingScene", LoadSceneMode.Additive);
        }
        else
        {
            SoundManager.Instance.PlaySound("wrong");
            Debug.LogError("Wrong scene named!");
        } 

        IsFading = true;
        _fadeScreen.FadeIn();
        yield return new WaitForSeconds(_fadeScreen.FadeDuration);
        IsFading = false;
    }
}
