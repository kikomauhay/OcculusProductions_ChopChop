using System.Collections;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class NEW_ColliderCheck : MonoBehaviour 
{
    #region Properties

    public CustomerOrder Order { get; set; }

    #endregion
    #region Private

    [SerializeField] private bool _isTutorial;
    [SerializeField] private float _dishPercantage = 0.8f;
    [SerializeField] private float _patiencePercentage = 0.2f;
    [SerializeField] private float _disableTimer;

    private Collider _collider;

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
        _disableTimer = 3f; 
    }
    private void Update() => test();
    private void OnTriggerEnter(Collider other)
    {
        if (Order == null)
        {
            Debug.LogError($"{Order} is null!");
            SoundManager.Instance.PlaySound("wrong");
            return;
        }
        if (other.gameObject.GetComponent<Ingredient>() != null)
        {            
            DoIngredientCollision(other.gameObject.GetComponent<Ingredient>());
            return;
        }

        NEW_Plate plate = other.gameObject.GetComponent<NEW_Plate>();
        NEW_Dish dish = other.gameObject.GetComponent<NEW_Dish>();

        // makes sure that you have both a PLATE & DISH script
        if (dish == null)
        {
            Debug.LogError($"{other.name} has a Dish script");
            SoundManager.Instance.PlaySound("wrong");
        }
        else if (plate == null)
        {
            Debug.LogError($"{other.name} has a Plate script");
            SoundManager.Instance.PlaySound("wrong");
        }
        else DoDishCollision(dish, plate);
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
        Destroy(ing.gameObject);

        // the player shouldn't get a Game Over in the tutorial
        if (GameManager.Instance.CurrentShift != GameShift.Training)
        {
            Order.CustomerSR = 0f;
            StartCoroutine(CO_DisableCollider());
            StartCoroutine(Order.CO_AngryReaction());
            StartCoroutine(GameManager.Instance.CO_GameOver());
        }
        else 
        {
            SoundManager.Instance.PlaySound("ingredient order");
            Debug.LogWarning("Player served a ingredient to the customer!");
        }
    }
    private void DoDishCollision(NEW_Dish dish, NEW_Plate plate)
    {
        // customer's reaction when getting the dish
        CheckFoodConition(dish); 
        
        dish.DisableDish();
        plate.Served();          

        StartCoroutine(CO_DisableCollider());
    }
    private void CheckFoodConition(NEW_Dish dish)
    {
        if (dish.FoodCondition != FoodCondition.CLEAN) // contaminared order
        {
            if (!_isTutorial) 
            {
                Order.CustomerSR = 0f;
                StartCoroutine(Order.CO_DirtyReaction());
                // Debug.LogWarning("Game Over!");
            }
            else SoundManager.Instance.PlaySound("contaminated order");
            
            return;
        }
        if (dish.DishPlatter != Order.WantedPlatter) // wrong order
        {   
            if (!_isTutorial)
            {
                Order.CustomerSR = 0f;
                StartCoroutine(Order.CO_AngryReaction());
            }
            else SoundManager.Instance.PlaySound("wrong order");
            
            return;
        }

        // UX after serving the customer
        float dishScore = dish.Score * _dishPercantage;
        float patienceScore = Order.PatienceRate * _patiencePercentage;
        Order.CustomerSR = (dishScore + patienceScore) / 2f; // dish quality has more focus becuase of CAPSTN
        StartCoroutine(Order.CO_HappyReaction());

        if (_isTutorial)
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
