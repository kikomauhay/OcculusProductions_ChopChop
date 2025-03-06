using UnityEngine;
using TMPro;

public enum ShoppingCart { SALMON, TUNA, RICE, SEAWEED } 

public class ShopManager : Singleton<ShopManager>
{
#region Members   

    [SerializeField] Transform _orderSpawnPoint;
    [SerializeField] GameObject _salmonPrefab, _tunaPrefab;
    [SerializeField] TextMeshProUGUI _playerMoneyText;

    [Header("Prices UI")]
    [SerializeField] TextMeshProUGUI _totalCostText;
    [SerializeField] TextMeshProUGUI _salmonCostText, _tunaCostText; 
    
    [Header("Order Amount UI")]
    [SerializeField] TextMeshProUGUI _salmonOrderAmountText;
    [SerializeField] TextMeshProUGUI _tunaOrderAmountText;

    // SHOP VARIABLES
    int _salmonOrderCount, _tunaOrderCount;
    float _salmonPrice, _tunaPrice, _totalPrice;
    const int ORDER_THRESHOLD = 99;

#endregion

    protected override void Awake() => base.Awake();
    protected override void OnApplicationQuit() => base.OnApplicationQuit();
    
    void Start()
    {
        _tunaPrice = 0f;
        _totalPrice = 0f;
        _salmonPrice = 100f;   
        _tunaOrderCount = 0;
        _salmonOrderCount = 0;

        GameManager.Instance.OnMoneyChanged?.Invoke(10000f);

        UpdatePlayerMoneyUI();
        UpdateSalmonCountUI();
        UpdateTotalPriceUI();
        UpdateTunaCountUI();
    }

    public void DoOrderSupplies()
    {
        if (GameManager.Instance.AvailableMoney >= _totalPrice) 
        { 
            ToOrderSupplies();
            GameManager.Instance.OnMoneyChanged?.Invoke(-_totalPrice);
            Debug.LogWarning($"Available cash: ${GameManager.Instance.AvailableMoney}");
        }
    }
    public void ToOrderSupplies()
    {
        // need to randomize positions a bit
        // so they don't stack and make a mess

        if (_salmonOrderCount > 0)
        {
            for (int i = 0; i < _salmonOrderCount; i++)
            {
                SpawnManager.Instance.SpawnFoodItem(_salmonPrefab, 
                                                    SpawnObjectType.FOOD,
                                                    _orderSpawnPoint);
            }
        }
        if (_tunaOrderCount > 0)
        {
            for (int i = 0; i < _tunaOrderCount; i++)
            {
                SpawnManager.Instance.SpawnFoodItem(_tunaPrefab, 
                                                    SpawnObjectType.FOOD,
                                                    _orderSpawnPoint);
            }
        }

        // reset order counts after spawning
        _salmonOrderCount = 0;
        _tunaOrderCount = 0;
        UpdateSalmonCountUI();
        UpdateTunaCountUI();
    }

    float CalculateTotalPrice()
    {
        return (_salmonOrderCount * _salmonPrice) + 
               (_tunaOrderCount * _tunaPrice);
    }

#region Button_Methods

    public void IncreaseOrder(ShoppingCart type)
    {
        switch (type)
        {
            case ShoppingCart.SALMON:
                _salmonOrderCount++;
                Mathf.Clamp(_salmonOrderCount, 0, ORDER_THRESHOLD);
                UpdateSalmonCountUI();
                break;

            default: break;
        }
    }
    public void DecreaseOrder(ShoppingCart type)
    {
        switch (type)
        {
            case ShoppingCart.SALMON:
                if (_salmonOrderCount > 0)
                {
                    _salmonOrderCount--;
                    Mathf.Clamp(_salmonOrderCount, 0, _salmonOrderCount);
                    UpdateSalmonCountUI();
                }
                break;

            default: break;
        }
    }

    public void IncreaseSalmonSlabOrder()
    {
        _salmonOrderCount++;
        UpdateSalmonCountUI();

        Mathf.Clamp(_salmonOrderCount, 0, ORDER_THRESHOLD);
    }

    public void DecreaseSalmonOrder()
    {
        if (_salmonOrderCount > 0)
        {
            _salmonOrderCount--;
            UpdateSalmonCountUI();
        }

        Mathf.Clamp(_salmonOrderCount, 0, _salmonOrderCount);
    }

    public void IncreaseTunaOrder()
    {
        _tunaOrderCount++;

        Mathf.Clamp(_tunaOrderCount, 0, ORDER_THRESHOLD);
    }

    public void DecreaseTunaOrder()
    {
        if (_tunaOrderCount > 0)
        {
            _tunaOrderCount--;
        }

        Mathf.Clamp(_tunaOrderCount, 0, _salmonOrderCount);
    }

#endregion

#region UI_Methods

    void UpdateSalmonCountUI() => 
        _salmonOrderAmountText.text = $"{_salmonOrderCount}";
    
    void UpdateTunaCountUI() => 
        _tunaOrderAmountText.text = $"{_tunaOrderCount}";

    void UpdatePlayerMoneyUI() =>
        _playerMoneyText.text = $"${GameManager.Instance.AvailableMoney}";

    void UpdateTotalPriceUI() => 
        _totalCostText.text = $"${CalculateTotalPrice()}";

#endregion
}