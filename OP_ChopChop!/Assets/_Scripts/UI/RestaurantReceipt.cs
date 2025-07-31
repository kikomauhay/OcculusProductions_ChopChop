using UnityEngine;
using TMPro;
using Unity.VisualScripting;

public class RestaurantReceipt : MonoBehaviour
{
    #region Members

    public int CustomersServed { get; set; }

    [SerializeField] private GameObject[] customerRatings;
    [SerializeField] private GameObject[] kicthenRatings;
    [SerializeField] private GameObject[] restaurantRatings;

    [SerializeField] private TextMeshProUGUI totalCustomerServedTxt;
    public int totalcustomerServed;

    //index is set according to order of rating which is S to F.
    //0 = S....4 = F.
    #endregion

    #region Unity

    private void OnEnable()
    {
        GameManager.Instance.OnStartService += DisableScores;
    }
    private void OnDisable()
    {
        GameManager.Instance.OnStartService -= DisableScores;
    }

    #endregion
    #region Ratings

    public void GiveTotalCustomerServed() => //Gives the text for the total customer Served;
        totalCustomerServedTxt.text = $"{GameManager.Instance.CustomersServed}";
    
    public void SetCustomerRating(int index) =>
        customerRatings[index].gameObject.SetActive(true);  

    public void SetKitchenRating(int index) => 
        kicthenRatings[index].gameObject.SetActive(true);

    public void SetRestaurantRating(int index) => 
        restaurantRatings[index].gameObject.SetActive(true);

    #endregion

    public int ConvertToScoreIndex(float scoreToCheck) // return the int index for the rating for score to check
    {
        if (scoreToCheck >= 90f) 
            return 0; // S

        else if (scoreToCheck >= 80f &&  scoreToCheck <= 89f)
            return 1; // A
        
        else if (scoreToCheck >= 70f && scoreToCheck <= 79f)
            return 2; // B
        
        else if (scoreToCheck >= 60f && scoreToCheck <= 69f)
            return 3; // C
        
        else if (scoreToCheck <= 59f)
            return 4; // F
        
        else
        {
            Debug.LogWarning("Resulted in a negative number!");
            return -1;
        }            
    }

    private void DisableScores()
    {
        if (customerRatings.Length > 0)
            foreach (GameObject ratings in customerRatings)
                ratings.SetActive(false);

        if (kicthenRatings.Length > 0)
            foreach (GameObject ratings in kicthenRatings)
                ratings.SetActive(false);

        if (restaurantRatings.Length > 0)
            foreach (GameObject ratings in restaurantRatings)
                ratings.SetActive(false);
    }
}
