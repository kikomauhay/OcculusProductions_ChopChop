using UnityEngine.SceneManagement;
using UnityEngine;

public class UIHandler : Singleton<UIHandler> {

    protected override void Awake() { base.Awake(); }

    public void SwitchSceneTo(string name) 
    {
        // avoids restarting the scene by accident
        if (SceneManager.GetActiveScene().name != name)
            SceneManager.LoadSceneAsync(name);
    }

    public void Quit() 
    {
        Application.Quit();
        Debug.LogWarning("Quitted the game!");
    }


    protected override void OnApplicationQuit() { base.OnApplicationQuit(); }

    

}
 