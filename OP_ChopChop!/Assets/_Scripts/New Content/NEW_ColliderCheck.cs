using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// 
/// - Acts as the reworked version of ColliderCheck.cs
/// 
/// WHAT THIS SCRIPT SHOULD DO: 
///     - Connects a Customer Oorder and the Dish being served
///     - Disable the dish once it's served to the customer
/// 
/// </summary> 

[RequireComponent(typeof(BoxCollider))]
public class NEW_ColliderCheck : MonoBehaviour 
{
#region Properties

    public CustomerOrder Order { get; set; }

#endregion

    [SerializeField] private bool _isTutorial;
    private Collider _collider;

    [Header("Debugging")]
    [SerializeField] private bool _isDevloperMode;

#region Unity

    private void Awake()
    {
        _collider = GetComponent<BoxCollider>();
        // Debug.Log($"{ToString()} developer mode: {_isDevloperMode}");
    }
    private void Start() 
    {
        _collider.isTrigger = true;
        _collider.enabled = true;    
    }
    private void OnTriggerEnter(Collider other)
    {
        Ingredient ing = other.gameObject.GetComponent<Ingredient>();
        NEW_Plate plate = other.gameObject.GetComponent<NEW_Plate>();
        NEW_Dish dish = other.gameObject.GetComponent<NEW_Dish>();

        if (Order == null)
        {
            Debug.LogError($"{Order} is null!");
            return;
        }

        // player has served an INGREDIENT to the customer
        if (ing != null)
        {
            DoIngredientCollision();
            Destroy(other.gameObject);
            return;
        }

        // player has served a DISH to the customer
        if (dish != null)
        {
            DoDishCollision(dish);
            dish.DisableDish();
            plate.Served();
        }

        StartCoroutine(CO_DisableCollider());

        // TUTORIAL LOGIC AFTER THE DISH IS SERVED
        if (!_isTutorial) return;
        
        if (Order.IsTunaCustomer) // TUNA CUSTOMER
        {
            OnBoardingHandler.Instance.AddOnboardingIndex();
            OnBoardingHandler.Instance.PlayOnboarding();
            ShopManager.Instance.ClearList();
            Debug.LogWarning("Tuna Sashimi customer was served!");
        }       
        else if (Order.IsTutorial)  // ATRIUM CUSTOMER
        {
            OnBoardingHandler.Instance.AddOnboardingIndex();
            OnBoardingHandler.Instance.PlayOnboarding();
            Debug.LogWarning("Atrium was served!");
        } 
    }

#endregion

#region Helpers

    private void DoIngredientCollision() 
    {
        Debug.LogError("Player has served an ingredient to the customer!");

        Order.CustomerSR = 0f;

        StartCoroutine(Random.value > 0.5f ? Order.CO_DirtyReaction() : 
                                             Order.CO_AngryReaction());

        StartCoroutine(GameManager.Instance.CO_GameOver());
    }
    private void DoDishCollision(NEW_Dish dish) 
    {
        if (dish.FoodCondition != FoodCondition.CLEAN)
        {
            Order.CustomerSR = 0f;
            Debug.LogError("Game Over!");
            StartCoroutine(Order.CO_DirtyReaction());
        }
        else if (dish.DishPlatter == Order.WantedPlatter)
        {
            if (!Order.IsTutorial)
            {
                Order.CustomerSR = (dish.Score + Order.PatienceRate) / 2f;
                StartCoroutine(Order.CO_HappyReaction());
            }
            else Destroy(Order.gameObject);

            Debug.LogWarning("Happy reaction");
        }
        else
        {
            Order.CustomerSR = 0f;
            StartCoroutine(Order.CO_AngryReaction());
            Debug.LogWarning("Angy reaction");
        }
    }

#endregion

    public void DisableTutorial() => _isTutorial = false;


#region Enumerators

    private IEnumerator CO_DisableCollider()
    {
        _collider.enabled = false;
        yield return new WaitForSeconds(3f);
        _collider.enabled = true;
    }

#endregion

}
