using System.Collections;
using UnityEngine;

public class ColliderCheck : MonoBehaviour
{
    public CustomerOrder CustomerOrder { get; set; }

    void OnTriggerEnter(Collider other)
    {
        if (CustomerOrder == null) 
        {
            Debug.LogError("Null CustomerOrder");
            return;
        }

        if (other.gameObject.GetComponent<Ingredient>() != null) 
        {
            Debug.LogError("GIVEN ORDER IS AN INGREDIENT");
            
            // CustomerOrder.StartCoroutine("DoNegativeReaction");
            StartCoroutine(CustomerOrder.DoReaction(FaceVariant.MAD));

            Destroy(other.gameObject);
            return;
        }

        Dish collidedDish = other.gameObject.GetComponentInChildren<Dish>();
        Plate plate = other.gameObject.GetComponent<Plate>();

        if (CustomerOrder.OrderIsSameAs(collidedDish))
        {
            Debug.LogWarning("CORRECT ORDER");

            // CustomerOrder.StartCoroutine("DoPositiveReaction");
            StartCoroutine(CustomerOrder.DoReaction(FaceVariant.HAPPY));
            
            // calculates the customer SR
            CustomerOrder.CustomerSR = (collidedDish.DishScore + CustomerOrder.PatienceRate) / 2f;
            
            // customer "eats" the food
            Destroy(collidedDish.gameObject);
            // SpawnManager.Instance.RemoveCustomer(CustomerOrder.gameObject);

            // plate.SetContaminated();      
            plate.TogglePlated();
            StartCoroutine(DisableColider());

            return;
        }

        Debug.LogError("WRONG ORDER");

        // CustomerOrder.StartCoroutine("DoNegativeReaction");
        StartCoroutine(CustomerOrder.DoReaction(FaceVariant.MAD));

        Destroy(other.gameObject);
    }

    IEnumerator DisableColider()
    {
        GetComponent<Collider>().enabled = false;

        yield return new WaitForSeconds(3f);

        GetComponent<Collider>().enabled = true;
    }
}