using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class OrderManager : Singleton<OrderManager>
{

    protected override void Awake() { base.Awake(); }

    [SerializeField] private GameObject[] dishPrefabs; //prefabs of dishes UI to appear
    //[SerializeField] private GameObject[] prefabSpawnLocations;  //idea is to lock the positions of spawning, prob
    [SerializeField] private List<GameObject> dishList; //List for the dishes UI to appear on screen


   void Start()
    {
        
    }

    void Update()
    {
        
    }

    private void SpawnDishUI()   //Spawning of the dishes on screen
    {
        //int ranNum = Random.Range(0, 1); //For spawning either Nigiri or Maki

        GameObject  dishToSpawn = Instantiate(dishPrefabs[0]);

        dishList.Add(dishToSpawn);
    }

    private void DishComplete(GameObject dishToRemove)
    {
        // Find the index of the dish to remove
        int index = dishList.IndexOf(dishToRemove);

        // Destroy the GameObject
        Destroy(dishToRemove);

        // Remove from the list
        if (index >= 0)
        {
            dishList.RemoveAt(index);
        }
    }

    /*
    To do
    - spawning the UI
    - Ticking of the box when ingredient place on plate is correct
    - Rectangle bar timer decreasing and changing color
    - Deleting and shifting the UIs if an order is complete and there are more orders
    
    */

    protected override void OnApplicationQuit() { base.OnApplicationQuit(); }

}
