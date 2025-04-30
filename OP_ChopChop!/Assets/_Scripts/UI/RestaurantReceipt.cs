using UnityEngine;
using TMPro;
using Unity.Mathematics;

public class RestaurantReceipt : MonoBehaviour
{
    [SerializeField] private GameObject[] customerRatings;
    [SerializeField] private GameObject[] kicthenRatings;
    [SerializeField] private GameObject[] restaurantRatings;
   
    [SerializeField] TextMeshProUGUI totalCustomerServedTxt;
    public int totalcustomerServed;

    //index is set according to order of rating which is S to F.
    //0 = S....4 = F.

#region Ratings

    public void GiveTotalCustomerServed() => //Gives the text for the total customer Served;
        totalCustomerServedTxt.text = totalcustomerServed.ToString();
    
    public void GiveCustomerRating(int index) =>
        customerRatings[index].gameObject.SetActive(true);  

    public void GiveKitchenRating(int index) => 
        kicthenRatings[index].gameObject.SetActive(true);

    public void GiveRestaurantRating(int index) => 
        restaurantRatings[index].gameObject.SetActive(true);

#endregion

    public int ReturnScoretoIndexRating(float scoreToCheck) // return the int index for the rating for score to check
    {
        if (scoreToCheck >= 97f) 
            return 0; // S

        else if (scoreToCheck >= 93f &&  scoreToCheck <= 96f)
            return 1; // A
        
        else if (scoreToCheck >= 89f && scoreToCheck <= 92f)
            return 2; // B
        
        else if (scoreToCheck >= 85f && scoreToCheck <= 88f)
            return 3; // C
        
        else if (scoreToCheck <= 84f)
            return 4; // F
        
        else return -1;
    }
}
