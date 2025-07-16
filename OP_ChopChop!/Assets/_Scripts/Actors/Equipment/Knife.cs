using UnityEngine;

public class Knife : Equipment
{
    [SerializeField] private bool _isTutorial;

    #region Unity

        protected override void Start()
        {
            base.Start();
            InvokeRepeating("CheckMaterial", 1f, 1f);

            HandManager.Instance.SetKnife(this);
        }
        protected override void OnDestroy()
        {
            base.OnDestroy();
            CancelInvoke("CheckMaterial");  
        }
    protected override void OnTriggerEnter(Collider other)
    {
        if (!IsClean)
            base.OnTriggerEnter(other);
    }

    #endregion

    #region Public

    public override void HitTheGround()
        {
            base.HitTheGround();

            SoundManager.Instance.PlaySound(Random.value > 0.5f ? 
                                            "knife dropped 01" : 
                                            "knife dropped 02");
        }
        public override void PickUpEquipment() => SoundManager.Instance.PlaySound("knife grabbed");
    
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
