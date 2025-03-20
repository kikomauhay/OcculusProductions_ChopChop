using UnityEngine;
using TMPro;

/// <summary> -WHAT THIS SCRIPT DOES-
/// 
/// Handles any UI related to the Store
/// Anything relating to cost should be here
/// 
/// </summary>

public class TEMP_OrderInventoryUI : MonoBehaviour
{
#region Members

    [Header("Ingredient UI Components"), Tooltip("0 = salmon, 1 = tuna, 2 = rice")]
    [SerializeField] OrderUI[] _ingredientOrders;
    
    [SerializeField] TextMeshProUGUI _playerMoneyTxt, _totalPriceTxt;

    const int MAX_ORDER_COUNT = 99;
    float _sumTotalPrice;

#endregion

    void Start()
    {
        // currentPlayerMoney = startPlayerMoney;
        // GameManager.Instance.AddMoney(10000f); // +10k  

        UpdatePlayerMoney();
        UpdateSumTotalPrice();
    }

#region Update_UI_Methods

    void UpdatePlayerMoney() => 
        _playerMoneyTxt.text = $"{GameManager.Instance.AvailableMoney}";
    
    void UpdateSumTotalPrice() => _totalPriceTxt.text = $"{_sumTotalPrice}";

    void UpdateInventoryCount(IngredientType type) =>
        _ingredientOrders[(int)type].InventoryCountTxt.text = $"{_ingredientOrders[(int)type].InventoryCount}";
    
    void UpdateBuyCount(IngredientType type) => 
        _ingredientOrders[(int)type].InventoryCountTxt.text = $"{_ingredientOrders[(int)type].InventoryCount}";
    
    void UpdateStockPriceCount(IngredientType type) =>
        _ingredientOrders[(int)type].StockPriceTxt.text = $"{_ingredientOrders[(int)type].StockPrice}";

    void UpdateTotalPriceCount(IngredientType type) =>
        _ingredientOrders[(int)type].TotalPriceTxt.text = $"{_ingredientOrders[(int)type].TotalPrice}";

#endregion

#region Button_Methods

    public void BuyIngredients()
    {
        _sumTotalPrice = CalculateTotalPrice();

        if (GameManager.Instance.AvailableMoney < _sumTotalPrice)
        {
            Debug.LogError("YOU DON'T HAVE ENOUGH MONEY!");
            return;
        }

        GameManager.Instance.DeductMoney(_sumTotalPrice);

        SpawnIngredients();
        UpdatePlayerMoney();
    }

    public void IncreaseOrderCount(int choice)
    {
        int count = _ingredientOrders[choice].BuyCount;

        count++;
        Mathf.Clamp(count, 0, MAX_ORDER_COUNT);

        UpdateBuyCount((IngredientType)choice);
        UpdateTotalPriceCount((IngredientType)choice);
        UpdateSumTotalPrice();
    }
    public void DecreaseOrderCount(int choice)
    {
        int count = _ingredientOrders[choice].BuyCount;

        count--;
        Mathf.Clamp(count, 0, count);

        UpdateBuyCount((IngredientType)choice);
        UpdateTotalPriceCount((IngredientType)choice);
        UpdateSumTotalPrice();
    }

#endregion

#region Button_Helpers

    void SpawnIngredients()
    {
        foreach (OrderUI orderUI in _ingredientOrders)
        {
            // skips ingredients that has nothing selected 
            if (orderUI.BuyCount < 1) continue;

            for (int i = 0; i < orderUI.BuyCount; i++)
            {
                SpawnManager.Instance.SpawnObject(orderUI.OrderPrefab, 
                                                  orderUI.Spawnpoint, 
                                                  SpawnObjectType.INGREDIENT);
            }
        }

        // resets the order count after spawning the ingredients
        for (int i = 0; i < _ingredientOrders.Length; i++)
            _ingredientOrders[i].BuyCount = 0;
    }
    float CalculateTotalPrice()
    {
        float totalPrice = 0f;

        foreach (OrderUI order in _ingredientOrders)
            totalPrice += order.CalculateTotalPrice();

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
    public int InventoryCount; // # of ingredients in your inventory
    public int BuyCount;       // # of ingredients to buy
    public float StockPrice, TotalPrice;

    [Header("UI Texts")]
    public TextMeshProUGUI InventoryCountTxt;
    public TextMeshProUGUI BuyCountTxt;
    public TextMeshProUGUI StockPriceTxt, TotalPriceTxt;

    public float CalculateTotalPrice()
    {
        if (BuyCount > 1)
            return StockPrice * BuyCount;

        return 0f;
    }
}