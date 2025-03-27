using UnityEngine.SceneManagement;

public class SceneHandler : Singleton<SceneHandler> 
{
    protected override void Awake() => base.Awake();
    protected override void OnApplicationQuit() => base.OnApplicationQuit();

    void Start() => 
        SceneManager.LoadSceneAsync("TrainingScene", LoadSceneMode.Additive);

    public void GoToMainScene()
    {
        SceneManager.UnloadSceneAsync("TrainingScene");
        SceneManager.LoadSceneAsync("MainGameScene", LoadSceneMode.Additive);
    }
    public void GoToTrainingScene()
    {
        SceneManager.UnloadSceneAsync("MainGameScene");
        SceneManager.LoadSceneAsync("TrainingScene", LoadSceneMode.Additive);
    }
}
