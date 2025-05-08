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

#region Public

    public void CallNextTutorial()
    {
        if (_tutorialPlayed) return;

        if (_isTutorial)
        {
            StartCoroutine(OnBoardingHandler.Instance.Onboarding05());
            _tutorialPlayed = true;
        }         
    }
    public override void HitTheGround()
    {
        base.HitTheGround();

        SoundManager.Instance.PlaySound(Random.value > 0.5f ? 
                                        "knife dropped 01" : 
                                        "knife dropped 02",
                                        SoundGroup.EQUIPMENT);
    }
#endregion
#region Helpers

    private void CheckMaterial() 
    {
        if (IsClean)
            _rend.material = _cleanMat;
        
        else         
            _rend.materials = new Material[] { _dirtyMat, _dirtyOSM };        
    }

#endregion
}
