using UnityEngine;
using TMPro;

public class NEW_OrderInventoryUI : Singleton<NEW_OrderInventoryUI> 
{
    protected override void Awake() => base.Awake();
    protected override void OnApplicationQuit() => base.OnApplicationQuit();

    [Header("GameObjects")]
    [SerializeField] private GameObject salmonSlabPrefab;
    [SerializeField] private GameObject tunaSlabPrefab;
    [SerializeField] private Transform salmonSpawnPoint;
    [SerializeField] private Transform tunaSpawnPoint;

    [Header("PlayerCurrency")]
    //[SerializeField] private int startPlayerMoney;
    [SerializeField] public float currentPlayerMoney;
    [SerializeField] private TextMeshProUGUI txtCurrentPlayerMoney;

    [Header("IngredientPrice")]
    [SerializeField] private int salmonSlabPrice;
    [SerializeField] private TextMeshProUGUI txtSalmonSlabPrice;
    [SerializeField] private int tunaSlabPrice;
    [SerializeField] private TextMeshProUGUI txtTunaSlabPrice;
    [SerializeField] private int ricePrice;
    [SerializeField] private TextMeshProUGUI txtRicePrice;
   
    void Start()
    {
        txtCurrentPlayerMoney.text = currentPlayerMoney.ToString();
        
        // Ingredient prices
        txtSalmonSlabPrice.text = salmonSlabPrice.ToString();
        txtTunaSlabPrice.text = tunaSlabPrice.ToString();
        txtRicePrice.text = ricePrice.ToString();
    }

    private void DeductPlayerMoney(int priceOfItem)
    {
        if (currentPlayerMoney >= priceOfItem)
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
}
