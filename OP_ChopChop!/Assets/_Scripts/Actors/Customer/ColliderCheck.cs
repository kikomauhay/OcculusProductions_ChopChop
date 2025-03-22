using System.Collections;
using UnityEngine;

public class ColliderCheck : MonoBehaviour
{
    public CustomerOrder CustomerOrder { get; set; }

    void OnTriggerEnter(Collider other)
    {
        if (CustomerOrder == null)
        {
            Debug.LogError("null CustomerOrder");
            return;
        }

        if (other.gameObject.GetComponent<Ingredient>() != null)
        {
            CustomerOrder.CustomerSR = 0f;
            StartCoroutine(CustomerOrder.AngryReaction());
            Destroy(other.gameObject); // destroys the ingredient on collision

            return;
        }

        Dish collidedDish = other.gameObject.GetComponentInChildren<Dish>();
        Plate plate = other.gameObject.GetComponent<Plate>();

        // customer reaction based on the given order
        if (collidedDish.IsContaminated) // ORDER IS EXPIRED OR CONTAMINATED
        {
            CustomerOrder.CustomerSR = 0f;
            StartCoroutine(CustomerOrder.ExpiredReaction());            
        }
        else if (CustomerOrder.OrderIsSameAs(collidedDish)) // CORRECT ORDER
        {
            CustomerOrder.CustomerSR = (collidedDish.DishScore + CustomerOrder.PatienceRate) / 2f;
            StartCoroutine(CustomerOrder.HappyReaction());            
        }
        else // WRONG ORDER
        {
            CustomerOrder.CustomerSR = 0f;
            StartCoroutine(CustomerOrder.AngryReaction());            
        }

        plate.ToggleClean();      
        plate.TogglePlated();
        Destroy(collidedDish.gameObject);   
        StartCoroutine(DisableColider());
    }

    IEnumerator DisableColider()
    {
        GetComponent<Collider>().enabled = false;
        yield return new WaitForSeconds(3f);
        GetComponent<Collider>().enabled = true;
    }
}