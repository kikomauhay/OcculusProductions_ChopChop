using UnityEngine;

public class CustomerColliderCheck : MonoBehaviour
{
    public CustomerOrder CustomerOrder { get; set; }

    private void OnTriggerEnter(Collider other)
    {
        if (CustomerOrder.GetComponent<CustomerOrder>() == null) return;

        Dish collidedDish = other.gameObject.GetComponent<Dish>();
        Plate plate = collidedDish.gameObject.GetComponentInParent<Plate>();

        if (CustomerOrder.OrderIsSameAs(other.gameObject.GetComponent<Dish>()))
        {
            Debug.LogWarning("CORRECT ORDER");
            CustomerOrder.StartCoroutine("DoPositiveReaction");
            
            // calculates the customer SR
            CustomerOrder.CustomerSR = (collidedDish.DishScore + CustomerOrder.PatienceRate) / 2f;
            
            // customer "eats" the food
            Destroy(other.gameObject);

            plate.SetContaminated();            
            return;
        }

        Debug.LogError("WRONG ORDER");
        CustomerOrder.StartCoroutine("DoNegativeReaction");
        
        // idk if the customer still eats the food or skips it entirely
    }
}