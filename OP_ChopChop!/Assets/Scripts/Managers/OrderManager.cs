using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class OrderManager : Singleton<OrderManager>
{

#region Members

    [Header("Arrays of Prefabs and Locations")]
    [SerializeField] private GameObject[] _dishPrefabs; // prefabs of dishes UI to appear
    [SerializeField] private GameObject[] _prefabSpawnLocations;  // idea is to lock the positions of spawning

    public GameObject[] PrefabSpawnLocations => _prefabSpawnLocations; 

    [Header("Orders on Screen")]
    [SerializeField] private List<GameObject> _orderList; // List for the dishes UI to appear on screen

    [Header("Set Timer for Order")]
    [SerializeField] private float _timeToMakeOrder; // how long customer's patiences (needs to be balanced)
    [SerializeField] private float _nextOrderTimer;

    [Header("For Sushi checking")]
    [SerializeField] private GameObject[] _completedDishes;
    [SerializeField] private GameObject _plateToServe;

#endregion

#region Methods

    protected override void Awake() => base.Awake(); 
    protected override void OnApplicationQuit() => base.OnApplicationQuit(); 
    void Start() => StartCoroutine(StartNextOrder());

#endregion

    private void DoSpawningDishOrder()
    {
        //int ranNum = Random.Range(0, 1); //For spawning either Nigiri or Maki

        for (int i = 0; i < _prefabSpawnLocations.Length; i++)
        {
            // there is an empty slot 
            if (!_prefabSpawnLocations[i].GetComponent<SpawnLocationScript>().IsPrefabPresent) 
            {
                GameObject spawnOrder = Instantiate(
                    _dishPrefabs[0],
                    _prefabSpawnLocations[i].gameObject.transform.position,
                    _prefabSpawnLocations[i].gameObject.transform.rotation
                );

                _orderList.Add(spawnOrder);
                spawnOrder.GetComponent<SushiDishUI>()._OrderLocation = _prefabSpawnLocations[i].gameObject;

                // fills a slot so this doens't take any other orders
                _prefabSpawnLocations[i].gameObject.GetComponent<SpawnLocationScript>().IsPrefabPresent = true;

                // sets customer timer before they leave
                spawnOrder.GetComponent<SushiDishUI>().maxTime = _timeToMakeOrder; 
                break;
            }
        }
        StartCoroutine(StartNextOrder());        
    }

    public bool IsEmptySpawnLocation()
    {
        Debug.Log("isEmptyPlaying");
        for (int i = 0; i < _prefabSpawnLocations.Length; i++)
        {
            if (_prefabSpawnLocations[i].gameObject.GetComponent<SpawnLocationScript>().IsPrefabPresent == false)
            {
                Debug.Log("IsEmpty True");
                return true;
            }
        }
        Debug.Log("IsEmpty False");
        return false;
    }

    public IEnumerator StartNextOrder()
    {   
        yield return new WaitForSeconds(_nextOrderTimer);

        if (IsEmptySpawnLocation())
            DoSpawningDishOrder();
    }


#region Order_Checking

/// <summary>
/// Upon serving the plate, this system checks against the list if there are any matching orders. 
/// If there is, yeet that one
/// If there are duplicates, check against which one's timer is about to be run out. 
/// Yeet the one with the lesser timer. 
/// </summary>

    public void CheckOrder()
    {
        for(int i = 0; i < _orderList.Count; i++) 
        {
            // gets the type of the sushi attached to the plate
            Sushi sushiComponent = _plateToServe.GetComponentInChildren<Sushi>(); 

            // gets the type of sushi from the order
            SushiDishUI dishUIComponent = _orderList[i].GetComponent<SushiDishUI>(); 

            if (sushiComponent != null && 
                dishUIComponent != null && 
                sushiComponent.DishType.Equals(dishUIComponent.dishType))
            {
                if (i > 0 && _orderList[i].GetComponent<SushiDishUI>().GetTimeLeft < _orderList[0].GetComponent<SushiDishUI>().GetTimeLeft)
                {
                    OrderComplete(_orderList[0]);
                    RemoveDishFromList(_orderList[0]);
                    _orderList.RemoveAt(0);
                    Debug.Log($"Removed {_orderList[0].name}");
                }
                else
                {
                    OrderComplete(_orderList[i]);
                    RemoveDishFromList(_orderList[i]);
                    _orderList.RemoveAt(i);
                    Debug.Log($"Removed {_orderList[i].name}");
                }
                break;
            }   
        }
    }

    public void RemoveDishFromList(GameObject dishToRemove)
    {
        _orderList.Remove(dishToRemove);
    }
    public void OrderComplete(GameObject orderToRemove)
    {
       //orderToRemove.GetComponent<SushiDishUI>().DestroyPrefab();
    }

#endregion
}
