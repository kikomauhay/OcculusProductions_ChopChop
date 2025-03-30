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
    
    [SerializeField] Material _fadeOutMaterial;
    [SerializeField] Color _fadeOutStartColor;

    [Range(0.1f, 10f)] // default values => 5f 
    [SerializeField] float _fadeInSpeed, _fadeOutSpeed;  

#endregion

#region Unity_Methods

    protected override void Awake() 
    {
        base.Awake();
        _fadeOutStartColor.a = 0f;
    }
    protected override void OnApplicationQuit() => base.OnApplicationQuit();
    void Start() 
    {
        CanPause = true;
        IsFadingIn = false;
        IsFadingOut = false;
        
        SceneManager.LoadSceneAsync("TrainingScene", LoadSceneMode.Additive);
        Debug.Log("1. went to training scene");
    }
        
    void Update()
    {
        test();

        if (IsFadingOut)
        {
            if (_fadeOutStartColor.a < 1f)
            {
                _fadeOutStartColor.a += Time.deltaTime * _fadeOutSpeed;
                _fadeOutMaterial.color = _fadeOutStartColor;
            }
            else IsFadingOut = false;
        } 

        if (IsFadingIn)
        {
            if (_fadeOutStartColor.a > 0f)
            {
                _fadeOutStartColor.a -= Time.deltaTime * _fadeInSpeed;
                _fadeOutMaterial.color = _fadeOutStartColor;
            }
            else IsFadingIn = false;
        } 
    }

    void test() 
    {
        if (Input.GetKeyDown(KeyCode.Space))
            StartCoroutine("LoadMainGameScene");
    }

#endregion

#region Scene_Transitions

    void FadeOut()
    {
        _fadeOutMaterial.color = _fadeOutStartColor;
        IsFadingOut= true;
    }

    void FadeIn()
    {
        if (_fadeOutMaterial.color.a >= 1f)
        {
            _fadeOutMaterial.color = _fadeOutStartColor;
            IsFadingIn = true;
        }
    }

#endregion


    /* -OLD FADE IN CODE- 
    IEnumerator CO_FadeIn()
    {
        float startAlpha = _fadeOutMaterial.color.a;
        float currentAlpha = _fadeOutMaterial.color.a;
        Color color = _fadeOutMaterial.color;

        // fade in 
        while (currentAlpha < 1f)
        {
            color.a = Mathf.Lerp(startAlpha, currentAlpha, Time.deltaTime);
            _fadeOutMaterial.color = color;
        }


        yield return null;
    }

    IEnumerator FadeOutThenChangeScene()
    {
        yield break;
    }
    IEnumerator LoadMainGameScene()
    {
        FadeOut();

        while (IsFadingOut)
            yield return null;

        SceneManager.LoadSceneAsync("GaniScene");
        Debug.LogWarning("went to gani scene");
    } */

    public IEnumerator LoadScene(string sceneName)
    {
        FadeOut();

        while (IsFadingOut)
            yield return null;        

        if (sceneName == "MainGameScene") 
        {
            SceneManager.LoadSceneAsync("MainGameScene", LoadSceneMode.Additive);
            yield return null;          
            SceneManager.UnloadSceneAsync("TrainingScene");
        }
        else if (sceneName == "TrainingScene")
        {
            SceneManager.UnloadSceneAsync("MainGameScene");
            yield return null;
            SceneManager.LoadSceneAsync("TrainingScene", LoadSceneMode.Additive);
        }
        else
        {
            SoundManager.Instance.PlaySound("wrong", SoundGroup.GAME);
            Debug.LogError("Wrong scene named!");
        }
    }
}
