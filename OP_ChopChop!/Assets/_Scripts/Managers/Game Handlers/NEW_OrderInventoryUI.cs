using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NEW_OrderInventoryUI : Singleton<NEW_OrderInventoryUI> 
{
    protected override void Awake() => base.Awake();
    protected override void OnApplicationQuit() => base.OnApplicationQuit();

    [Header("GameObjects")]
    [SerializeField] private GameObject salmonSlabPrefab;
    [SerializeField] private GameObject tunaSlabPrefab;
    [SerializeField] private Transform salmonSpawnPoint;
    [SerializeField] private Transform tunaSpawnPoint;
    //----------------------------------------------------------------//
    [Header("PlayerCurrency")]
    //[SerializeField] private int startPlayerMoney;
    [SerializeField] public float currentPlayerMoney;
    [SerializeField] private TextMeshProUGUI txtCurrentPlayerMoney;
    //----------------------------------------------------------------//
    #region CommentedOutCode
    /*
    [Header("IngredientStockCount")]
    [SerializeField] private int currentSalmonStock;
    [SerializeField] private TextMeshProUGUI txtCurrentSalmonStockCount;
    [SerializeField] private int currentTunaStock;
    [SerializeField] private TextMeshProUGUI txtCurrentTunaStockCount;
    [SerializeField] private float currentRiceStock; //I left this as float because Idk maybe we indicate per 0.1 decrements per grab of rice?
    [SerializeField] private TextMeshProUGUI txtCurrentRiceStockCount;
   
    //----------------------------------------------------------------//
    [Header("ToOrderIngredientCount")]
    [SerializeField] private int salmonSlabOrderCount;
    [SerializeField] private TextMeshProUGUI txtSalmonOrderCount;
    [SerializeField] private int tunaSlabOrderCount;
    [SerializeField] private TextMeshProUGUI txtTunaOrderCount;
    [SerializeField] private int riceOrderCount;
    [SerializeField] private TextMeshProUGUI txtRiceOrderCount;


     [Header("TotalPrice")]
    [SerializeField] private int salmonOrderTotalPrice;
    [SerializeField] private TextMeshProUGUI txtSalmonTotalPrice;
    [SerializeField] private int tunaOrderTotalPrice;
    [SerializeField] private TextMeshProUGUI txtTunaTotalPrice;
    [SerializeField] private int riceOrderTotalPrice;
    [SerializeField] private TextMeshProUGUI txtRiceTotalPrice;
    [SerializeField] private int totalPrice;
    [SerializeField] private TextMeshProUGUI txtTotalPrice;
     */
    #endregion
    //----------------------------------------------------------------//
    [Header("IngredientPrice")]
    [SerializeField] private int salmonSlabPrice;
    [SerializeField] private TextMeshProUGUI txtSalmonSlabPrice;
    [SerializeField] private int tunaSlabPrice;
    [SerializeField] private TextMeshProUGUI txtTunaSlabPrice;
    [SerializeField] private int ricePrice;
    [SerializeField] private TextMeshProUGUI txtRicePrice;
    //----------------------------------------------------------------//
   

    // Start is called before the first frame update
    void Start()
    {
        txtCurrentPlayerMoney.text = currentPlayerMoney.ToString();
        //IngredientPrice
        txtSalmonSlabPrice.text = salmonSlabPrice.ToString();
        txtTunaSlabPrice.text = tunaSlabPrice.ToString();
        txtRicePrice.text = ricePrice.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        #region CommentedOUtUpdateCode
        /*
        // Ingredient Stock Count
        txtCurrentSalmonStockCount.text = currentSalmonStock.ToString();
        txtCurrentTunaStockCount.text = currentTunaStock.ToString();
        txtCurrentRiceStockCount.text = currentRiceStock.ToString();
      
        // Total Price
        txtSalmonTotalPrice.text = CalculateSalmonTotalPrice().ToString();
        txtTunaTotalPrice.text = CalculateTunaTotalPrice().ToString();
        txtRiceTotalPrice.text = CalculateRiceTotalPrice().ToString();
        txtTotalPrice.text = CalculateTotalPrice().ToString();

        // Ingredient Order Count
        txtSalmonOrderCount.text = $"{salmonSlabOrderCount}";
        txtTunaOrderCount.text = $"{tunaSlabOrderCount}";
        txtRiceOrderCount.text = $"{riceOrderCount}";
        */
        #endregion
    }

    private void DeductPlayerMoney(int priceOfItem)
    {
        if(currentPlayerMoney >= priceOfItem)
        {
            currentPlayerMoney -= priceOfItem;
        }
        else
        {
            Debug.Log("NO MONEH");
        }
       
        txtCurrentPlayerMoney.text = currentPlayerMoney.ToString();
    }

    public void BuySalmon()
    {
        DeductPlayerMoney(salmonSlabPrice);
        Instantiate(salmonSlabPrefab, salmonSpawnPoint.transform.localPosition, salmonSpawnPoint.transform.rotation);
    }

    public void BuyTuna()
    {
        DeductPlayerMoney(tunaSlabPrice);
        Instantiate(tunaSlabPrefab, salmonSpawnPoint.transform.localPosition, salmonSpawnPoint.transform.rotation);
    }

    public void BuyRice()
    {
        DeductPlayerMoney(ricePrice);
        //insert code here to reset Rice at rice cooker
    }

    #region CommentedOutCode
    /*
    public void DoOrderSupplies()
    {
        if (currentPlayerMoney >= totalPrice)
        {
            ToOrderSupplies();
            currentPlayerMoney -= totalPrice;

        }
    }

    public void ToOrderSupplies()
    {
        if (salmonSlabOrderCount > 0)
        {
            for (int i = 0; i < salmonSlabOrderCount; i++)
            {
                Instantiate(salmonSlabPrefab, salmonSpawnPoint.transform.localPosition, salmonSpawnPoint.transform.rotation);
            }
        }

        if (tunaSlabOrderCount > 0)
        {
            for (int i = 0; i < tunaSlabOrderCount; i++)
            {
                Instantiate(tunaSlabPrefab, salmonSpawnPoint.transform.localPosition, salmonSpawnPoint.transform.rotation);
            }
        }

        // Reset order counts after spawning
        salmonSlabOrderCount = 0;
        tunaSlabOrderCount = 0;
        riceOrderCount = 0;

        txtCurrentPlayerMoney.text = currentPlayerMoney.ToString();
    }

    private int CalculateTotalPrice()
    {
        totalPrice = (salmonSlabOrderCount * salmonSlabPrice) +
                     (tunaSlabOrderCount * tunaSlabPrice) +
                     (riceOrderCount * ricePrice);

        return totalPrice;
    }

    private int CalculateSalmonTotalPrice()
    {
        salmonOrderTotalPrice = (salmonSlabOrderCount * salmonSlabPrice);


        return salmonOrderTotalPrice;
    }

    private int CalculateTunaTotalPrice()
    {
        tunaOrderTotalPrice = (tunaSlabOrderCount * tunaSlabPrice);

        return tunaOrderTotalPrice;
    }

    private int CalculateRiceTotalPrice()
    {
        riceOrderTotalPrice = (riceOrderCount * ricePrice);

        return riceOrderTotalPrice;
    }


    public void IncreaseSalmonSlabOrder()
    {
        salmonSlabOrderCount++;

        Mathf.Clamp(salmonSlabOrderCount, 0, 99);

        txtSalmonOrderCount.text = salmonSlabOrderCount.ToString();
    }

    public void DecreaseSalmonSlabOrder()
    {
        if (salmonSlabOrderCount > 0)
        {
            salmonSlabOrderCount--;
        }

        Mathf.Clamp(salmonSlabOrderCount, 0, salmonSlabOrderCount);

        txtSalmonOrderCount.text = salmonSlabOrderCount.ToString();
    }

    public void IncreaseTunaSlabOrder()
    {
        tunaSlabOrderCount++;

        Mathf.Clamp(tunaSlabOrderCount, 0, 99);

        txtTunaOrderCount.text = tunaSlabOrderCount.ToString();
    }

    public void DecreaseTunaSlabOrder()
    {
        if (tunaSlabOrderCount > 0)
        {
            tunaSlabOrderCount--;
        }

        Mathf.Clamp(tunaSlabOrderCount, 0, tunaSlabOrderCount);

        txtTunaOrderCount.text = tunaSlabOrderCount.ToString();
    }

    public void IncreaseRiceAmount()
    {
        riceOrderCount++;

        Mathf.Clamp(riceOrderCount, 0, 1);

        txtRiceOrderCount.text = riceOrderCount.ToString();
    }

    public void DecreaseRiceAmount()
    {
        if (riceOrderCount > 0)
        {
            riceOrderCount--;
        }

        Mathf.Clamp(riceOrderCount, 0, riceOrderCount);
    }
    */
    #endregion
}
