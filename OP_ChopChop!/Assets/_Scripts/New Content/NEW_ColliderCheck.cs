using System.Collections;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class NEW_ColliderCheck : MonoBehaviour
{
    #region Properties

    public CustomerOrder CustomerOrder { get; set; }

    #endregion
    #region Private

    [SerializeField] private bool _isTutorial;
    [SerializeField] private float _dishPercentage;
    [SerializeField] private float _patiencePercentage;
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
        GameManager.Instance.OnStartService += DisableTutorial;

        _collider.isTrigger = true;
        _collider.enabled = true;

        _dishPercentage = 1f;
        _patiencePercentage = 1f; 
        _disableTimer = 3f;
    }
    private void Update() => test();
    private void OnTriggerEnter(Collider other)
    {
        if (CustomerOrder == null)
        {
            Debug.LogError($"{CustomerOrder} is null!");
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
            // Debug.LogWarning($"Collided with {other.gameObject.name}");
            // Debug.LogWarning("Finished dish collision!");

            if (GameManager.Instance.CurrentShift == GameShift.Training)
            {
                dish.DisableDish();
                plate.Served();
            }
        }
    }
    private void OnDestroy()
    {
        OnBoardingHandler.Instance.OnTutorialEnd -= DisableTutorial;
        GameManager.Instance.OnStartService -= DisableTutorial;
    }

    private void test()
    {
        if (!_isDevloperMode) return;

        if (Input.GetKeyDown(KeyCode.Tab))
            Debug.Log($"{this} wanted plater: {CustomerOrder.WantedPlatter}");
    }

    #endregion
    #region Helpers

    private void DoIngredientCollision(Ingredient ing)
    {
        if (GameManager.Instance.CurrentShift != GameShift.Training)
        {
            CustomerOrder.PatienceScore = 0f;
            StartCoroutine(CO_DisableCollider());
            StartCoroutine(CustomerOrder.CO_AngryReaction());
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
    }
    private void CheckFoodConition(NEW_Dish dish)
    {
        if (dish.FoodCondition != FoodCondition.CLEAN)
        {
            TriggerContainatedOrder();
            Debug.LogError("Triggered dirty order!");
        }
        else if (dish.DishPlatter != CustomerOrder.WantedPlatter)
        {
            TriggerWrongOrder();
            Debug.LogError("Triggered wrong order!");
        }
        else
        {
            TriggerCorrectOrder(dish);

            if (GameManager.Instance.CurrentShift == GameShift.Training)
            {
                CustomerOrder = null;
                TriggerOnboarding();
                Debug.LogWarning("Triggered correct order!");
            }
        }
    }
    private void TriggerContainatedOrder()
    {
        if (GameManager.Instance.CurrentShift != GameShift.Training)
        {
            CustomerOrder.PatienceScore = 0f;
            StartCoroutine(CustomerOrder.CO_DirtyReaction());
            Debug.LogError("Player served a dirty order!");
        }
        else if (_isTutorial)
        {
            SoundManager.Instance.PlaySound("wrong");
            SoundManager.Instance.PlaySound("contaminated order");
        }
    }
    private void TriggerWrongOrder()
    {
        if (GameManager.Instance.CurrentShift != GameShift.Training)
        {
            CustomerOrder.PatienceScore = 0f;
            GameManager.Instance.AddToCustomerScores(CustomerOrder.PatienceScore);
            StartCoroutine(CustomerOrder.CO_AngryReaction());
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
        float dishScore = dish.Score * _dishPercentage;
        float patienceScore = CustomerOrder.PatienceScore * _patiencePercentage;
        CustomerOrder.PatienceScore = dishScore + patienceScore; // dish quality has more focus becuase of CAPSTN
        
        Debug.LogWarning($"{this} Current Score: {CustomerOrder.PatienceScore}!");

        GameManager.Instance.AddToCustomerScores(CustomerOrder.PatienceScore);
        StartCoroutine(CustomerOrder.CO_HappyReaction());
        // Destroy(Order.gameObject);
    }
    private void TriggerOnboarding()
    {
        if (!_isTutorial) return;

        OnBoardingHandler.Instance.AddOnboardingIndex();
        OnBoardingHandler.Instance.PlayOnboarding();

        if (CustomerOrder.IsTunaCustomer)
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
        yield return new WaitForSeconds(_disableTimer);
        _collider.enabled = true;

        if (GameManager.Instance.CurrentShift == GameShift.Service) 
            CustomerOrder = null; // need this here so it doesn't trigger a lot of times 
    }

    #endregion  
}