using UnityEngine;

public class Knife : Equipment
{
    [SerializeField] private bool _isTutorial;
    private bool _tutorialPlayed;

    public void CallNextTutorial()
    {
        if (_tutorialPlayed) return;

        if (_isTutorial)
        {
            StartCoroutine(OnBoardingHandler.Instance.Onboarding05());
            _tutorialPlayed = true;
        }         
    }
}
