using UnityEngine;
using TMPro;

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

        // GameManager.Instance.AddMoney(10000f); // +10k  

        UpdateAllUI();
    }

#region Shop

    public void DoOrderSupplies()
    {
        if (GameManager.Instance.CurrentPlayerMoney >= _totalPrice) 
        { 
            ToOrderSupplies();
            GameManager.Instance.DeductMoney(_totalPrice);
            Debug.LogWarning($"Available cash: ${GameManager.Instance.CurrentPlayerMoney}");
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
                SpawnManager.Instance.SpawnObject(_salmonPrefab, 
                                                  _orderSpawnPoint,
                                                  SpawnObjectType.INGREDIENT);

            }
        }
        if (_tunaOrderCount > 0)
        {
            for (int i = 0; i < _tunaOrderCount; i++)
            {
                SpawnManager.Instance.SpawnObject(_tunaPrefab,                                        
                                                  _orderSpawnPoint,
                                                  SpawnObjectType.INGREDIENT);
            }
        }

        // reset order counts after spawning
        _salmonOrderCount = 0;
        _tunaOrderCount = 0;
        UpdateFishCountUI();
    }

    float CalculateTotalPrice()
    {
        return (_salmonOrderCount * _salmonPrice) + 
               (_tunaOrderCount * _tunaPrice);
    }

#endregion

#region Buttons

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

    public void IncreaseFishOrder(bool isTuna)
    {        
        if (isTuna)
        {
            _tunaOrderCount++;
            Mathf.Clamp(_tunaOrderCount, 0, ORDER_THRESHOLD);
        }
        else
        {
            _salmonOrderCount++;
            Mathf.Clamp(_salmonOrderCount, 0, ORDER_THRESHOLD);
        }

        UpdateFishCountUI();
    }
    public void DecreaseFishOrder(bool isTuna)
    {

        if (isTuna)
        {
            _tunaOrderCount--;
            Mathf.Clamp(_tunaOrderCount, 0, _tunaOrderCount);
        }
        else
        {
            _salmonOrderCount--;
            Mathf.Clamp(_salmonOrderCount, 0, _salmonOrderCount);
        }
        
            
        UpdateFishCountUI();
    }

#endregion

#region UI_Methods

    void UpdateAllUI()
    {
        UpdateSalmonCountUI();
        UpdateTunaCountUI();
        UpdatePlayerMoneyUI();
        UpdateTotalPriceUI();
    }
    void UpdateFishCountUI()
    {
        UpdateSalmonCountUI();
        UpdateTunaCountUI();
    }

    void UpdateSalmonCountUI() => 
        _salmonOrderAmountText.text = $"{_salmonOrderCount}";
    
    void UpdateTunaCountUI() => 
        _tunaOrderAmountText.text = $"{_tunaOrderCount}";

    void UpdatePlayerMoneyUI() =>
        _playerMoneyText.text = $"${GameManager.Instance.CurrentPlayerMoney}";

    void UpdateTotalPriceUI() => 
        _totalCostText.text = $"${CalculateTotalPrice()}";

#endregion
}