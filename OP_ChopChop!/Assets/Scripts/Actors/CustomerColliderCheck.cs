using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerColliderCheck : MonoBehaviour
{
    public CustomerOrder customerOrder { get; set; }

    private void OnCollisionEnter(Collision collision)
    {
        if(customerOrder.GetComponent<CustomerOrder>() != null) 
        { 
          if(customerOrder.CheckDishServed(collision.gameObject))  
          {
              customerOrder.StartCoroutine("CustomerDeleteTimer");
          }
        }
    }
}
