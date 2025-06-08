using UnityEngine;

public class SalmonIngredient : Ingredient
{
    private static bool _tutorialDone = false;

    protected override void Start()
    {
        base.Start();

        if (!_tutorialDone && _sliceIndex == 4)
        {
            Debug.Log("THIS IS RUNNING");
            OnBoardingHandler.Instance.AddOnboardingIndex();
            OnBoardingHandler.Instance.PlayOnboarding();
            _tutorialDone = true;
        }
    } 
    protected override void OnTriggerEnter(Collider other) { }


}