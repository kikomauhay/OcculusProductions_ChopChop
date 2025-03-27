using UnityEngine;
using TMPro;

public class ShopManager : Singleton<ShopManager> 
{
#region Members

    [SerializeField] GameObject _salmonPrefab, _tunaPrefab;
    [SerializeField] Transform _salmonTransform, _tunaTransform;

    [Header("Ingredient Prices")]
    [SerializeField] int _ricePrice;
    [SerializeField] int _salmonPrice, _tunaPrice;

    [Header("Ingredient UI")]
    [SerializeField] TextMeshProUGUI _txtRicePrice;
    [SerializeField] TextMeshProUGUI _txtSalmonPrice, _txtTunaPrice;

    [SerializeField] TextMeshProUGUI _txtPlayerMoney;

#endregion

#region Unity_Methods

    protected override void Awake() => base.Awake();
    protected override void OnApplicationQuit() => base.OnApplicationQuit();
    void Start()
    {
        // Ingredient prices
        _txtSalmonPrice.text = _salmonPrice.ToString();
        _txtTunaPrice.text = _tunaPrice.ToString();
        _txtRicePrice.text = _ricePrice.ToString();

        _txtPlayerMoney.text = GameManager.Instance.CurrentPlayerMoney.ToString();
    }

#endregion

#region Buying

    public void BuySalmon()
    {
        GameManager.Instance.DeductMoney(_salmonPrice);
        SpawnManager.Instance.SpawnObject(_salmonPrefab, 
                                          _salmonTransform.transform, 
                                          SpawnObjectType.INGREDIENT);
    }
    public void BuyTuna()
    {
        GameManager.Instance.DeductMoney(_tunaPrice);
        SpawnManager.Instance.SpawnObject(_tunaPrefab,
                                          _tunaTransform.transform,
                                          SpawnObjectType.INGREDIENT);
    }
    public void BuyRice()
    {
        GameManager.Instance.DeductMoney(_ricePrice);
        Debug.LogError("Rice spanwing hasn't been added yet!");

        // insert code here to reset Rice at rice cooker
    }

#endregion
}
