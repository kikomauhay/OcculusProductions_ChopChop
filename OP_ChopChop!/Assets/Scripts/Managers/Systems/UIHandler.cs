using UnityEngine.SceneManagement;
using UnityEngine;

public class UIHandler : Singleton<UIHandler> {

    protected override void Awake() => base.Awake();
    protected override void OnApplicationQuit() => base.OnApplicationQuit();


    public void SwitchSceneTo(string name) 
    {
        // avoids restarting the scene by accident
        if (SceneManager.GetActiveScene().name != name)
            SceneManager.LoadSceneAsync(name);
    }

    public void Quit() 
    {
        Application.Quit();

        // debug is only here to test if quit actually works
        Debug.LogWarning("Quitted the game!");
    }



    

}
 