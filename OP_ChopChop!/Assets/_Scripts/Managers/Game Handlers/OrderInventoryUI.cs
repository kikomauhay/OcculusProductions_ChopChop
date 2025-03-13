using UnityEngine;
using TMPro;

public class OrderInventoryUI : MonoBehaviour
{
#region Members

    [Header("Ingredient UI Components"), Tooltip("0 = salmon, 1 = tuna, 2 = rice")]
    [SerializeField] OrderUI[] _ingredientOrders;
    
    [SerializeField] TextMeshProUGUI _playerMoneyTxt, _totalPriceTxt;

    const int MAX_ORDER_COUNT = 99;
    float _totalPrice;

#endregion

    // Start is called before the first frame update
    void Start()
    {
        // currentPlayerMoney = startPlayerMoney;
        // GameManager.Instance.AddMoney(10000f); // +10k  

        // buy button -> do order supplies
    }

    // Update is called once per frame
    
    void Update()
    {
        /*
        //player currency
        _playerMoneyTxt.text = $"{GameManager.Instance.AvailableMoney}";

        //IngredientStockCount
        _currentSalmonStockTxt.text = _currentSalmonStock.ToString();
        _currentTunaStockTxt.text = _currentTunaStock.ToString();
        _currentRiceStockTxt.text = _currentRiceStock.ToString();

        //ToOrderIngredientCount
        _salmonOrderCountTxt.text = _salmonSlabOrderCount.ToString();
        _tunaOrderCountTxt.text = _tunaSlabOrderCount.ToString();
        _riceOrderCountTxt.text = _riceOrderCount.ToString();

        //IngredientPrice
        txtSalmonSlabPrice.text = salmonSlabPrice.ToString();
        txtTunaSlabPrice.text = tunaSlabPrice.ToString();
        txtRicePrice.text = ricePrice.ToString();

        //TotalPrice
        txtSalmonTotalPrice.text = CalculateSalmonTotalPrice().ToString();
        txtTunaTotalPrice.text = CalculateTunaTotalPrice().ToString();
        txtRiceTotalPrice.text = CalculateRiceTotalPrice().ToString();
        _totalPriceTxt.text = CalculateTotalPrice().ToString();
        //txtTotalPrice.text = totalPrice.ToString(); */
    }

#region UI_Updates

    void UpdateIngredientToBuyCount(IngredientType type) => 
        _ingredientOrders[(int)type].StockCountTxt.text = $"{_ingredientOrders[(int)type].StockCount}";

    void UpdateTotalPriceCount(IngredientType type) =>
        _ingredientOrders[(int)type].TotalPriceTxt.text = $"{_ingredientOrders[(int)type].TotalPrice}";

    void UpdatePriceCount(IngredientType type) =>
        _ingredientOrders[(int)type].PriceTxt.text = $"{_ingredientOrders[(int)type].StockPrice}";

    void UpdateIngredientStockCount(IngredientType type) =>
        _ingredientOrders[(int)type].StockCountTxt.text = $"{_ingredientOrders[(int)type].StockCount}";

    void UpdatePlayerMoney() =>
        _playerMoneyTxt.text = $"{GameManager.Instance.AvailableMoney}";

#endregion


    #region Public
    /*
    public void DoOrderSupplies()
    {
        if(GameManager.Instance.AvailableMoney >= totalPrice) 
        { 
            ToOrderSupplies();
            GameManager.Instance.DeductMoney(totalPrice);
        }
    }

    public void ToOrderSupplies()
    {
        if (_salmonSlabOrderCount > 0)
        {
            for (int i = 0; i < _salmonSlabOrderCount; i++)
            {
                Instantiate(_salmonSlabPrefab, _fishSpawnpoint.transform.localPosition, _fishSpawnpoint.transform.rotation);
            }
        }

        if (_tunaSlabOrderCount > 0)
        {
            for (int i = 0; i < _tunaSlabOrderCount; i++)
            {
                Instantiate(_tunaSlabPrefab, _fishSpawnpoint.transform.localPosition, _fishSpawnpoint.transform.rotation);
            }
        }

        if(_riceOrderCount > 0)
        {
            //Insert Instantiate code here
        }

        // Reset order counts after spawning
        _salmonSlabOrderCount = 0;
        _tunaSlabOrderCount = 0;
        _riceOrderCount = 0;
    }
    */

    public void ChangeOrderCount(IngredientType type, bool isAdd)
    {
        int count = _ingredientOrders[(int)type].ToOrderCount;

        if (isAdd)
        {
            count++;
            Mathf.Clamp(count, 0, MAX_ORDER_COUNT);
        }
        else
        {
            count--;
            Mathf.Clamp(count, 0, count);
        }
    }
   
#endregion

#region Private

    float CalculateTotalPrice()
    {
        float totalPrice = 0f;

        foreach (OrderUI order in _ingredientOrders)
            totalPrice += order.CalculatePrice();

        if (totalPrice > 0f)
            return totalPrice;

        else return 0f;        
    }

#endregion
}

[System.Serializable]
public struct OrderUI
{
    [Header("Spawning")]
    public GameObject OrderPrefab;
    public Transform Spawnpoint;

    [Header("Counts & Prices")]
    public int StockCount;   // # of ingredients in your inventory
    public int ToOrderCount; // # of ingredients to buy
    public float StockPrice, TotalPrice;

    [Header("UI Texts")]
    public TextMeshProUGUI StockCountTxt;
    public TextMeshProUGUI ToOrderCountTxt;
    public TextMeshProUGUI PriceTxt, TotalPriceTxt;

    public float CalculatePrice()
    {
        if (ToOrderCount > 1)
            return StockPrice * ToOrderCount;

        else return 0f;
    }
}