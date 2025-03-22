using UnityEngine.SceneManagement;

public class SceneHandler : Singleton<SceneHandler> 
{
    protected override void Awake() => base.Awake();
    protected override void OnApplicationQuit() => base.OnApplicationQuit();
    
    public void GoToScene(string name) => 
        SceneManager.LoadSceneAsync(name);
}
