using UnityEngine;

public class CustomerColliderCheck : MonoBehaviour
{
    public CustomerOrder CustomerOrder { get; set; }

    private void OnTriggerEnter(Collider other)
    {
        if (CustomerOrder.GetComponent<CustomerOrder>() == null) return; 

        Dish dish = other.gameObject.GetComponent<Dish>();
        
        if (CustomerOrder.CheckDishServed(other.gameObject))
        {
            Debug.Log("Dish Served");
            CustomerOrder.StartCoroutine("DoPositiveReaction"); 
            CustomerOrder.CustomerSR = (dish.DishScore + CustomerOrder.PatienceRate) / 2f;
            Destroy(other.gameObject);
        }  
        else
        {
            Debug.Log("Dish not Detected"); 
        }
    }
}