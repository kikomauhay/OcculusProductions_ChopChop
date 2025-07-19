using System.Collections;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(Trashable))]
public abstract class Equipment : MonoBehaviour 
{
    #region Properties

    public bool IsClean => _isClean;
    public Material DirtyMaterial => _dirtyMat;

    #endregion
    #region Protected 

    [SerializeField] protected bool _isClean;
    [SerializeField] protected Material _dirtyOSM, _cleanMat, _dirtyMat;
    protected Vector3 _startPosition;
    protected Renderer _rend;
    protected XRGrabInteractable _interactable;

    // DIRTY MECHANIC
    [SerializeField] protected int _maxUsageCounter; // max counter before it gets dirty
    protected int _usageCounter;                     // counter to know how many times equipment has been used

    [Header("Debugging")]
    [SerializeField] protected bool _isDeveloperMode;

    #endregion
    #region Private 
    
    private bool _coroutineRunning;

    #endregion

    #region Unity

    protected virtual void Awake()
    {
        _rend = GetComponent<Renderer>();
        _interactable = GetComponent<XRGrabInteractable>();

        if (_isDeveloperMode)
            Debug.LogWarning($"{this} is developer mode: {_isDeveloperMode}");

        if (_interactable == null)
            Debug.LogWarning($"Null reference for {_interactable}");
    }
    protected virtual void Start() 
    {
        GameManager.Instance.OnStartService += ResetPosition;
        OnBoardingHandler.Instance.OnTutorialEnd += ResetPosition;

        _isClean = true;
        _coroutineRunning = false;
        _startPosition = transform.position;

        _usageCounter = 0;

        if (_maxUsageCounter == 0)
            Debug.LogError($"Current max use for {this} is 0");

        if (_interactable != null) 
            HandManager.Instance.RegisterGrabbable(_interactable);
    }
    protected virtual void Update() => Test();
    protected virtual void OnDestroy() 
    {
        GameManager.Instance.OnStartService -= ResetPosition;
        OnBoardingHandler.Instance.OnTutorialEnd -= ResetPosition;
    }
    protected virtual void OnTriggerEnter(Collider other) // CLEANING MECHANIC
    {
        Sponge sponge = other.gameObject.GetComponent<Sponge>();
        
        if (sponge == null) return;

        if (sponge.IsClean || sponge.IsWet)
            DoCleaning(sponge);
    }
    protected virtual void OnTriggerExit(Collider other)
    {
        Sponge sponge = other.gameObject.GetComponent<Sponge>();
        
        if (sponge == null) return;

        if (_coroutineRunning)
        {
            StopCoroutine(CO_Clean(sponge));
            _coroutineRunning = false;
        }
    }
    protected void OnCollisionEnter(Collision other)
    {
        // equipment -> ingredient
        if (other.gameObject.GetComponent<Ingredient>() != null)
        {
            Ingredient ing = other.gameObject.GetComponent<Ingredient>();

            if (!_isClean)
            {
                ing.SetMoldy();
                Debug.LogWarning($"{name} contaminated {ing.name}");
                return;
            }
            else if (!ing.IsFresh) 
            {
                SetDirty();
                Debug.LogWarning($"{ing.name} contaminated {name}");
                return;
            }
        }

        // equipment -> food
        if (other.gameObject.GetComponent<UPD_Food>() != null)
        {
            UPD_Food food = other.gameObject.GetComponent<UPD_Food>();

            if (!_isClean)
            {
                food.SetMoldy();
                Debug.LogWarning($"{name} contaminated {food.name}");
                return;
            }
            else if (food.Condition != FoodCondition.CLEAN)
            {
                SetDirty();
                Debug.LogWarning($"{food.name} contaminated {name}");
                return;
            }
        }

        // equipment -> another equipment
        if (other.gameObject.GetComponent<Equipment>() != null)
        {
            Equipment eq = other.gameObject.GetComponent<Equipment>();

            if (!_isClean)
            {
                eq.SetDirty();
                Debug.LogWarning($"{name} contaminated {eq.name}");
                return;
            }
            else if (!eq.IsClean)
            {
                SetDirty();
                Debug.LogWarning($"{eq.name} contaminated {name}");
                return;
            }
        }

        if (other.gameObject.GetComponent<Sponge>() != null) 
        {
            Sponge sponge = other.gameObject.GetComponent<Sponge>();

            if (!_isClean && sponge.IsWet && sponge.IsClean) 
            {
                //insert clean logic here
                SetClean(sponge);
                Debug.LogWarning($"{sponge.name} cleaned {name}");
            }
        }
    }

    protected virtual void Test()
    {
        if (Input.GetKeyDown(KeyCode.Space) && _isDeveloperMode)
        {
            SetDirty();
            Debug.Log($"{name} is dirty!");
        }
    }

    #endregion
    #region Public

    public virtual void HitTheGround()
    {
        SetDirty();
        ResetPosition();
    }
    public virtual void Trashed()
    {
        SetDirty();
        ResetPosition();
    }
    public virtual void PickUpEquipment() {}
    public void IncrementUseCounter()
    {
        _usageCounter++;

        if (_usageCounter == _maxUsageCounter)
            SetDirty();
    }
    public void SetDirty()
    {
        _usageCounter = _maxUsageCounter;
        _isClean = false;
        _rend.materials = new Material[] { _dirtyMat, _dirtyOSM };
    }

    #endregion
    #region Helpers

    protected void ResetPosition() 
    {
        transform.position = _startPosition;
        transform.rotation = Quaternion.identity;
    }
    protected void SetClean(Sponge sponge)
    {
        _usageCounter = 0;
        _isClean = true;
        _rend.materials = new Material[] { _cleanMat };
        _coroutineRunning = false;
        sponge.SetDirty();
    }
    protected virtual void DoCleaning(Sponge sponge)
    {
        SpawnManager.Instance.SpawnVFX(VFXType.BUBBLE, transform, 5f);
        
        if (_coroutineRunning)
            StopCoroutine(CO_Clean(sponge));
        
        StartCoroutine(CO_Clean(sponge));
    }

    #endregion

    #region Enumerators

    protected IEnumerator CO_Clean(Sponge sponge)
    {
        _coroutineRunning = true;
        yield return new WaitForSeconds(2f);
        SetClean(sponge);
    }

    #endregion
}
