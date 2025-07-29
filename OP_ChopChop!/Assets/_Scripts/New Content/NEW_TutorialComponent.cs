using UnityEngine;

public class NEW_TutorialComponent : MonoBehaviour
{
    #region Members

    public bool IsInteractable => _isInteractable;
    public int TutorialIndex => _tutorialIndex;

    [SerializeField] private bool _isInteractable;
    [SerializeField] private int _tutorialIndex;

    private OnBoardingHandler _onbHandler;

    #endregion
    #region Methods

    private void Start()
    {
        _onbHandler = OnBoardingHandler.Instance;

        if (GameManager.Instance.CurrentShift == GameShift.Service)
            EnableInteraction();
    }

    public void DisableInteraction() => _isInteractable = false;
    public void EnableInteraction() => _isInteractable = true;
    public bool IsCorrectIndex() => 
        OnBoardingHandler.Instance.CurrentStep == TutorialIndex;
    
    #endregion
}

/* public class OLD_ColliderCheck : MonoBehaviour 
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

    private void Update()
    {
        if (!_isDevloperMode) return;

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            Debug.Log($"{this} wanted plater: {Order.WantedPlatter}");
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (Order == null)
        {
            Debug.LogError($"CustomerOrder is null!");
            return;
        }

        if (other.gameObject.GetComponent<Ingredient>() != null)
        {
            // the player shouldn't get a Game Over in the tutorial
            if (GameManager.Instance.CurrentShift == GameShift.Training)
            {
                Debug.LogError("Game over logic not possible in tutorial!");
                return;
            }

            // starts the game over logic
            Order.CustomerSR = 0f;
            Destroy(other.gameObject);
            StartCoroutine(CO_DisableCollider());
            StartCoroutine(GameManager.Instance.CO_GameOver());
            StartCoroutine(Random.value > 0.5f ? Order.CO_DirtyReaction() :
                                                 Order.CO_AngryReaction());

            // test debug to confirm all logic has worked
            Debug.LogError("Served an INGREDIENT!");
            return;
        }

        NEW_Plate plate = other.gameObject.GetComponent<NEW_Plate>();
        NEW_Dish dish = other.gameObject.GetComponent<NEW_Dish>();

        // makes sure that you have both a PLATE & DISH script
        if (plate != null && dish != null)
        {
            DoDishCollision(dish); // customer's reaction when getting the dish

            // visual cofirmation that the DISH was served 
            dish.DisableDish(); // removes the food from the plate
            plate.Served(); // increments the use counter & removed the food            

            StartCoroutine(CO_DisableCollider()); // temporarily disables the collider
        }
        else
        {
            Debug.LogError($"{other.name} has a missing Plate or Dish script");
            return;
        }

        if (_isTutorial)
        {
            OnBoardingHandler.Instance.AddOnboardingIndex();
            OnBoardingHandler.Instance.PlayOnboarding();

            if (Order.IsTunaCustomer)
            {
                ShopManager.Instance.ClearList();
                Debug.LogWarning("Benny was served!");
            }
            else Debug.LogWarning("Atrium was served!");
        }
    }
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

    #region Helpers
    
    private void DoDishCollision(NEW_Dish dish)
    {
        if (dish.FoodCondition != FoodCondition.CLEAN)
        {
            Order.CustomerSR = 0f;
            StartCoroutine(Order.CO_DirtyReaction());

            Debug.LogWarning("Game Over!");
        }
        else if (dish.DishPlatter == Order.WantedPlatter)
        {
            Order.CustomerSR = (dish.Score + Order.PatienceRate) / 2f;
            StartCoroutine(Order.CO_HappyReaction());

            Debug.LogWarning("Happy reaction");
        }
        else
        {
            Order.CustomerSR = 0f;
            StartCoroutine(Order.CO_AngryReaction());

            Debug.LogWarning("Angy reaction");
        }
    }
    public void DisableTutorial() => _isTutorial = false;
    
    #endregion
}
*/