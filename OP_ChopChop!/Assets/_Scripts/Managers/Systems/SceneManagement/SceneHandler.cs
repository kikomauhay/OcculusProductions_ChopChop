using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine;

public class SceneHandler : Singleton<SceneHandler>
{
#region Readers

    public bool IsFadingIn { get; private set; }
    public bool IsFadingOut { get; private set; }
    public bool CanPause { get; private set; }

#endregion

#region Members

    [SerializeField] FadeScreen _fadeScreen;
    [SerializeField] string _sceneName;

#endregion

#region Unity_Methods

    protected override void Awake() => base.Awake();
    
    protected override void OnApplicationQuit() => base.OnApplicationQuit();
    void Start() 
    {
        CanPause = true;
        IsFadingIn = false;
        IsFadingOut = false;
                
        SceneManager.LoadSceneAsync(_sceneName, LoadSceneMode.Additive);

        if (_sceneName == "MainGameScene")
            GameManager.Instance.ChangeShift(GameShift.PRE_SERVICE);
    }

#endregion

    public IEnumerator LoadScene(string sceneName)
    {
        _fadeScreen.FadeOut();
        yield return new WaitForSeconds(_fadeScreen.FadeDuration);

        if (sceneName == "MainGameScene") 
        {
            SceneManager.LoadSceneAsync("MainGameScene", LoadSceneMode.Additive);
            yield return null;          
            SceneManager.UnloadSceneAsync("TrainingScene");
        }
        else if (sceneName == "TrainingScene")
        {
            SceneManager.LoadSceneAsync("TrainingScene", LoadSceneMode.Additive);
            yield return null;
            SceneManager.UnloadSceneAsync("MainGameScene");
        }
        else
        {
            SoundManager.Instance.PlaySound("wrong", SoundGroup.GAME);
            Debug.LogError("Wrong scene named!");
        } 

        _fadeScreen.FadeIn();
        yield return new WaitForSeconds(_fadeScreen.FadeDuration);
    }
}
