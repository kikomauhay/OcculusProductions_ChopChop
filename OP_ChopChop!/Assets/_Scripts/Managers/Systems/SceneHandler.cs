using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine;
using Unity.VisualScripting;

public class SceneHandler : Singleton<SceneHandler> 
{

    [SerializeField] Material _material;

    protected override void Awake() => base.Awake();
    protected override void OnApplicationQuit() => base.OnApplicationQuit();

    void Start() 
    {
        SceneManager.LoadSceneAsync("MainGameScene", LoadSceneMode.Additive);
        Debug.LogWarning("went to main game scene!");
    }

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

    IEnumerator CO_FadeIn()
    {
        float startAlpha = _material.color.a;
        float currentAlpha = _material.color.a;
        Color color = _material.color;

        // fade in 
        while (currentAlpha < 1f)
        {
            color.a = Mathf.Lerp(startAlpha, currentAlpha, Time.deltaTime);
            _material.color = color;
        }


        yield return null;






    }

    IEnumerator CO_TrainingScene()
    {
        yield return null;
    }


}
