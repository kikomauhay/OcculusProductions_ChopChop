using System.Collections;
using System.Runtime.CompilerServices;
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
    #region Members

    public CustomerOrder Order { get; set; }

    [SerializeField] private bool _isTutorial;
    private Collider _collider;
    private float _disableTimer;

    [Header("Debugging")]
    [SerializeField] private bool _isDevloperMode;

    #endregion

    #region Unity

    private void Awake()
    {
        _collider = GetComponent<BoxCollider>();

        if (_isDevloperMode)
            Debug.Log($"{this} developer mode: {_isDevloperMode}");

        if (_isTutorial)
            Debug.Log($"{this} tutorial mode: {_isTutorial}");
    }
    private void Start()
    {
        _collider.isTrigger = true;
        _collider.enabled = true;
        _disableTimer = 5f; 
    }
    private void Update() => test();
    private void OnTriggerEnter(Collider other)
    {
        if (Order == null)
        {
            Debug.LogError($"CustomerOrder is null!");
            SoundManager.Instance.PlaySound("wrong");
            return;
        }
        if (other.gameObject.GetComponent<Ingredient>() != null)
        {
            // the player shouldn't get a Game Over in the tutorial
            if (GameManager.Instance.CurrentShift == GameShift.Training && _isTutorial)
            {
                PlayWrongDishServed();
            }
            else
            {
                DoIngredientCollision(other.gameObject.GetComponent<Ingredient>());
            }             

            return;
        }

        NEW_Plate plate = other.gameObject.GetComponent<NEW_Plate>();
        NEW_Dish dish = other.gameObject.GetComponent<NEW_Dish>();

        // makes sure that you have both a PLATE & DISH script
        if (dish != null && plate != null)
        {
            CompareDish(dish, plate);
        }
        else
        {
            Debug.LogError($"{other.name} has a missing Plate or Dish script");
            SoundManager.Instance.PlaySound("wrong");
        }
    }
    
    private void test()
    {
        if (!_isDevloperMode) return;

        if (Input.GetKeyDown(KeyCode.Tab))
            Debug.Log($"{this} wanted plater: {Order.WantedPlatter}");        
    }

    #endregion
    #region Helpers

    private void DoIngredientCollision(Ingredient ing)
    {
        // starts the game over logic
        Order.CustomerSR = 0f;
        Destroy(ing.gameObject);
        StartCoroutine(CO_DisableCollider());
        StartCoroutine(GameManager.Instance.CO_GameOver());
        StartCoroutine(Random.value > 0.5f ? Order.CO_DirtyReaction() :
                                             Order.CO_AngryReaction());

        Debug.LogError("Served an INGREDIENT!");
    }
    private void CompareDish(NEW_Dish dish, NEW_Plate plate)
    {
        DoDishCollision(dish); // customer's reaction when getting the dish

        // visual cofirmation that the DISH was served 
        dish.DisableDish(); // removes the food from the plate
        plate.Served(); // increments the use counter & removed the food            

        StartCoroutine(CO_DisableCollider()); // temporarily disables the collider
    }
    private void DoDishCollision(NEW_Dish dish)
    {
        if (dish.FoodCondition != FoodCondition.CLEAN)
        {
            Order.CustomerSR = 0f;
            StartCoroutine(Order.CO_DirtyReaction());
            Debug.LogWarning("Game Over!");

            if (_isTutorial)
                PlayWrongDishServed();
        }
        else if (dish.DishPlatter == Order.WantedPlatter)
        {
            Order.CustomerSR = (dish.Score + Order.PatienceRate) / 2f;
            StartCoroutine(Order.CO_HappyReaction());
            Debug.LogWarning("Happy reaction");

            if (_isTutorial)
                PlayExtraOnboarding();
        }
        else
        {
            Order.CustomerSR = 0f;
            StartCoroutine(Order.CO_AngryReaction());
            Debug.LogWarning("Angy reaction");

            if (_isTutorial)
                PlayWrongDishServed();
        }
    }
    private void PlayExtraOnboarding()
    {
        OnBoardingHandler.Instance.AddOnboardingIndex();
        OnBoardingHandler.Instance.PlayOnboarding();

        if (Order.IsTunaCustomer)
        {
            ShopManager.Instance.ClearList();
            Debug.LogWarning("Benny was served!");
            
            // in case we find a timing defect for the onboarding
            // DisableTutorial();
        }
        else Debug.LogWarning("Atrium was served!");
    }
    private void PlayWrongDishServed()
    {
        SoundManager.Instance.PlaySound("wrong order");
        Debug.LogWarning("Player served the wrong dish to the customer!");
    }

    public void DisableTutorial() => _isTutorial = false;
    
    #endregion
    
    #region Enumerators

    private IEnumerator CO_DisableCollider()
    {
        _collider.enabled = false;
        Debug.LogWarning("Collider disabled!");
        yield return new WaitForSeconds(_disableTimer);

        _collider.enabled = true;
        Debug.LogWarning("Collider enabled!");

        Order = null;
        Debug.LogWarning("CustomerOrder is now null!");
    }

    #endregion
}
