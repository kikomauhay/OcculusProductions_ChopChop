using UnityEngine;

/// <summary>
/// 
/// Acts as the tutorial for the player
/// Makes sure that the player doens't break the sequence of the onboarding  
/// 
/// </summary>

public class OnBoardingHandler : Singleton<OnBoardingHandler> 
{
    protected override void Awake() => base.Awake();
    protected override void OnApplicationQuit() => base.OnApplicationQuit();

    int _tutorialIndex = 0;
}
