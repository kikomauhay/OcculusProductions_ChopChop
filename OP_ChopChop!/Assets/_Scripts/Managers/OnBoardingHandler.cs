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
    int _tutorialIndex = 0;

    protected override void Awake() => base.Awake();
    protected override void OnApplicationQuit() => base.OnApplicationQuit();
    void Start() 
    {
        GameManager.Instance.ChangeShift(GameShift.TRAINING);
        // StartCoroutine(ChangeToMainGame());
    }
    IEnumerator ChangeToMainGame()
    {
        Debug.Log("waiting 5s to change to main game scene");

        yield return new WaitForSeconds(5f);
        StartCoroutine(SceneHandler.Instance.LoadScene("MainGameScene"));
        GameManager.Instance.ChangeShift(GameShift.PRE_SERVICE);
        
        Debug.Log("2. went to main game scene");
    } 
}
