using UnityEngine;

public class Knife : Equipment
{

    [SerializeField] private bool _isTutorial;

    protected override void Start() 
    {
        base.Start();
        name = "Knife";
    }

    public void CallNextTutorial()
    {
        if (!_isTutorial) return;

        GetComponent<OutlineMaterial>().DisableHighlight();
        StartCoroutine(OnBoardingHandler.Instance.CallOnboarding(4));
    }
}
