using System.Collections;
using UnityEngine;
using System;

public class KitchenCleaningManager : Singleton<KitchenCleaningManager>
{
    #region Properties

    // Standardized script for activating colliders on the hand, will draft it up and will ask help from isagani to clean up the code later
    public Action OnCleanedArea { get; set; }
    public Action OnStartDecayAgain { get; set; }
    public float KitchenScore { get; private set; } // overall cleanliness meter of the kitchen
    public int HandUsageCounter { get; private set; }
    public int MaxDirtyColliders
    {
        get => _maxDirtyColliders;
        set
        {
            if (value > 4)
                _maxDirtyColliders = 4;

            else if (value < 1)
                _maxDirtyColliders = 1;

            else
                _maxDirtyColliders = value;
        }
    }

    #endregion
    #region Members

    [SerializeField] private GameObject[] _dirtyColliders;
    private const float ONE_MINUTE = 60f;
    private float _decayTimer, _decayRate, _cleanlinessThreshold;
    private int _maxDirtyColliders;
    private bool _canClean;

    #endregion

    #region Unity

    protected override void Awake() => base.Awake();
    protected override void OnApplicationQuit() => base.OnApplicationQuit();
    private void Start()
    {
        OnCleanedArea += IncreaseCleanRate;
        GameManager.Instance.OnEndService += StopAllCoroutines;

        KitchenScore = 85f; // so that even if there's only 1 dirty collider (needs balance testing)
        HandUsageCounter = 30;
        _decayTimer = 5f;
        _decayRate = 5f;
        _maxDirtyColliders = 1;
        _cleanlinessThreshold = 80f; // kitchen needs to go below this score to start cleaning 
        _canClean = false;           // prevents the player from cleaning too much

        if (_maxDirtyColliders < 1)
            Debug.LogWarning($"There will only be {_maxDirtyColliders} colliders available!");  
    }
    private void OnDestroy()
    {
        OnCleanedArea -= IncreaseCleanRate;
        GameManager.Instance.OnEndService -= StopAllCoroutines;
    }

    #endregion
    #region Pubilc

    public void EnableRandomColliders()
    {
        int counter = 0;

        if (MaxDirtyColliders == 4)
        {
            EnableColliders(true);
            return;
        }

        do
        {
            for (int i = 0; i < _dirtyColliders.Length; i++)
            {
                if (counter == MaxDirtyColliders) break;

                if (UnityEngine.Random.value > 0.5f &&
                    !_dirtyColliders[i].activeSelf)
                {
                    _dirtyColliders[i].SetActive(true);
                    counter++;
                }
            }
        }
        while (counter != MaxDirtyColliders); // in case of bad odds       
    }
    public void IncreaseCleanRate()
    {
        if (!_canClean) // no need to focus too much on cleaning
        {
            Debug.LogWarning("You can't clean yet!");
            return;
        }

        // enables the decay mechanic to start again
        // if (KitchenScore < 1) StartCoroutine(CO_DecayKitchen());

        KitchenScore += UnityEngine.Random.Range(5, 10); // test value for now

        if (KitchenScore > 100)
            KitchenScore = 100;
    }

    #endregion
    #region Private

    private void StartKitchenDecay() => StartCoroutine(CO_DecayKitchen());
    private void EnableColliders(bool isActive)
    {
        foreach (GameObject gameObject in _dirtyColliders)
            gameObject.SetActive(isActive);
    }

    #endregion
    #region Enumerators

    // -OLD KITCHEN DECAY LOGIC- 
    private IEnumerator CO_DecayKitchen()
    {
        KitchenScore = 100;

        yield return new WaitForSeconds(10f);

        while (KitchenScore > 0)
        {
            yield return new WaitForSeconds(_decayTimer);
            KitchenScore -= _decayRate;

            if (KitchenScore < 70f && !_canClean)
            {
                _canClean = true;
                EnableColliders(true);
            }
            else if (KitchenScore > _cleanlinessThreshold)
            {
                _canClean = false;
                EnableColliders(false);
            }
        }
    }    
    public IEnumerator CO_EnableDirtyColliders()
    {
        yield return new WaitForSeconds(ONE_MINUTE);
        EnableRandomColliders();
    }

    #endregion
}
