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
        OnBoardingHandler.Instance.OnTutorialEnd += DisableTutorial;

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
        if (dish != null && plate != null)
        {
            DoDishCollision(dish, plate);
            Debug.Log("Finished dish collision!");
        }
    }
    private void OnDestroy() => 
        OnBoardingHandler.Instance.OnTutorialEnd -= DisableTutorial;
    
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
        if (GameManager.Instance.CurrentShift != GameShift.Training)
        {
            Order.CustomerSR = 0f;
            StartCoroutine(CO_DisableCollider());
            StartCoroutine(Order.CO_AngryReaction());
            StartCoroutine(GameManager.Instance.CO_GameOver());
        }
        else 
        {
            SoundManager.Instance.PlaySound("wrong");
            SoundManager.Instance.PlaySound("ingredient order");
            Debug.LogError("Player served a ingredient to the customer!");
        }
    }
    private void DoDishCollision(NEW_Dish dish, NEW_Plate plate)
    {
        CheckFoodConition(dish);
        dish.DisableDish();
        plate.Served();         
        
        StartCoroutine(CO_DisableCollider());
    }
    private void CheckFoodConition(NEW_Dish dish)
    {
        if (dish.FoodCondition != FoodCondition.CLEAN)
        {
            TriggerContainatedOrder();  
            Debug.LogError("Triggered dirty order!");         
        }
        else if (dish.DishPlatter == Order.WantedPlatter) 
        {
            TriggerCorrectOrder(dish);
            TriggerCatOnboarding();
            Debug.LogWarning("Triggered correct order!");         
        }
        else TriggerWrongOrder();
    }
    private void TriggerContainatedOrder()
    {
        if (GameManager.Instance.CurrentShift != GameShift.Training)
        {       
            Order.CustomerSR = 0f;
            StartCoroutine(Order.CO_DirtyReaction());
            Debug.LogError("Player served a dirty order!");        
        }
        else
        {
            SoundManager.Instance.PlaySound("wrong");
            SoundManager.Instance.PlaySound("contaminated order");
        }        
    }
    private void TriggerWrongOrder()
    {
        if (GameManager.Instance.CurrentShift != GameShift.Training)
        {
            Order.CustomerSR = 0f;
            GameManager.Instance.AddToCustomerScores(Order.CustomerSR);
            StartCoroutine(Order.CO_AngryReaction());
            Debug.LogError("Player served the wrong order!");
        }
        else
        {
            SoundManager.Instance.PlaySound("wrong");
            SoundManager.Instance.PlaySound("wrong order");
        }
    }
    private void TriggerCorrectOrder(NEW_Dish dish)
    {
        // UX after serving the customer
        float dishScore = dish.Score * _dishPercantage;
        float patienceScore = Order.PatienceRate * _patiencePercentage;
        Order.CustomerSR = (dishScore + patienceScore) / 2f; // dish quality has more focus becuase of CAPSTN
        
        GameManager.Instance.AddToCustomerScores(Order.CustomerSR);
        StartCoroutine(Order.CO_HappyReaction());
        // Destroy(Order.gameObject);

        Order = null;
        Debug.LogWarning("CustomerOrder is now null!");
    }
    private void TriggerCatOnboarding()
    {
        if (!_isTutorial) return;

        OnBoardingHandler.Instance.AddOnboardingIndex();
        OnBoardingHandler.Instance.PlayOnboarding();

        if (Order.IsTunaCustomer)
        {
            ShopManager.Instance.ClearList();
            Debug.LogWarning("Benny was served!");
        }
        else Debug.LogWarning("Atrium was served!");
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
    }

    #endregion  
}