using UnityEngine;

public class CustomerColliderCheck : MonoBehaviour
{
    public CustomerOrder CustomerOrder { get; set; }

    private void OnTriggerEnter(Collider other)
    {
        if (CustomerOrder.GetComponent<CustomerOrder>() == null) return;

        Dish collidedDish = other.gameObject.GetComponent<Dish>();

        if (CustomerOrder.OrderIsSameAs(other.gameObject.GetComponent<Dish>()))
        {
            CustomerOrder.StartCoroutine("DoPositiveReaction");
            
            // calculates the customer SR
            CustomerOrder.CustomerSR = (collidedDish.DishScore + CustomerOrder.PatienceRate) / 2f;
            Destroy(other.gameObject);

            Debug.LogWarning("CORRECT ORDER");

            // salmon gets eaten
            // plate gets dirty

            return;
        }

        Debug.LogError("WRONG ORDER");
        CustomerOrder.StartCoroutine("DoNegativeReaction");
    }
}