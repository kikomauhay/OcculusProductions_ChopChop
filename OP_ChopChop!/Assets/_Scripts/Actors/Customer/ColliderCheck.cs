using System.Collections;
using UnityEngine;

public class ColliderCheck : MonoBehaviour
{
    public CustomerOrder CustomerOrder { get; set; }

    void OnTriggerEnter(Collider other)
    {
        if (CustomerOrder == null) return;

        // player served an ingredient
        if (other.gameObject.GetComponent<Ingredient>() != null)
        {
            CustomerOrder.CustomerSR = 0f;
            StartCoroutine(CustomerOrder.AngryReaction());
            Destroy(other.gameObject); // destroys the ingredient on collision
            StartCoroutine(DisableCollider());

            return;
        }

        Plate plate = other.gameObject.GetComponent<Plate>();
        Dish dish = other.gameObject.GetComponent<Dish>();
        Food food = other.gameObject.GetComponentInChildren<Food>();

        // customer reaction based on the given order
        if (dish.IsContaminated)
        {
            // ORDER IS EXPIRED OR CONTAMINATED
            CustomerOrder.CustomerSR = 0f;
            
            // DO GAME OVER LOGIC
        }
        else if (CustomerOrder.OrderIsSameAs(dish))
        {
            // CORRECT ORDER
            CustomerOrder.CustomerSR = (dish.DishScore + CustomerOrder.PatienceRate) / 2f;
            StartCoroutine(CustomerOrder.HappyReaction());
        }
        else
        {
            // WRONG ORDER
            CustomerOrder.CustomerSR = 0f;
            StartCoroutine(CustomerOrder.AngryReaction());
        }

        // finishing actions for the plate
        Destroy(food.gameObject);
        StartCoroutine(DisableCollider());

        plate.IncrementUseCounter();
        plate.TogglePlated();
        dish.EnableBoxCollider();
    }

    IEnumerator DisableCollider()
    {
        GetComponent<Collider>().enabled = false;
        yield return new WaitForSeconds(3f);
        GetComponent<Collider>().enabled = true;
    }
}