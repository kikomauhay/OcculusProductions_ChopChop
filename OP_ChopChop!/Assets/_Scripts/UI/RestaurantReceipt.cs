using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RestaurantReceipt : MonoBehaviour
{
    [SerializeField] private GameObject[] customerRatings;
    [SerializeField] private GameObject[] kicthenRatings;
    [SerializeField] private GameObject[] restaurantRatings;
   
    [SerializeField] TextMeshProUGUI totalCustomerServedTxt;
    public int totalcustomerServed;

    //index is set according to order of rating which is S to F.
    //0 = S....4 = F.

    public void GiveTotalCustomerServed() //Gives the text for the total customer Served;
    {
        totalCustomerServedTxt.text = totalcustomerServed.ToString();
    }

    public void GiveCustomerRating(int index)  
    {
        customerRatings[index].gameObject.SetActive(true);
    }

    public void GiveKitchenRating(int index)
    {
        kicthenRatings[index].gameObject.SetActive(true);
    }

    public void GiveRestaurantRating(int index)
    {
        restaurantRatings[index].gameObject.SetActive(true);
    }


    public int ReturnScoretoIndexRating(float scoreToCheck) // return the int index for the rating for score to check
    {
        int rating = 0;

        if(scoreToCheck >= 97f) 
        {
            rating = 0; //S
        }
        else if (scoreToCheck >= 93 &&  scoreToCheck <= 96)
        {
            rating = 1; //A
        }
        else if (scoreToCheck >= 89 && scoreToCheck <= 92)
        {
            rating = 2; //B
        }
        else if (scoreToCheck >= 85 && scoreToCheck <= 88)
        {
            rating = 3; //C
        }
        else if (scoreToCheck <= 84)
        {
            rating = 4; //F
        }

        return rating;
    }
    
}
