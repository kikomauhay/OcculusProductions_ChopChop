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

            Debug.LogError("Game Over!");
            return;
        }

        Plate plate = other.gameObject.GetComponent<Plate>();
        Dish dish = other.gameObject.GetComponentInChildren<Dish>();
        
        // customer reaction based on the given order
        CheckDish(dish);

        // finishing actions for the plate
        Destroy(dish.gameObject);
        StartCoroutine(DisableCollider());

        plate.IncrementUseCounter();
        plate.TogglePlated();
    }

#region Helpers

    void CheckDish(Dish d)
    {
        if (d.IsContaminated || d.IsExpired) // ORDER IS EXPIRED OR CONTAMINATED
        {
            CustomerOrder.CustomerSR = 0f;
            Debug.LogError("Game Over!");
            StartCoroutine(CustomerOrder.ExpiredReaction());
        }
        else if (CustomerOrder.OrderIsSameAs(d)) // CORRECT ORDER
        {    
            CustomerOrder.CustomerSR = (d.DishScore + CustomerOrder.PatienceRate) / 2f;
            StartCoroutine(CustomerOrder.HappyReaction());
        }
        else // WRONG ORDER
        {
            CustomerOrder.CustomerSR = 0f;
            StartCoroutine(CustomerOrder.AngryReaction());
        }
    }
    IEnumerator DisableCollider()
    {
        GetComponent<Collider>().enabled = false;
        yield return new WaitForSeconds(3f);
        GetComponent<Collider>().enabled = true;
    }

#endregion
}