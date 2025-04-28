using UnityEngine;

public class Knife : Equipment
{
    [SerializeField] private bool _isTutorial;

    public void CallNextTutorial()
    {
        if (_isTutorial)         
            StartCoroutine(OnBoardingHandler.Instance.Onboarding05());
    }
}
