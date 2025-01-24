using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class OrderManager : Singleton<OrderManager>
{
    protected override void Awake() { base.Awake(); }

    [Header("Arrays of Prefabs and Locations")]
    [SerializeField] private GameObject[] dishPrefabs; //prefabs of dishes UI to appear
    [SerializeField] private GameObject[] prefabSpawnLocations;  //idea is to lock the positions of spawning

    public GameObject[] _PrefabSpawnLocations
    {
        get { return prefabSpawnLocations; }
    }

    [Header("Orders on Screen")]
    [SerializeField] private List<GameObject> orderList; //List for the dishes UI to appear on screen

    [Header("Set Timer for Order")]
    [SerializeField] private float timeToMakeOrder; //how long customer's patiences //This needs to be balanced based on the Play Testing
    [SerializeField] private float nextOrderTimer;

    [Header("For Sushi checking")]
    [SerializeField] private GameObject[] preFabCompletedDishes;
    [SerializeField] private GameObject plateToServe;


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

                spawnOrder.GetComponent<SushiDishUI>()._OrderLocation = prefabSpawnLocations[x].gameObject;

                prefabSpawnLocations[x].gameObject.GetComponent<SpawnLocationScript>()._IsPrefabPresent = true;

                spawnOrder.GetComponent<SushiDishUI>().maxTime = timeToMakeOrder; //Set Timer

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


    /// <summary>
    /// Upon serving the plate, this system checks against the list if there are any matching Orders. 
    /// If there is, yeet that one
    /// If there are duplicates, check against which one's timer is about to be run out. Yeet the one with the lesser timer 
    /// </summary>
    public void CheckOrder()
    {
        for(int i = 0; i < orderList.Count;i++) 
        {
            Sushi sushiComponent = plateToServe.GetComponentInChildren<Sushi>(); 
            //^To Get Enum of the Sushi attached to the plate^
            SushiDishUI dishUIComponent = orderList[i].GetComponent<SushiDishUI>(); 
            //^To Get the Enum of the Sushi of the order^

            if (sushiComponent != null && dishUIComponent != null && 
                sushiComponent.dishType.Equals(dishUIComponent.dishType))
            {
                if (i > 0 && orderList[i].GetComponent<SushiDishUI>().GetTimeLeft < orderList[0].GetComponent<SushiDishUI>().GetTimeLeft)
                {
                    OrderComplete(orderList[0]);
                    RemoveDishFromList(orderList[0]);
                    orderList.RemoveAt(0);
                    Debug.Log("Removed Current Order");
                }
                else
                {
                    OrderComplete(orderList[i]);
                    RemoveDishFromList(orderList[i]);
                    orderList.RemoveAt(i);
                    Debug.Log("Removed Other Order");
                }
                break;
            }   
        }
    }

    public void RemoveDishFromList(GameObject dishToRemove)
    {
        orderList.Remove(dishToRemove);
    }

    public void OrderComplete(GameObject orderToRemove)
    {
       orderToRemove.GetComponent<SushiDishUI>().DestroyPrefab();
    }

    


    protected override void OnApplicationQuit() { base.OnApplicationQuit(); }

}
