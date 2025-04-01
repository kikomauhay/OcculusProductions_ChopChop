using System.Collections;
using UnityEngine;

/// <summary>
/// 
/// Acts as the tutorial for the player
/// Makes sure that the player doens't break the sequence of the onboarding  
/// 
/// </summary>

public class OnBoardingHandler : Singleton<OnBoardingHandler> 
{
    // int _tutorialIndex = 0;

    protected override void Awake() => base.Awake();
    protected override void OnApplicationQuit() => base.OnApplicationQuit();


    void Start() => StartCoroutine(ChangeToMainGame());
    // void Update() => test();    
    void test()
    {
        if (Input.GetKeyDown(KeyCode.Return) && !SceneHandler.Instance.IsFading)
        {
            StartCoroutine(SceneHandler.Instance.LoadScene("MainGameScene"));
            GameManager.Instance.ChangeShift(GameShift.PRE_SERVICE);
        }
    }

    IEnumerator ChangeToMainGame()
    {
        float time = 5f;

        Debug.Log($"waiting {time}s to change to main game scene");

        yield return new WaitForSeconds(time);
        StartCoroutine(SceneHandler.Instance.LoadScene("MainGameScene"));
        GameManager.Instance.ChangeShift(GameShift.PRE_SERVICE);
    } 
}
