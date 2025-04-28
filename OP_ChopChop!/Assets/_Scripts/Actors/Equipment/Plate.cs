using UnityEngine;

public class Plate : Equipment
{
#region Members

    public bool IsPlated { get; private set; }
    public Collider BoxTrigger => _boxTrigger;


    [Tooltip("The Box Collider Component")] 
    [SerializeField] private Collider _boxTrigger;
    [SerializeField] private bool _isTutorial;

#endregion

#region Unity_Methods

    protected override void Start()
    {
        base.Start();

        name = "Plate";
        _maxUsageCounter = 1;

        if (transform.childCount > 1)
        {
            IsPlated = true;
            _boxTrigger.enabled = false;
        }
        else 
        {
            IsPlated = false;
            _boxTrigger.enabled = true;
        }

        Debug.Log($"Is clean: {IsClean}");
    }
    protected override void OnTriggerEnter(Collider other) 
    {
        base.OnTriggerEnter(other);

        if (!_isTutorial) return;
        
        Ingredient ing = other.gameObject.GetComponent<Ingredient>(); 

        if (ing != null)
        {
            if (ing.IngredientType != IngredientType.SALMON) return;

            GetComponent<OutlineMaterial>().DisableHighlight();
            StartCoroutine(OnBoardingHandler.Instance.Onboarding07());
        }
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
