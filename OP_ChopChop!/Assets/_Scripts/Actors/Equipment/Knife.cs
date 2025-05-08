using UnityEngine;

public class Knife : Equipment
{
    [SerializeField] private bool _isTutorial;
    private bool _tutorialPlayed = false;

#region Unity

    protected override void Start()
    {
        base.Start();
        InvokeRepeating("CheckMaterial", 1f, 1f);
    }
    protected override void OnDestroy()
    {
        base.OnDestroy();
        CancelInvoke("CheckMaterial");  
    }

#endregion

#region Helpers

    public void CallNextTutorial()
    {
        if (_tutorialPlayed) return;

        if (_isTutorial)
        {
            StartCoroutine(OnBoardingHandler.Instance.Onboarding05());
            _tutorialPlayed = true;
        }         
    }  
    private void CheckMaterial() 
    {
        if (IsClean)
            _rend.material = _cleanMat;
        
        else         
            _rend.materials = new Material[] { _dirtyMat, _outlineTexture };        
    }

#endregion
}
