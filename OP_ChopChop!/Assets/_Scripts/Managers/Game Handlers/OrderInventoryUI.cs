using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class OrderInventoryUI : MonoBehaviour
{
    [Header("GameObjects")]
    [SerializeField] private GameObject salmonSlabPrefab;
    [SerializeField] private GameObject tunaSlabPrefab;
    [SerializeField] private GameObject ricePrefab; //idk if this is it
    [SerializeField] private Transform riceSpawnPoint; //spawn Point for rice, idk kung ano plano ngo
    [SerializeField] private Transform fishSpawnPoint;
    //----------------------------------------------------------------//
    [Header("PlayerCurrency")]
    [SerializeField] private int startPlayerMoney;
    [SerializeField] private int currentPlayerMoney;
    [SerializeField] private TextMeshProUGUI txtCurrentPlayerMoney;
    //----------------------------------------------------------------//
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
    //----------------------------------------------------------------//
    [Header("IngredientPrice")]
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
    [SerializeField] private TextMeshProUGUI txtTotalPrice;

    // Start is called before the first frame update
    void Start()
    {
        currentPlayerMoney = startPlayerMoney;
    }

    // Update is called once per frame
    void Update()
    {
        //player currency
        txtCurrentPlayerMoney.text = currentPlayerMoney.ToString();

        //IngredientStockCount
        txtCurrentSalmonStockCount.text = currentSalmonStock.ToString();
        txtCurrentTunaStockCount.text = currentTunaStock.ToString();
        txtCurrentRiceStockCount.text = currentRiceStock.ToString();

        //ToOrderIngredientCount
        txtSalmonOrderCount.text = salmonSlabOrderCount.ToString();
        txtTunaOrderCount.text = tunaSlabOrderCount.ToString();
        txtRiceOrderCount.text = riceOrderCount.ToString();

        //IngredientPrice
        txtSalmonSlabPrice.text = salmonSlabPrice.ToString();
        txtTunaSlabPrice.text = tunaSlabPrice.ToString();
        txtRicePrice.text = ricePrice.ToString();

        //TotalPrice
        txtSalmonTotalPrice.text = CalculateSalmonTotalPrice().ToString();
        txtTunaTotalPrice.text = CalculateTunaTotalPrice().ToString();
        txtRiceTotalPrice.text = CalculateRiceTotalPrice().ToString();
        txtTotalPrice.text = CalculateTotalPrice().ToString();
        //txtTotalPrice.text = totalPrice.ToString();
    }

    public void DoOrderSupplies()
    {
        if(currentPlayerMoney >= totalPrice) 
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
                Instantiate(salmonSlabPrefab, fishSpawnPoint.transform.localPosition, fishSpawnPoint.transform.rotation);
            }
        }

        if (tunaSlabOrderCount > 0)
        {
            for (int i = 0; i < tunaSlabOrderCount; i++)
            {
                Instantiate(tunaSlabPrefab, fishSpawnPoint.transform.localPosition, fishSpawnPoint.transform.rotation);
            }
        }

        if(riceOrderCount > 0)
        {
            //Insert Instantiate code here
        }

        // Reset order counts after spawning
        salmonSlabOrderCount = 0;
        tunaSlabOrderCount = 0;
        riceOrderCount = 0;
    }

    private int CalculateTotalPrice()
    {
        totalPrice = (salmonSlabOrderCount * salmonSlabPrice) + 
                     (tunaSlabOrderCount * tunaSlabPrice)     +
                     (riceOrderCount * ricePrice)               ;

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
    }

    public void DecreaseSalmonSlabOrder()
    {
        if (salmonSlabOrderCount > 0)
        {
            salmonSlabOrderCount--;
        }

        Mathf.Clamp(salmonSlabOrderCount, 0, salmonSlabOrderCount);
    }

    public void IncreaseTunaSlabOrder()
    {
        tunaSlabOrderCount++;

        Mathf.Clamp(tunaSlabOrderCount, 0, 99);
    }

    public void DecreaseTunaSlabOrder()
    {
        if (tunaSlabOrderCount > 0)
        {
            tunaSlabOrderCount--;
        }

        Mathf.Clamp(tunaSlabOrderCount, 0, tunaSlabOrderCount);
    }

    public void IncreaseRiceAmount()
    {
        riceOrderCount++;

        Mathf.Clamp(riceOrderCount, 0, 1);
    }

    public void DecreaseRiceAmount()
    {
        if(riceOrderCount > 0)
        {
            riceOrderCount--;
        }

        Mathf.Clamp(riceOrderCount, 0, riceOrderCount);
    }
}
