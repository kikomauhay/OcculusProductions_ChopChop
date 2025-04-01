using UnityEngine;

public class Plate : Equipment
{
#region Members

    public bool IsPlated { get; private set; }

    [Tooltip("The Box Collider Component")] 
    [SerializeField] Collider _boxTrigger;

    public Collider BoxTrigger => _boxTrigger;

#endregion

#region Unity_Methods

    protected override void Start()
    {
        base.Start();

        name = "Plate";
        _maxUsageCounter = 1;

        IsPlated = false;
        _boxTrigger.enabled = true;
    }
    
    protected override void OnCollisionEnter(Collision other)
    {
        base.OnCollisionEnter(other);
        SoundManager.Instance.PlaySound(Random.value > 0.5f ?
                                        "plate placed 01" : "plate placed 02",
                                        SoundGroup.EQUIPMENT);
    } 

#endregion

    public void TogglePlated() => IsPlated = !IsPlated;
    protected override void DoCleaning()
    {
        base.DoCleaning();

        if (_boxTrigger == null) return;

        if (!IsClean && !IsPlated)
            _boxTrigger.enabled = true;

        else
            _boxTrigger.enabled = false;
    }
}
