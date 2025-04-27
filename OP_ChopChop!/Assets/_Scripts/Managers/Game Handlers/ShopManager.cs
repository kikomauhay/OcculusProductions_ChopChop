using System.Collections;
using UnityEngine.UI;
using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class ShopManager : StaticInstance<ShopManager> 
{
#region Members

    [SerializeField] GameObject _salmonPrefab, _tunaPrefab;
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
        _txtPlayerMoney.text = GameManager.Instance.CurrentPlayerMoney.ToString();
        _tutorialPlayed = false;
        _orderBoxes = new List<GameObject>();

        // initialize ingredient prices in UI
        _txtSalmonPrice.text = _salmonPrice.ToString();
        _txtTunaPrice.text = _tunaPrice.ToString();
        _txtRicePrice.text = _ricePrice.ToString();
        
        Debug.Log($"Is Tutorial: {_isTutorial}");
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            BuySalmon();

        if (Input.GetKeyDown(KeyCode.Backspace))
            BuyTuna();
    }

#endregion

#region Buying

    public void BuySalmon()
    {
        if (_isTutorial)
        {
            // UX when the player has pressed the button
            SoundManager.Instance.PlaySound("select", SoundGroup.GAME);
            StartCoroutine(ButtonCooldownTimer(0));

            // waiting time for the salmon to spawn
            StartCoroutine(DeliveryWaitTime());
            GameObject box = Instantiate(_salmonPrefab, transform.position, transform.rotation);
            box.GetComponent<OrderBox>().IsTutorial = true; 
            _orderBoxes.Add(box);            

            // removes the highlight and triggers the next tutorial  
            if (!_tutorialPlayed)
            {          
                GetComponent<OutlineMaterial>().DisableHighlight();
                StartCoroutine(OnBoardingHandler.Instance.CallOnboarding(2));
                _tutorialPlayed = true;
            }
            return;
        }       

        if (GameManager.Instance.CurrentPlayerMoney > 0)
        {
            // UX when the player has pressed the button
            SoundManager.Instance.PlaySound("select", SoundGroup.GAME);
            StartCoroutine(ButtonCooldownTimer(0));
            
            // deduction of money
            GameManager.Instance.DeductMoney(_salmonPrice);
            _txtPlayerMoney.text = GameManager.Instance.CurrentPlayerMoney.ToString();
            
            // waiting time before the salmon spawns
            StartCoroutine(DeliveryWaitTime());
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
            SoundManager.Instance.PlaySound("select", SoundGroup.GAME);
            StartCoroutine(ButtonCooldownTimer(1));

            // waiting time for the tuna to spawn
            StartCoroutine(DeliveryWaitTime());
            GameObject box = Instantiate(_tunaPrefab, transform.position, transform.rotation);
            box.GetComponent<OrderBox>().IsTutorial = true; 
            _orderBoxes.Add(box);
            return;
        }

        if (GameManager.Instance.CurrentPlayerMoney > 0)
        {
            // UX when the player has pressed the button
            SoundManager.Instance.PlaySound("select", SoundGroup.GAME);
            StartCoroutine(ButtonCooldownTimer(1));
           
            // deduction of money
            GameManager.Instance.DeductMoney(_tunaPrice);
            _txtPlayerMoney.text = GameManager.Instance.CurrentPlayerMoney.ToString();            
            
            // waiting time before the tuna spawns
            StartCoroutine(DeliveryWaitTime());
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
            SoundManager.Instance.PlaySound("wrong", SoundGroup.GAME);
            return;
        }

        if (GameManager.Instance.CurrentPlayerMoney > 0)
        {
            // UX when the player has pressed the button
            SoundManager.Instance.PlaySound("select", SoundGroup.GAME);
            StartCoroutine(ButtonCooldownTimer(2));
            
            // deduction of money
            GameManager.Instance.DeductMoney(_ricePrice);
            _txtPlayerMoney.text = GameManager.Instance.CurrentPlayerMoney.ToString();

            // waiting time before the rice spawns
            StartCoroutine(DeliveryWaitTime());

            // insert code below to reset the Rice at the Rice Cooker
        }
    }
    
    public void ClearList()
    {
        if (_orderBoxes.Count < 1) return;

        foreach (GameObject box in _orderBoxes)
            Destroy(box);

        _orderBoxes.Clear();        
    }

#endregion

#region Enumerators

    private IEnumerator ButtonCooldownTimer(int index)
    {
        _interactableButtons[index].interactable = false;
        yield return new WaitForSeconds(2f);
        _interactableButtons[index].interactable = true;
    }

    private IEnumerator DeliveryWaitTime()
    {
        yield return new WaitForSeconds(5f);
    } 

#endregion
}
