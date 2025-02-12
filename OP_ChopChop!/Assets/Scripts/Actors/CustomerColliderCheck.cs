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
            CustomerOrder.StartCoroutine("DoPositiveReaction");
            
            // calculates the customer SR
            CustomerOrder.CustomerSR = (collidedDish.DishScore + CustomerOrder.PatienceRate) / 2f;
            Destroy(other.gameObject);
            plate.SetContaminated();

            Debug.LogWarning("CORRECT ORDER");
            return;
        }

        Debug.LogError("WRONG ORDER");
        CustomerOrder.StartCoroutine("DoNegativeReaction");
    }
}