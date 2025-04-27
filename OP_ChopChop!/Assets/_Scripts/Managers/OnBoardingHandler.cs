using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class OnBoardingHandler : Singleton<OnBoardingHandler> 
{
#region Members

    public bool TutorialPlaying { get; private set; } = false;

    public System.Action OnTutorialEnd;

    [Header("Objects"), Tooltip("This is sequentually organized.")]
    [SerializeField] private GameObject _faucetKnob; 
    [SerializeField] private GameObject _orderScreen, _freezer, _knife;
    [SerializeField] private GameObject _riceCooker, _plate, _sponge, _menuScreen;
    [SerializeField] private GameObject _stinkyVFX;
    
    [Header("Customers")]
    [SerializeField] private GameObject _atriumPrefab;
    [SerializeField] private GameObject _tunaCustomerPrefab;

    [Header("Panels")]
    [SerializeField] private GameObject _slicingPanel;
    [SerializeField] private GameObject _moldingPanel;

    [Space(10f), SerializeField] private Transform _customerSpawnpoint;

    [Space(10f)]
    [SerializeField] private Collider _dirtyCollider;
    [SerializeField] private Collider _servingCollision;

    [Header("InputButtonRef")]
    [SerializeField] public InputActionReference Continue;

    [Header("Special Stuff")]
    [SerializeField] public bool isPlayeDoneWashing = false;

    #endregion

    #region Unity

    protected override void Awake() => base.Awake();
    protected override void OnApplicationQuit() => base.OnApplicationQuit();
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha0)) StartCoroutine(CallOnboarding(0));
        if (Input.GetKeyDown(KeyCode.Alpha1)) StartCoroutine(CallOnboarding(1));
        if (Input.GetKeyDown(KeyCode.Alpha2)) StartCoroutine(CallOnboarding(2));
        if (Input.GetKeyDown(KeyCode.Alpha3)) StartCoroutine(CallOnboarding(3));
        if (Input.GetKeyDown(KeyCode.Alpha4)) StartCoroutine(CallOnboarding(4));
        if (Input.GetKeyDown(KeyCode.Alpha5)) StartCoroutine(CallOnboarding(5));
        if (Input.GetKeyDown(KeyCode.Alpha6)) StartCoroutine(CallOnboarding(6));
        if (Input.GetKeyDown(KeyCode.Alpha7)) StartCoroutine(CallOnboarding(7));
        if (Input.GetKeyDown(KeyCode.Alpha8)) StartCoroutine(CallOnboarding(8));

        if(isPlayeDoneWashing)
        {
            Continue.action.Enable();
            Continue.action.performed += ContinueCallOnboarding1;
        }

    }

    public void ContinueCallOnboarding1(InputAction.CallbackContext context)
    {
        StartCoroutine(CallOnboarding(1));
        Continue.action.Disable();
        Continue.action.performed -= ContinueCallOnboarding1;
    }

    private void OnTriggerEnter(Collider other)
    {
        Sponge sponge = other.gameObject.GetComponent<Sponge>();

        if (sponge != null) 
            StartCoroutine(DoCleaning());
    }

    private IEnumerator EnableSlicingPanel()
    {
        while (true)
        {
            _slicingPanel.SetActive(true);
            yield return new WaitForSeconds(4f);
            _slicingPanel.SetActive(false);
        }
    }
    private IEnumerator EnableMoldingPanel()
    {
        while (true)
        {
            _moldingPanel.SetActive(true);
            yield return new WaitForSeconds(4f);
            _moldingPanel.SetActive(false);
        }
    }
    
#endregion

    public IEnumerator CallOnboarding3()
    {
        SoundManager.Instance.PlaySound("onb 03", SoundGroup.TUTORIAL);
        yield return new WaitForSeconds(3f);
        Debug.Log("Onb 03 FREEZER OUTLINE ON");
        _freezer.GetComponentInChildren<OutlineMaterial>().EnableHighlight();
    }

    public IEnumerator CallOnboarding4()
    {
        SoundManager.Instance.PlaySound("onb 04", SoundGroup.TUTORIAL);
        yield return new WaitForSeconds(5f);
        _knife.GetComponentInChildren<OutlineMaterial>().EnableHighlight();
        EnableSlicingPanel();
    }

    public IEnumerator CallOnboarding5()
    {
        SoundManager.Instance.PlaySound("onb 05", SoundGroup.TUTORIAL);
        yield return new WaitForSeconds(5f);
        _riceCooker.GetComponentInChildren<OutlineMaterial>().EnableHighlight();
        EnableMoldingPanel();
    }

    public IEnumerator CallOnboarding6()
    {
        SoundManager.Instance.PlaySound("onb 06", SoundGroup.TUTORIAL);
        yield return new WaitForSeconds(10f);
        _plate.GetComponent<OutlineMaterial>().EnableHighlight();
    }

    public IEnumerator CallOnboarding7()
    {
        SoundManager.Instance.PlaySound("onb 07", SoundGroup.TUTORIAL);
        yield return new WaitForSeconds(12f);
        Instantiate(_tunaCustomerPrefab, _customerSpawnpoint.position, _customerSpawnpoint.rotation);
    }

    public IEnumerator CallOnboarding8()
    {
        SoundManager.Instance.PlaySound("onb 08", SoundGroup.TUTORIAL);
        yield return new WaitForSeconds(10f);
        TriggerStinky();
    }

    public IEnumerator CallOnboarding9()
    {
        SoundManager.Instance.PlaySound("onb 09", SoundGroup.TUTORIAL);
        yield return new WaitForSeconds(8f);
        _menuScreen.GetComponent<OutlineMaterial>().EnableHighlight();
    }

    public IEnumerator CallOnboarding(int mode)
    {
        if (SoundManager.Instance.SoundPlaying()) 
        {
            Debug.LogError("A sound is alraedy playing!"); 
            yield break;
        }
        if (TutorialPlaying)
        {
            Debug.LogError("Tutorial is already playing!");
            yield break;
        }

        SoundManager.Instance.StopAllAudio();
        TutorialPlaying = true;

        switch (mode)
        {
            case 0: // STARTING TUTORIAL
                SpawnManager.Instance.SpawnTutorialCustomer(true);
                SoundManager.Instance.PlaySound("onb 01", SoundGroup.TUTORIAL);
               
                yield return new WaitForSeconds(20f);
                _faucetKnob.GetComponent<OutlineMaterial>().EnableHighlight();
                Continue.action.Enable();
                break;

            case 1: // INGREDIENT ORDERING TUTORIAL
                Debug.Log("Onb 02 playing");
                SoundManager.Instance.PlaySound("onb 02", SoundGroup.TUTORIAL);
                yield return new WaitForSeconds(10f);
                _orderScreen.GetComponent<OutlineMaterial>().EnableHighlight(); 
                break;

            case 2: // FREEZER TUTORIAL
                Debug.Log("Onb 03 playing");
                SoundManager.Instance.PlaySound("onb 03", SoundGroup.TUTORIAL);
                yield return new WaitForSeconds(3f);
                Debug.Log("Onb 03 FREEZER OUTLINE ON");
                _freezer.GetComponentInChildren<OutlineMaterial>().EnableHighlight();
                break;

            case 3: // CHOPPING TUTORIAL
                SoundManager.Instance.PlaySound("onb 04", SoundGroup.TUTORIAL);
                yield return new WaitForSeconds(5f);
                _knife.GetComponentInChildren<OutlineMaterial>().EnableHighlight();
                EnableSlicingPanel();   
                break;

            case 4: // MOLDING TUTORIAL
                SoundManager.Instance.PlaySound("onb 05", SoundGroup.TUTORIAL);
                yield return new WaitForSeconds(5f);
                _riceCooker.GetComponentInChildren<OutlineMaterial>().EnableHighlight();
                EnableMoldingPanel();
                break;

            case 5: // FOOD COMBINATION TUTORIAL
                SoundManager.Instance.PlaySound("onb 06", SoundGroup.TUTORIAL);
                yield return new WaitForSeconds(10f);
                _plate.GetComponent<OutlineMaterial>().EnableHighlight();
                break;

            case 6: // SECOND CUSTOMER TUTORIAL
                SoundManager.Instance.PlaySound("onb 07", SoundGroup.TUTORIAL);
                yield return new WaitForSeconds(12f);
                Instantiate(_tunaCustomerPrefab, _customerSpawnpoint.position, _customerSpawnpoint.rotation);
                break;

            case 7: // CLEANING TUTORIAL
                SoundManager.Instance.PlaySound("onb 08", SoundGroup.TUTORIAL);
                yield return new WaitForSeconds(10f);
                
                TriggerStinky();
                break;

            case 8: // POST-SERVICE TUTORIAL
                SoundManager.Instance.PlaySound("onb 09", SoundGroup.TUTORIAL);
                yield return new WaitForSeconds(8f);
                _menuScreen.GetComponent<OutlineMaterial>().EnableHighlight();
                break;

            default: yield break;
        }

        TutorialPlaying = false;
    }

    #region Helpers

    public void Disable()
    { 
        gameObject.SetActive(false);
        OnTutorialEnd?.Invoke();
    }

    public void TriggerStinky()
    {
        // makes the sponge more visible
        _sponge.GetComponent<OutlineMaterial>().EnableHighlight();
        
        // enbles the cleaning logic
        _dirtyCollider.enabled = true;
        _stinkyVFX = Instantiate(_stinkyVFX, 
                                 _dirtyCollider.transform.position, 
                                 _dirtyCollider.transform.rotation);
    }

#endregion    

    private IEnumerator DoCleaning()
    {
        yield return new WaitForSeconds(2f);

        Destroy(_stinkyVFX);
        _dirtyCollider.enabled = true;
        CallOnboarding(8);
    }
}