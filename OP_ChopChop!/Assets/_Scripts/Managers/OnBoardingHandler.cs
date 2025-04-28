using System.Collections.Generic;
using UnityEngine.InputSystem;
using System.Collections;
using UnityEngine;

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

    [Header("Panels")]
    [SerializeField] private GameObject _slicingPanel;
    [SerializeField] private GameObject _moldingPanel;

    [Space(10f), SerializeField] private Transform _customerSpawnpoint;

    [Space(10f)]
    [SerializeField] private Collider _dirtyCollider;
    [SerializeField] private Collider _servingCollision;

    [Header("Input Button Ref")]
    [SerializeField] public InputActionReference Continue;

    [Header("Debugging")]
    [SerializeField] private List<GameObject> _plates;
    [SerializeField] public bool DoneWashing = false;

#endregion

#region Unity

    protected override void Awake() => base.Awake();
    protected override void OnApplicationQuit() => base.OnApplicationQuit();
    private void Update()
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

        if (DoneWashing)
        {
            Continue.action.Enable();
            Continue.action.performed += ContinueCallOnboarding1;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        Sponge sponge = other.gameObject.GetComponent<Sponge>();

        if (sponge != null) 
            StartCoroutine(DoCleaning());
    }

#endregion

    public void ContinueCallOnboarding1(InputAction.CallbackContext context) 
    {
        StartCoroutine(CallOnboarding(1));
        Continue.action.Disable();
        Continue.action.performed -= ContinueCallOnboarding1;
    }
    

#region Onboarding

    public IEnumerator Onboarding01() // STARTING TUTORIAL
    {
        SpawnManager.Instance.SpawnTutorialCustomer(true);
        SoundManager.Instance.PlaySound("onb 01", SoundGroup.TUTORIAL);
        yield return new WaitForSeconds(20f);
    
        _faucetKnob.GetComponent<OutlineMaterial>().EnableHighlight();
        Continue.action.Enable();
    }
    public IEnumerator Onboarding02() // INGREDIENT ORDERING TUTORIAL
    {
        SoundManager.Instance.PlaySound("onb 02", SoundGroup.TUTORIAL);
        yield return new WaitForSeconds(10f);
        _orderScreen.GetComponent<OutlineMaterial>().EnableHighlight(); 
    }
    public IEnumerator Onboarding03() // FREEZER TUTORIAL
    {
        SoundManager.Instance.PlaySound("onb 03", SoundGroup.TUTORIAL);
        yield return new WaitForSeconds(3f);

        _freezer.GetComponentInChildren<OutlineMaterial>().EnableHighlight();
    }   
    public IEnumerator Onboarding04() // CHOPPING TUTORIAL       //WE ARE HEREEEE IN TERMS OF TESTING
    {
        _freezer.GetComponentInChildren<OutlineMaterial>().DisableHighlight();
        SoundManager.Instance.PlaySound("onb 04", SoundGroup.TUTORIAL);
        yield return new WaitForSeconds(5f);
        _knife.GetComponentInChildren<OutlineMaterial>().EnableHighlight();
        StartCoroutine(EnableSlicingPanel());
    }
    public IEnumerator Onboarding05() // MOLDING TUTORIAL             
    {
        _knife.GetComponentInChildren<OutlineMaterial>().DisableHighlight();
        SoundManager.Instance.PlaySound("onb 05", SoundGroup.TUTORIAL);
        yield return new WaitForSeconds(5f);
        _riceCooker.GetComponentInChildren<OutlineMaterial>().EnableHighlight();
        StartCoroutine(EnableMoldingPanel());
    }
    public IEnumerator Onboarding06() // FOOD COMBINATION TUTORIAL
    {
        SoundManager.Instance.PlaySound("onb 06", SoundGroup.TUTORIAL);
        yield return new WaitForSeconds(10f);
        _plate.GetComponent<OutlineMaterial>().EnableHighlight();
    }
    public IEnumerator Onboarding07() // SECOND CUSTOMER TUTORIAL
    {
        SoundManager.Instance.PlaySound("onb 07", SoundGroup.TUTORIAL);
        yield return new WaitForSeconds(12f);
        SpawnManager.Instance.SpawnTutorialCustomer(false);
    }
    public IEnumerator Onboarding08() // CLEANING TUTORIAL
    {
        SoundManager.Instance.PlaySound("onb 08", SoundGroup.TUTORIAL);
        yield return new WaitForSeconds(10f);
        TriggerStinky();
    }
    public IEnumerator Onboarding09() // POST-SERVICE TUTORIAL
    {
        SoundManager.Instance.PlaySound("onb 09", SoundGroup.TUTORIAL);
        yield return new WaitForSeconds(8f);
        _menuScreen.GetComponent<OutlineMaterial>().EnableHighlight();
    }

#endregion

    //Code below not working as intended
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
                SpawnManager.Instance.SpawnTutorialCustomer(false);
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
    //Code above not working as intended

#region Helpers

    public void Disable()
    { 
        gameObject.SetActive(false);
        
        if (_plates.Count > 0)
        {
            foreach (GameObject p in _plates)
                Destroy(p);

            _plates.Clear();
        }

        OnTutorialEnd?.Invoke();
    }

    public void TriggerStinky()
    {
        // makes the sponge more visible
        _sponge.GetComponent<OutlineMaterial>().EnableHighlight();
        
        // enbles the cleaning logic
        _dirtyCollider.enabled = true;
        StartCoroutine(SpawnStinky());        
    }

#endregion    

#region Enumerators

    private IEnumerator SpawnStinky()
    {
        while (_dirtyCollider.enabled)
        {
            SpawnManager.Instance.SpawnVFX(VFXType.STINKY, _dirtyCollider.transform, 3f);
            yield return new WaitForSeconds(2f);
        }
    }
    private IEnumerator DoCleaning()
    {
        yield return new WaitForSeconds(2f);

        _dirtyCollider.enabled = false;
        StartCoroutine(Onboarding08());
    }
    private IEnumerator EnableSlicingPanel()
    {
         _slicingPanel.SetActive(true);
         yield return new WaitForSeconds(10f);
         _slicingPanel.SetActive(false);
    }
    private IEnumerator EnableMoldingPanel()
    {
        
         _moldingPanel.SetActive(true);
         yield return new WaitForSeconds(10f);
         _moldingPanel.SetActive(false);
    }

#endregion
}