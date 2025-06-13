using System.Collections;
using UnityEngine.UI;
using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class ShopManager : StaticInstance<ShopManager> 
{
#region Members

    [SerializeField] GameObject _salmonPrefab, _tunaPrefab , _riceCooker;
    [SerializeField] Transform _spawnpoint;

    [Header("Ingredient Prices")]
    [SerializeField] private int _ricePrice;
    [SerializeField] private int _salmonPrice, _tunaPrice;

    [Header("Ingredient UI")]
    [SerializeField] private TextMeshProUGUI _txtPlayerMoney;
    [SerializeField] private TextMeshProUGUI _txtRicePrice, _txtSalmonPrice, _txtTunaPrice;

    [Header("Button")]
    [SerializeField] private Button[] _interactableButtons;

    [Header("Onboarding")]
    [SerializeField] private bool _isTutorial;  
    
    private bool _tutorialPlayed;
    private List<GameObject> _orderBoxes;

#endregion

#region Unity

    protected override void Awake() => base.Awake();
    protected override void OnApplicationQuit() => base.OnApplicationQuit();
    private void Start()
    {
        OnBoardingHandler.Instance.OnTutorialEnd += ClearList;

        _txtPlayerMoney.text = GameManager.Instance.CurrentPlayerMoney.ToString();
        _tutorialPlayed = false;
        _orderBoxes = new List<GameObject>();

        // initialize ingredient prices in UI
        UpatePlayerMoneyUI();
        _txtSalmonPrice.text = _salmonPrice.ToString();
        _txtTunaPrice.text = _tunaPrice.ToString();
        _txtRicePrice.text = _ricePrice.ToString();
        
        if (_isTutorial)
            Debug.Log($"{name} Is Tutorial: {_isTutorial}");
    }
    private void OnDestroy() => OnBoardingHandler.Instance.OnTutorialEnd -= ClearList;

#endregion

#region Buying

    public void BuySalmon()
    {
        if (_isTutorial)
        {
            // UX when the player has pressed the button
            SoundManager.Instance.PlaySound("select");
            StartCoroutine(ButtonCooldownTimer(0));

            SpawnManager.Instance.SpawnVFX(VFXType.SMOKE, _spawnpoint, 2f);
            _orderBoxes.Add(SpawnManager.Instance.SpawnObject(_salmonPrefab, 
                                                              _spawnpoint, 
                                                              SpawnObjectType.INGREDIENT));            
            
            // triggers the next tutorial  
            if (!_tutorialPlayed)
            {
                OnBoardingHandler.Instance.AddOnboardingIndex();
                OnBoardingHandler.Instance.PlayOnboarding();
                _tutorialPlayed = true;
            }
            return;
        }       

        if (GameManager.Instance.CurrentPlayerMoney > 0)
        {
            // UX when the player has pressed the button
            SoundManager.Instance.PlaySound("select");
            StartCoroutine(ButtonCooldownTimer(0));
            
            // deduction of money
            GameManager.Instance.DeductMoney(_salmonPrice);
            UpatePlayerMoneyUI();
            
            SpawnManager.Instance.SpawnVFX(VFXType.SMOKE, _spawnpoint, 2f);
            SpawnManager.Instance.SpawnObject(_salmonPrefab,
                                              _spawnpoint.transform,                    
                                              SpawnObjectType.INGREDIENT);
        }
    }
    public void BuyTuna()
    {
        if (_isTutorial)
        {
            // UX when the player has pressed the button
            SoundManager.Instance.PlaySound("select");
            StartCoroutine(ButtonCooldownTimer(1));

            SpawnManager.Instance.SpawnVFX(VFXType.SMOKE, _spawnpoint, 2f);
            _orderBoxes.Add(SpawnManager.Instance.SpawnObject(_tunaPrefab, 
                                                              _spawnpoint, 
                                                              SpawnObjectType.INGREDIENT));     
            return;
        }

        if (GameManager.Instance.CurrentPlayerMoney > 0)
        {
            // UX when the player has pressed the button
            SoundManager.Instance.PlaySound("select");
            StartCoroutine(ButtonCooldownTimer(1));
           
            // deduction of money
            GameManager.Instance.DeductMoney(_tunaPrice);
            UpatePlayerMoneyUI();           
            
            SpawnManager.Instance.SpawnVFX(VFXType.SMOKE, _spawnpoint, 2f);
            SpawnManager.Instance.SpawnObject(_tunaPrefab,
                                              _spawnpoint.transform,
                                              SpawnObjectType.INGREDIENT);
        }
    }
    public void BuyRice()
    {
        if (_isTutorial)
        {
            SoundManager.Instance.PlaySound("wrong");
            return;
        }

        if (GameManager.Instance.CurrentPlayerMoney > 0)
        {
            Debug.LogWarning($"Rice Ordered, Money remaining: {GameManager.Instance.CurrentPlayerMoney} ");
            // UX when the player has pressed the button
            SoundManager.Instance.PlaySound("select");
            StartCoroutine(ButtonCooldownTimer(2));

            // deduction of money
            GameManager.Instance.DeductMoney(_ricePrice);
            UpatePlayerMoneyUI();

            // waiting time before the rice spawns
            StartCoroutine(RiceDeliveryWaitTime());
        }
        else
        {
            SoundManager.Instance.PlaySound("wrong");
            return;
        }
    }

#endregion
    
#region Helpers

    public void ClearList()
    {
        if (_orderBoxes.Count < 1) return;

        foreach (GameObject box in _orderBoxes)
            Destroy(box);

        _orderBoxes.Clear();        
    }

    private void UpatePlayerMoneyUI() => 
        _txtPlayerMoney.text = GameManager.Instance.CurrentPlayerMoney.ToString();

#endregion

#region Enumerators

    private IEnumerator ButtonCooldownTimer(int index)
    {
        _interactableButtons[index].interactable = false;
        yield return new WaitForSeconds(2f);
        _interactableButtons[index].interactable = true;
    }

    private IEnumerator RiceDeliveryWaitTime()
    {
        Debug.LogWarning("Rice has been ordered, please wait");
        yield return new WaitForSeconds(5f);
        _riceCooker.GetComponentInChildren<RiceSpawn>().ResetRice();
    } 

#endregion
}
