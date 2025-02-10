using UnityEngine;

public class CustomerColliderCheck : MonoBehaviour
{
    public CustomerOrder CustomerOrder { get; set; }

    private void OnCollisionEnter(Collision collision)
    {
        if (CustomerOrder.GetComponent<CustomerOrder>() == null) return; 

        Dish dish = collision.gameObject.GetComponent<Dish>();
        
        if (CustomerOrder.CheckDishServed(collision.gameObject))
        {
            CustomerOrder.StartCoroutine("DoPositiveReaction"); 
            CustomerOrder.CustomerSR = (dish.DishScore + CustomerOrder.PatienceRate) / 2f;
        }  
    }
}