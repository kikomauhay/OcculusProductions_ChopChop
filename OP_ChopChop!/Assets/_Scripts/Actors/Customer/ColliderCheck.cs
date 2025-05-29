using System.Collections;
using UnityEngine;

public class ColliderCheck : MonoBehaviour
{
    public CustomerOrder CustomerOrder { get; set; }
    [SerializeField] private bool _isTutorial;

    private void OnTriggerEnter(Collider other)
    {
        if (CustomerOrder == null) return;

        // player served an ingredient
        if (other.gameObject.GetComponent<Ingredient>() != null)
        {
            CustomerOrder.CustomerSR = 0f;

            StartCoroutine(CustomerOrder.CO_DirtyReaction());
            
            Destroy(other.gameObject); // destroys the ingredient on collision
            StartCoroutine(DisableCollider());

            Debug.LogError("Game Over!");
            return;
        }

        NEW_Plate plate = other.gameObject.GetComponent<NEW_Plate>();
        NEW_Dish dish = other.gameObject.GetComponentInChildren<NEW_Dish>();
        
        // customer reaction based on the given order
        CheckDish(dish);

        // finishing actions for the plate



        Destroy(dish.gameObject);
        StartCoroutine(DisableCollider());


        if (!_isTutorial) return;
        
        
    }

#region Enumerators

    private IEnumerator DisableCollider()
    {
        GetComponent<Collider>().enabled = false;
        yield return new WaitForSeconds(3f);
        GetComponent<Collider>().enabled = true;
    }

    #endregion

    #region Helpers

    private void CheckDish(NEW_Dish d)
    {
        // ORDER IS EXPIRED OR CONTAMINATED
        if (d.FoodCondition != FoodCondition.CLEAN)
        {
            CustomerOrder.CustomerSR = 0f;
            Debug.LogError("Game Over!");
            StartCoroutine(CustomerOrder.CO_DirtyReaction());
        }

        // CORRECT ORDER
        else if (d.DishPlatter == CustomerOrder.WantedPlatter)
        {
            CustomerOrder.CustomerSR = (d.Score + CustomerOrder.PatienceRate) / 2f;
            StartCoroutine(CustomerOrder.CO_HappyReaction());
        }

        // WRONG ORDER
        else
        {
            CustomerOrder.CustomerSR = 0f;
            StartCoroutine(CustomerOrder.CO_AngryReaction());
        }
    }
    public void DisableTutorial() => _isTutorial = false;

#endregion
}