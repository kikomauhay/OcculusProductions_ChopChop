using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class OrderManager : Singleton<OrderManager>
{
    protected override void Awake() { base.Awake(); }

    [Header("Arrays of Prefabs and Locations")]
    [SerializeField] private GameObject[] dishPrefabs; //prefabs of dishes UI to appear
    [SerializeField] private GameObject[] prefabSpawnLocations;  //idea is to lock the positions of spawning, prob

    public GameObject[] _PrefabSpawnLocations
    {
        get { return prefabSpawnLocations; }
    }

    [Header("Orders on Screen")]
    [SerializeField] private List<GameObject> orderList; //List for the dishes UI to appear on screen

    [Header("Set Timer for Order")]
    [SerializeField] private float timeToMakeOrder; //how long customer's patiences //This needs to be balanced based on the Play Testing
    [SerializeField] private float nextOrderTimer;


   void Start()
    {
        StartCoroutine(TimerForNextOrder());
    }

    void Update()
    {
               
    }

    private void DoSpawningDishOrder()
    {
        //int ranNum = Random.Range(0, 1); //For spawning either Nigiri or Maki

        for( int x = 0; x < prefabSpawnLocations.Length; x++)
        {
            if (!prefabSpawnLocations[x].GetComponent<SpawnLocationScript>()._IsPrefabPresent) //there is a empty slot
            {
                GameObject spawnOrder = Instantiate(dishPrefabs[0],
                                             prefabSpawnLocations[x].gameObject.transform.position,
                                             prefabSpawnLocations[x].gameObject.transform.rotation);

                orderList.Add(spawnOrder);

                spawnOrder.GetComponent<NigiriDish>()._OrderLocation = prefabSpawnLocations[x].gameObject;

                prefabSpawnLocations[x].gameObject.GetComponent<SpawnLocationScript>()._IsPrefabPresent = true;

                spawnOrder.GetComponent<NigiriDish>().maxTime = timeToMakeOrder; //Set Timer

                break;
            }
           
        }

        StartCoroutine(TimerForNextOrder());
    }

    public bool IsEmptySpawnLocation()
    {
        Debug.Log("isEmptyPlaying");
        for (int i = 0; i < prefabSpawnLocations.Length; i++)
        {
            if (prefabSpawnLocations[i].gameObject.GetComponent<SpawnLocationScript>()._IsPrefabPresent == false)
            {
                Debug.Log("IsEmpty True");
                return true;
            }
        }
        Debug.Log("IsEmpty False");
        return false;
    }

    public IEnumerator TimerForNextOrder()
    {
        yield return new WaitForSeconds(nextOrderTimer);

        if (IsEmptySpawnLocation())
        {
            DoSpawningDishOrder();
        }
    }

    public void RemoveDishFromList(GameObject dishToRemove)
    {
        orderList.Remove(dishToRemove);
    }

    public void OrderComplete(GameObject orderToRemove)
    {
       orderToRemove.GetComponent<NigiriDish>().DestroyPrefab();
    }

    protected override void OnApplicationQuit() { base.OnApplicationQuit(); }

}
