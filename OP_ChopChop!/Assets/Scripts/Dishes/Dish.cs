using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// This will act as the PLATED version of the food
/// This is also the prefab that will be served to the customers
/// 
/// </summary>

public abstract class Dish : MonoBehaviour
{   

    public float DishScore { get; set; }
    public OrderType OrderType { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    protected void OnTriggerEnter(Collider other)
    {
        // collides with the custoemr to score points

        if (true) // change to the proper collision condition
        {
            GameManager.Instance.OnCustomerServed?.Invoke(DishScore);
        }
    }
}
