using UnityEngine;

public class Plate : Equipment
{
#region Members

    public bool IsPlated { get; private set; }


    [Tooltip("The Box Collider Component")] 
    [SerializeField] private Collider _boxTrigger;
    [SerializeField] private bool _isTutorial;

    private const float Y_OFFSET = 0.025f;

#endregion

#region Unity_Methods

    protected override void Start()
    {
        base.Start();

        _maxUsageCounter = 1;

        if (transform.childCount > 1)
        {
            IsPlated = true;
            _boxTrigger.enabled = false;
            // InvokeRepeating("SnapToCenter", 1f, 1f);  
        }
        else 
        {
            IsPlated = false;
            _boxTrigger.enabled = true;
        }

        Debug.Log($"Is clean: {IsClean}");                  
    }
  
    protected override void OnCollisionEnter(Collision other)
    {
        base.OnCollisionEnter(other);
        SoundManager.Instance.PlaySound(Random.value > 0.5f ? 
                                        "plate placed 01" : 
                                        "plate placed 02");
    } 

#endregion

#region Helpers

    public void TogglePlated() => IsPlated = !IsPlated;
    private void SnapToCenter()
    {
        if (transform.childCount < 1)
        {
            CancelInvoke("SnapToCenter");
            return;
        }

        Vector3 updatedPosition = transform.position;
        updatedPosition = new Vector3 (updatedPosition.x, 
                                       updatedPosition.y + Y_OFFSET,
                                       updatedPosition.z);

        transform.rotation = Quaternion.identity;
        transform.GetChild(0).transform.position = updatedPosition;
    }

#endregion
}
