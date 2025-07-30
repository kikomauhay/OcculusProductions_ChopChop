using System.Collections.Generic;
using System.Collections;
using UnityEngine;

/// </summary> -WHAT DOES THIS SCRIPT DO-
///
/// Spawns different objects that's needed in the game.
///
/// </summary>

public class SpawnManager : StaticInstance<SpawnManager>
{
    #region Members

    [Header("Onboarding")]
    [SerializeField] private bool _isTutorial;
    [SerializeField] private Collider _tutorialCollider;

    [Header("Prefabs"), Tooltip("0 = smoke, 1 = bubble, 2 = sparkle, 3 = stinky")]
    [SerializeField] private GameObject[] _vfxPrefabs;
    [SerializeField] private GameObject _salmonSashiPrefab, _tunaSashimiPrefab;

    [Space(10f)]
    [SerializeField] private GameObject _customerPrefab;
    [SerializeField] private GameObject _atriumPrefab, _tunaCustomerPrefab;

    [Header("Instantiated Bins"), Tooltip("0 = ingredients, 1 = foods, 2 = dishes, 3 = customers, 4 = VFXs")]
    [SerializeField] private Transform[] _bins; // avoids clutters in the hierarchy  

    [Header("Customer Components")]
    [SerializeField] private CustomerSeat[] _customerSeats;
    [SerializeField] private NEW_ColliderCheck[] _newColliderChecks;
    private List<GameObject> _seatedCustomers = new List<GameObject>();

    [Header("Customer Spawning Timers"), Tooltip("Can be changed to use for testing")]
    [SerializeField] private float _spawnCountdown;
    [SerializeField] private float _spawnInterval;

    [Header("Debugging")]
    [SerializeField] private bool _isDeveloperMode;

    [Header("Fish Slice Spawn Point")]
    [SerializeField] Transform _fishSliceSpawnPoint;

    public Transform FishSliceSpawnPoint{ get => _fishSliceSpawnPoint; set => _fishSliceSpawnPoint = value; }
    private int _spawnedCustomers;

    #endregion

    #region Unity

    protected override void Awake() 
    {
        base.Awake();

        if (_customerSeats.Length < 4)
            Debug.LogWarning("Missing Elements in CustomerSeats[]");

        if (_newColliderChecks.Length < 4)
            Debug.LogWarning("Missing Elements in NewColliderChecks[]");
    }
    protected override void OnApplicationQuit() => base.OnApplicationQuit();
    private void Start() // BIND TO EVENTS
    {
        GameManager.Instance.OnStartService += StartCustomerSpawning;
        GameManager.Instance.OnEndService += ClearCustomerSeats;
        GameManager.Instance.OnEndService += StopCustomerSpawning;

        _spawnedCustomers = 0;
        _spawnCountdown = 2f;
        _spawnInterval = 10f;

        if (_isTutorial)
        {
            transform.position = new Vector3(0.09f, 0f, 0f);
            Debug.Log($"{name} tutorial mode: {_isTutorial}");
        }

        if (!_isDeveloperMode)
            StartCoroutine(DelayedEventBinding());

        else
            Debug.Log($"{name} developer mode: {_isDeveloperMode}");
    }
    private void Update()
    {
        if (!_isDeveloperMode) return;

        if (Input.GetKeyDown(KeyCode.LeftAlt))
            SpawnTutorialCustomer(true);

        if (Input.GetKeyDown(KeyCode.LeftControl))
            SpawnTutorialCustomer(false);
    }
    private void OnDestroy() // UNBIND FROM EVENTS
    {
        if (_isDeveloperMode) return;

        GameManager.Instance.OnStartService -= StartCustomerSpawning;
        GameManager.Instance.OnEndService -= ClearCustomerSeats;
        GameManager.Instance.OnEndService -= StopCustomerSpawning;
        OnBoardingHandler.Instance.OnTutorialEnd -= ClearCustomerSeats;
    } 
    
    private IEnumerator CreateCustomer()
    {
        // prevents from adding too many customers
        if (_spawnedCustomers < GameManager.Instance.MaxCustomerCount) 
        {
            yield return new WaitForSeconds(_spawnCountdown);
            SpawnCustomer(GiveAvaiableSeat());
        }

        while (GameManager.Instance.CurrentShift == GameShift.Service)
        {
            // coroutine should stop spawning once all seats are full
            if (_spawnedCustomers >= GameManager.Instance.MaxCustomerCount) 
                yield break;

            yield return new WaitForSeconds(_spawnInterval);  
            SpawnCustomer(GiveAvaiableSeat());
        }
    }
    private IEnumerator DelayedEventBinding()
    {
        yield return new WaitForSeconds(2f);
        OnBoardingHandler.Instance.OnTutorialEnd += ClearCustomerSeats;
    }
    
    #endregion
    #region Spawning

    public GameObject SpawnSashimi(IngredientType type, Transform t)
    {
        if (type == IngredientType.RICE) return null;

        return Instantiate(type == IngredientType.SALMON ? _salmonSashiPrefab : _tunaSashimiPrefab,
                           t.position, t.rotation);
    }
    public GameObject SpawnObject(GameObject obj, Transform t, SpawnObjectType type)
    {
        return Instantiate(obj,
                           t.position, t.rotation,
                           _bins[(int)type]);
    }    
    public void SpawnVFX(VFXType type, Transform t, float destroyTime)
    {   
        GameObject vfxInstance = Instantiate(_vfxPrefabs[(int)type], 
                                             t.position, t.rotation,
                                             _bins[4]);

        Destroy(vfxInstance, destroyTime);
    }
    private void SpawnCustomer(int idx)
    {
        if (idx == -1) return;

        if (GameManager.Instance.CurrentShift != GameShift.Service) return;
        
        GameObject customer = SpawnObject(_customerPrefab, 
                                          _customerSeats[idx].transform, 
                                          SpawnObjectType.CUSTOMER);

        CustomerActions customerActions = customer.GetComponent<CustomerActions>();

        // assigns the index to the seat & collider
        CustomerSeat seat = _customerSeats[idx];
        NEW_ColliderCheck colliderCheck = _newColliderChecks[idx];

        // links a box collider & seat to the customer
        colliderCheck.Order = customer.GetComponent<CustomerOrder>();
        
        _seatedCustomers.Add(customer);
        customerActions.SeatIndex = idx;

        // prevents multiple customers getting the same seat 
        seat.IsEmpty = false;

        // adding random noises when the cats spawn
        StartCoroutine(customerActions.RandomMeowing());
        _spawnedCustomers++;

        if (_spawnedCustomers == GameManager.Instance.MaxCustomerCount)
            customer.GetComponent<CustomerOrder>().IsLastCustomer = true;
    }
    public void SpawnTutorialCustomer(bool isAtrium)
    {
        if (!_isTutorial)
        {
            Debug.LogError("Cannot spawn this type of customer!");
            return;
        }
        if (GameManager.Instance.CurrentShift != GameShift.Training)
        {
            Debug.LogError($"Current shift is not in training mode!");
            return;
        }

        // spawns a customer depending on the bool
        GameObject tutorialCustomer = Instantiate(isAtrium ? _atriumPrefab : _tunaCustomerPrefab,
                                                  _customerSeats[0].transform.position,
                                                  _customerSeats[0].transform.rotation,
                                                  transform);

        // assigns components to the new customer
        CustomerActions customerActions = tutorialCustomer.GetComponent<CustomerActions>();
        CustomerSeat seat = _customerSeats[0];
        NEW_ColliderCheck newCollider = _newColliderChecks[0];

        // links the box collider & seat to the customer
        newCollider.Order = tutorialCustomer.GetComponent<CustomerOrder>();
        _seatedCustomers.Add(tutorialCustomer);
        customerActions.SeatIndex = 0;

        // prevents multiple customers getting the same seat 
        seat.IsEmpty = false;

        if (isAtrium)
            Debug.LogWarning($"Spawned Atrium!");

        else
            Debug.LogWarning($"Spawned Tuna Customer!");

        Debug.Log($"{newCollider} wanted Order: {newCollider.Order.WantedPlatter}");
    }

    #endregion
    #region Customer Helpers

    public void RemoveCustomer(GameObject customer) 
    {
        int idx = customer.GetComponent<CustomerActions>().SeatIndex;
        
        // removes the customer from the list
        _seatedCustomers.Remove(customer);

        // removed any link from the removed customer 
        _customerSeats[idx].IsEmpty = true;
        _newColliderChecks[idx].Order = null;
    }   
    private int GiveAvaiableSeat() // sets the index where the customer should sit
    {
        for (int i = 0; i < _customerSeats.Length; i++)
        {
            CustomerSeat seat = _customerSeats[i].gameObject.GetComponent<CustomerSeat>();

            if (seat.IsEmpty)
                return i;
            
            else continue;
        }
        return -1; // all seats are empty
    }

    #endregion
    #region Event Methods

    public void StartCustomerSpawning() 
    {
        transform.position = Vector3.zero;
        StartCoroutine(CreateCustomer());
    }
    private void StopCustomerSpawning() => StopCoroutine(CreateCustomer());
    private void ClearCustomerSeats()
    {
        ClearSeats();
        StopAllCoroutines(); 
    }
    public void DisableTutorial()
    {
        _isTutorial = false;
        _tutorialCollider.GetComponent<NEW_ColliderCheck>().DisableTutorial();
    }
    public void ClearSeats()
    {
        if (_customerSeats.Length > 1)
            foreach (CustomerSeat seat in _customerSeats)
                seat.IsEmpty = true;

        if (_newColliderChecks.Length > 1)
            foreach (NEW_ColliderCheck col in _newColliderChecks)
                col.Order = null;

        if (_seatedCustomers.Count > 1)
        {
            foreach (GameObject obj in _seatedCustomers)
                Destroy(obj);

            _seatedCustomers.Clear();
        }
    }

    #endregion
}

#region Enumerations

    public enum SpawnObjectType 
    { 
        INGREDIENT, 
        FOOD, 
        DISH, 
        CUSTOMER, 
        VFX 
    }
    public enum VFXType // & destroyTime
    { 
        SMOKE,   // 1s
        BUBBLE,  // 3s
        SPARKLE, // 5s
        STINKY,  // 5s
        RICE,    // 3s
        SPLASH   // 4s
    }

#endregion