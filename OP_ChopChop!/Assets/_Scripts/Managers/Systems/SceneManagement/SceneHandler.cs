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

    [SerializeField] FadeScreen _fadeScreen;

#endregion

#region Unity_Methods

    protected override void Awake() => base.Awake();
    
    protected override void OnApplicationQuit() => base.OnApplicationQuit();
    void Start() 
    {
        CanPause = true;
        IsFading = false;
                
        SceneManager.LoadSceneAsync("TrainingScene", LoadSceneMode.Additive);
        GameManager.Instance.ChangeShift(GameShift.TRAINING);
    }

#endregion

    public IEnumerator LoadScene(string sceneName)
    {
        IsFading = true;
        _fadeScreen.FadeOut();
        yield return new WaitForSeconds(_fadeScreen.FadeDuration);
        IsFading = false;

        if (sceneName == "MainGameScene") 
        {
            SceneManager.UnloadSceneAsync("TrainingScene");
            // yield return null;          
            SceneManager.LoadSceneAsync("MainGameScene", LoadSceneMode.Additive);
        }
        else if (sceneName == "TrainingScene")
        {
            SceneManager.UnloadSceneAsync("MainGameScene");
            // yield return null;
            SceneManager.LoadSceneAsync("TrainingScene", LoadSceneMode.Additive);
        }
        else
        {
            SoundManager.Instance.PlaySound("wrong", SoundGroup.GAME);
            Debug.LogError("Wrong scene named!");
        } 

        IsFading = true;
        _fadeScreen.FadeIn();
        yield return new WaitForSeconds(_fadeScreen.FadeDuration);
        IsFading = false;
    }
}
