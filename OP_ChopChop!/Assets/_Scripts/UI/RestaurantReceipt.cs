using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestaurantReceipt : MonoBehaviour
{
    [SerializeField] private GameObject[] customerRatings;
    [SerializeField] private GameObject[] kicthenRatings;
    [SerializeField] private GameObject[] restaurantRatings;

    //index is set according to order of rating which is S to F.
    //0 = S....4 = F.

    private void GiveCustomerRating(int index)  
    {
        customerRatings[index].gameObject.SetActive(true);
    }

    private void GiveKitchenRating(int index)
    {
        kicthenRatings[index].gameObject.SetActive(true);
    }

    private void GiveRestaurantRating(int index)
    {
        restaurantRatings[index].gameObject.SetActive(true);
    }

}
