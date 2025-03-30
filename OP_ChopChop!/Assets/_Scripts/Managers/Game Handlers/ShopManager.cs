using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class ShopManager : StaticInstance<ShopManager> 
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

    [SerializeField] List<GameObject> _salmonSlabs, _tunaSlabs;

    [Header("Button")]
    [SerializeField] private Button[] interactableButtons;

    public const int MAX_ORDER_COUNT = 3;

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

        _salmonSlabs = new List<GameObject>();
        _tunaSlabs = new List<GameObject>();
    }

#endregion

#region Debugging code for button

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            BuyTuna();
        }
    }

    
#endregion
    
#region Buying

    public void BuySalmon()
    {
        if (_salmonSlabs.Count >= MAX_ORDER_COUNT) 
        {
            SoundManager.Instance.PlaySound("wrong", SoundGroup.GAME);
            return;
        }

        GameManager.Instance.DeductMoney(_salmonPrice);
        SpawnManager.Instance.SpawnVFX(VFXType.SMOKE, _salmonTransform, 2F);
        GameObject salmon = SpawnManager.Instance.SpawnObject(_salmonPrefab,
                                                              _salmonTransform.transform,
                                                              SpawnObjectType.INGREDIENT);

        StartCoroutine(ButtonCooldownTimer(0));

        _txtPlayerMoney.text = GameManager.Instance.CurrentPlayerMoney.ToString();
        _salmonSlabs.Add(salmon);

        SoundManager.Instance.PlaySound("select", SoundGroup.GAME);
    }
    public void BuyTuna()
    {
        if (_tunaSlabs.Count >= MAX_ORDER_COUNT)
        {
            SoundManager.Instance.PlaySound("wrong", SoundGroup.GAME);
            return;
        }

        GameManager.Instance.DeductMoney(_tunaPrice);
        SpawnManager.Instance.SpawnVFX(VFXType.SMOKE, _tunaTransform, 2F);
        GameObject tuna = SpawnManager.Instance.SpawnObject(_tunaPrefab,
                                                            _tunaTransform.transform,
                                                            SpawnObjectType.INGREDIENT);
        
        StartCoroutine(ButtonCooldownTimer(1));
       
        _txtPlayerMoney.text = GameManager.Instance.CurrentPlayerMoney.ToString();
        _tunaSlabs.Add(tuna);

        SoundManager.Instance.PlaySound("select", SoundGroup.GAME);
    }
    public void BuyRice()
    {
        GameManager.Instance.DeductMoney(_ricePrice);
        Debug.LogError("Rice spanwing hasn't been added yet!");

        StartCoroutine(ButtonCooldownTimer(2));

        _txtPlayerMoney.text = GameManager.Instance.CurrentPlayerMoney.ToString();
        SoundManager.Instance.PlaySound("select", SoundGroup.GAME);
        
        // insert code here to reset Rice at rice cooker
    }

    public void RemoveFromTunaList(GameObject obj) => _tunaSlabs.Remove(obj);
    public void RemoveFromSalmonList(GameObject obj) => _salmonSlabs.Remove(obj);

    private IEnumerator ButtonCooldownTimer(int index)
    {
        interactableButtons[index].interactable = false;

        yield return new WaitForSeconds(2f);

        interactableButtons[index].interactable = true;
    }
#endregion
}
