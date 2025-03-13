using System.Xml.Schema;
using TMPro;
using UnityEngine;


[System.Serializable]
public struct OrderUI
{
    [Header("Spawning")]
    public GameObject _orderPrefab;
    public Transform _spawnpoint;

    [Header("Counts & Prices")]
    public int _stockCount;   // # of ingredients in your inventory
    public int _toOrderCount; // # of ingredients to buy
    public float _stockPrice, _totalPrice;

    [Header("UI Texts")]
    public TextMeshProUGUI _stockCountTxt;
    public TextMeshProUGUI _toOrderCountTxt;
    public TextMeshProUGUI _priceTxt, _totalPriceTxt;
}


public class OrderInventoryUI : MonoBehaviour
{
    #region Members

    [Header("Ingredient UI Components"), Tooltip("0 = salmon, 1 = tuna, 2 = rice")]
    [SerializeField] OrderUI[] IngredientOrders;
    
    [SerializeField] TextMeshProUGUI _playerMoneyTxt, _totalPriceTxt;

    const int MAX_STOCK_COUNT = 99;
    float _totalPrice;

    /* -OLD UI VARIABLES -
    [Header("Prefabs to Spawn")]
    [SerializeField] private GameObject _ricePrefab;
    [SerializeField] private GameObject _salmonSlabPrefab, _tunaSlabPrefab;

    [Header("Spawnpoints")]
    [SerializeField] private Transform _riceSpawnpoint;
    [SerializeField] private Transform _fishSpawnpoint; 

    // [Header("Player Money")]
    // [SerializeField] private TextMeshProUGUI _playerMoneyTxt;
    // [SerializeField] private int startPlayerMoney;
    // [SerializeField] private int currentPlayerMoney;

    /*
    [Header("Ingredient Stock Count")] // items in inventory
    [SerializeField] private float _currentRiceStock; //I left this as float because Idk maybe we indicate per 0.1 decrements per grab of rice?
    [SerializeField] private int _currentSalmonStock, _currentTunaStock;
    [SerializeField] private TextMeshProUGUI _currentSalmonStockTxt, _currentTunaStockTxt, _currentRiceStockTxt;

    [Header("To-Order-Ingredient Count")]
    [SerializeField] private int _riceOrderCount;
    [SerializeField] private int _salmonSlabOrderCount, _tunaSlabOrderCount;
    [SerializeField] private TextMeshProUGUI _salmonOrderCountTxt, _tunaOrderCountTxt, _riceOrderCountTxt; 
    
    [Header("Ingredient Prices")]
    [SerializeField] private int salmonSlabPrice;
    [SerializeField] private TextMeshProUGUI txtSalmonSlabPrice;
    [SerializeField] private int tunaSlabPrice;
    [SerializeField] private TextMeshProUGUI txtTunaSlabPrice;
    [SerializeField] private int ricePrice;
    [SerializeField] private TextMeshProUGUI txtRicePrice; 
    
    //----------------------------------------------------------------//
    [Header("TotalPrice")]
    [SerializeField] private int salmonOrderTotalPrice;
    [SerializeField] private TextMeshProUGUI txtSalmonTotalPrice;
    [SerializeField] private int tunaOrderTotalPrice;
    [SerializeField] private TextMeshProUGUI txtTunaTotalPrice;
    [SerializeField] private int riceOrderTotalPrice;
    [SerializeField] private TextMeshProUGUI txtRiceTotalPrice;
    [SerializeField] private int totalPrice;
    [SerializeField] private TextMeshProUGUI txtTotalPrice; /**/



    #endregion

    // Start is called before the first frame update
    void Start()
    {
        // currentPlayerMoney = startPlayerMoney;
        // GameManager.Instance.AddMoney(10000f); // +10k  

        // buy button -> do order supplies
    }

    // Update is called once per frame
    /*
    void Update()
    {
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
        //txtTotalPrice.text = totalPrice.ToString();
    }

#region Public

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

    public void IncreaseSalmonSlabOrder()
    {
        _salmonSlabOrderCount++;

        Mathf.Clamp(_salmonSlabOrderCount, 0, 99);
    }

    public void DecreaseSalmonSlabOrder()
    {
        if (_salmonSlabOrderCount > 0)
        {
            _salmonSlabOrderCount--;
        }

        Mathf.Clamp(_salmonSlabOrderCount, 0, _salmonSlabOrderCount);
    }

    public void IncreaseTunaSlabOrder()
    {
        _tunaSlabOrderCount++;

        Mathf.Clamp(_tunaSlabOrderCount, 0, 99);
    }

    public void DecreaseTunaSlabOrder()
    {
        if (_tunaSlabOrderCount > 0)
        {
            _tunaSlabOrderCount--;
        }

        Mathf.Clamp(_tunaSlabOrderCount, 0, _tunaSlabOrderCount);
    }

    public void IncreaseRiceAmount()
    {
        _riceOrderCount++;

        Mathf.Clamp(_riceOrderCount, 0, 1);
    }

    public void DecreaseRiceAmount()
    {
        if (_riceOrderCount > 0)
        {
            _riceOrderCount--;
        }

        Mathf.Clamp(_riceOrderCount, 0, _riceOrderCount);
    }
#endregion

    #region Private

    private int CalculateTotalPrice()
    {
        totalPrice = (_salmonSlabOrderCount * salmonSlabPrice) + 
                     (_tunaSlabOrderCount * tunaSlabPrice)     +
                     (_riceOrderCount * ricePrice)               ;

        return totalPrice;
    }

    private int CalculateSalmonTotalPrice()
    {
        salmonOrderTotalPrice = (_salmonSlabOrderCount * salmonSlabPrice);

        return salmonOrderTotalPrice;
    }

    private int CalculateTunaTotalPrice()
    {
        tunaOrderTotalPrice = (_tunaSlabOrderCount * tunaSlabPrice);

        return tunaOrderTotalPrice;
    }

    private int CalculateRiceTotalPrice()
    {
        riceOrderTotalPrice = (_riceOrderCount * ricePrice);

        return riceOrderTotalPrice;
    }

#endregion

    */
    
}
