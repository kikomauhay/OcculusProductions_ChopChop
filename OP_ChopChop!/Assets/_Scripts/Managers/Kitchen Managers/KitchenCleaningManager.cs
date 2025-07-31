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
    private const float GRACE_PERIOD = 10f;
    private const float DIRTY_INTERVAL = 20f;
    private const float DIRTY_RATE = 8f;
    private int _maxDirtyColliders;

    [SerializeField] private bool _isDeveloperMode;

    #endregion

    #region Unity

    protected override void Awake() => base.Awake();
    protected override void OnApplicationQuit() => base.OnApplicationQuit();
    private void Start()
    {
        OnCleanedArea += IncreaseCleanRate;
        GameManager.Instance.OnStartService += StartDirtifying;
        GameManager.Instance.OnEndService += StopAllCoroutines;
        GameManager.Instance.OnEndService += DisableAllColliders;

        KitchenScore = 85f; // so that even if there's only 1 dirty collider (needs balance testing)
        HandUsageCounter = 30;
        _maxDirtyColliders = 1;

        if (_maxDirtyColliders < 1)
            Debug.LogWarning($"There will only be {_maxDirtyColliders} colliders available!");

        if (_isDeveloperMode)
            InvokeRepeating("PrintScore", 0f, 1f);
    }

    private void OnDestroy()
    {
        OnCleanedArea -= IncreaseCleanRate;
        GameManager.Instance.OnStartService -= StartDirtifying;
        GameManager.Instance.OnEndService -= StopAllCoroutines;
        GameManager.Instance.OnEndService -= DisableAllColliders;
    }

    #endregion
    #region Pubilc

    public void EnableRandomColliders()
    {
        // int counter = 0; included in the OLD SPANWING LOGIC

        if (_maxDirtyColliders != 4)
        {
            for (int i = 0; i < _maxDirtyColliders; i++)
            {
                _dirtyColliders[i].SetActive(true);
                // Debug.LogWarning($"{this}: a dirty collider has spawned!");
            }
        }
        else EnableAllColliders(true);    
    }
    public void IncreaseCleanRate()
    {
        KitchenScore += UnityEngine.Random.Range(10, 20); 

        if (KitchenScore > 100)
            KitchenScore = 100;

        Debug.LogWarning($"Cleaned the kitchen! Current score: {KitchenScore}");
    }

    #endregion
    #region Private

    // private void StartKitchenDecay() => StartCoroutine(CO_DecayKitchen());
    private void EnableAllColliders(bool isActive)
    {
        foreach (GameObject gameObject in _dirtyColliders)
            gameObject.SetActive(isActive);
    }
    private void PrintScore() => Debug.Log($"{this} Kitchen Score: {KitchenScore}!");

    #endregion
    #region Enumerators

    public IEnumerator CO_EnableDirtyColliders()
    {
        yield return new WaitForSeconds(UnityEngine.Random.Range(20f, 30f));
        
        EnableRandomColliders();
        
        // Debug.LogWarning($"{this}: dirty colliders done spawning!");
    }

    private IEnumerator CO_DirtifyKitchen()
    {
        yield return new WaitForSeconds(GRACE_PERIOD);

        while (KitchenScore > 0)
        {
            yield return new WaitForSeconds(DIRTY_INTERVAL);
            KitchenScore -= DIRTY_RATE;
            Debug.LogWarning($"Current kitchen score: {KitchenScore}");


            if (KitchenScore < 0)
            {
                KitchenScore = 0f;
                Debug.LogWarning("Kitchen is fully dirty!");
            }
        }
    }

    #region Helpers

    private void StartDirtifying() => StartCoroutine(CO_DirtifyKitchen());
    private void DisableAllColliders() => EnableAllColliders(false);
    
    #endregion

    #endregion
}
