using System.Collections;
using System.ComponentModel;
using UnityEditorInternal;
using UnityEngine;

public class OnBoardingHandler : Singleton<OnBoardingHandler> 
{
#region Members

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

    [Space(10f), SerializeField] private Transform _spawnpoint;
    [Space(10f), SerializeField] private Collider _dirtyCollider;

#endregion

#region Unity

    protected override void Awake() => base.Awake();
    protected override void OnApplicationQuit() => base.OnApplicationQuit();

    private void OnTriggerEnter(Collider other)
    {
        Sponge sponge = other.gameObject.GetComponent<Sponge>();

        if (sponge == null) return;


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

    public void Disable() => gameObject.SetActive(false);


#region New Onboarding

    public IEnumerator CallOnboarding(int mode)
    {
        SoundManager.Instance.StopAllAudio();

        switch (mode)
        {
            case 0: // STARTING TUTORIAL
                Instantiate(_atriumPrefab, _spawnpoint.position, _spawnpoint.rotation);
                SoundManager.Instance.PlaySound("onb 01", SoundGroup.TUTORIAL);
                yield return new WaitForSeconds(20f);
                _faucetKnob.GetComponent<OutlineMaterial>().EnableHighlight(); 
                break;
            
            case 1: // INGREDIENT ORDERING TUTORIAL
                SoundManager.Instance.PlaySound("onb 02", SoundGroup.TUTORIAL);
                yield return new WaitForSeconds(10f);
                _orderScreen.GetComponent<OutlineMaterial>().EnableHighlight(); 
                break;

            case 2: // FREEZER TUTORIAL
                SoundManager.Instance.PlaySound("onb 03", SoundGroup.TUTORIAL);
                yield return new WaitForSeconds(3f);
                _freezer.GetComponent<OutlineMaterial>().EnableHighlight();
                break;

            case 3: // CHOPPING TUTORIAL
                SoundManager.Instance.PlaySound("onb 04", SoundGroup.TUTORIAL);
                yield return new WaitForSeconds(5f);
                _knife.GetComponent<OutlineMaterial>().EnableHighlight();
                break;

            case 4: // MOLDING TUTORIAL
                SoundManager.Instance.PlaySound("onb 05", SoundGroup.TUTORIAL);
                yield return new WaitForSeconds(5f);
                _riceCooker.GetComponent<OutlineMaterial>().EnableHighlight();
                break;

            case 5: // FOOD COMBINATION TUTORIAL
                SoundManager.Instance.PlaySound("onb 06", SoundGroup.TUTORIAL);
                yield return new WaitForSeconds(10f);
                _plate.GetComponent<OutlineMaterial>().EnableHighlight();
                break;

            case 6: // SECOND CUSTOMER TUTORIAL
                SoundManager.Instance.PlaySound("onb 07", SoundGroup.TUTORIAL);
                yield return new WaitForSeconds(12f);
                Instantiate(_tunaCustomerPrefab, _spawnpoint.position, _spawnpoint.rotation);
                break;

            case 7: // CLEANING TUTORIAL
                SoundManager.Instance.PlaySound("onb 08", SoundGroup.TUTORIAL);
                yield return new WaitForSeconds(10f);
                _sponge.GetComponent<OutlineMaterial>().EnableHighlight();
                _dirtyCollider.enabled = true;
                Instantiate(_stinkyVFX, _dirtyCollider.transform.position, _dirtyCollider.transform.rotation);
                break;

            case 8: // POST-SERVICE TUTORIAL
                SoundManager.Instance.PlaySound("onb 09", SoundGroup.TUTORIAL);
                yield return new WaitForSeconds(8f);
                _menuScreen.GetComponent<OutlineMaterial>().EnableHighlight();
                break;

            default: break;
        }   
    }
    public IEnumerator IngredientOrderingTutorial() // ordering of salmon slab
    {        
        // triggered by closing the faucet knob (needs to only happen once)

        SoundManager.Instance.StopAllAudio();
        SoundManager.Instance.PlaySound("onb 02", SoundGroup.TUTORIAL);
        yield return new WaitForSeconds(11f);

        // highlight order screen
        // disable highlight when player is interacting
    }
    public IEnumerator FreezerTutrial()
    {
        // triggered by the spawning a salmon slab (needs to only happen once)

        SoundManager.Instance.StopAllAudio();
        SoundManager.Instance.PlaySound("onb 03", SoundGroup.TUTORIAL);
        yield return new WaitForSeconds(20f);

        // highlight ONLY ONE freezer
        // disable highlight when player is interacting
    }
    public IEnumerator ChoppingTutorial()
    {
        // triggered when the slab exits the fridge (needs to only happen once)

        SoundManager.Instance.StopAllAudio();
        SoundManager.Instance.PlaySound("onb 04", SoundGroup.TUTORIAL);
        yield return new WaitForSeconds(14f);

        // highlight knife 
        // disable highlight when the knife is picked up
    }
    public IEnumerator MoldingTutorial()
    {
        // triggered after the salmon is thinly sliced

        SoundManager.Instance.StopAllAudio();
        SoundManager.Instance.PlaySound("onb 05", SoundGroup.TUTORIAL);
        yield return new WaitForSeconds(10f);

        // highlight rice cooker
        // disable highlight when player picks up rice
    }
    public IEnumerator FoodCombinationTutorial()
    {
        // triggered after the salmon is thinly sliced

        SoundManager.Instance.StopAllAudio();
        SoundManager.Instance.PlaySound("onb 06", SoundGroup.TUTORIAL);
        yield return new WaitForSeconds(8f);

        // highlight a plate
        // disable highlight when the plate is picked up
    }
    public IEnumerator SecondCustomerTutorial()
    {
        // triggers after you serve to Atrium

        SoundManager.Instance.StopAllAudio();
        SoundManager.Instance.PlaySound("onb 07", SoundGroup.TUTORIAL);
        yield return new WaitForSeconds(8f);

        // highlight order screen
        // disable highlight when player is interacting
    }
    public IEnumerator CleaningTutorial()
    {
        // triggers after the second customer is served

        SoundManager.Instance.StopAllAudio();
        SoundManager.Instance.PlaySound("onb 08", SoundGroup.TUTORIAL);
        yield return new WaitForSeconds(8f);

        // highlight sponge
        // disable highlight when the sponge is picked up
    }
    public IEnumerator PostServiceTutorial()
    {
        // triggers after you're done cleaning

        SoundManager.Instance.StopAllAudio();
        SoundManager.Instance.PlaySound("onb 09", SoundGroup.TUTORIAL);
        yield return new WaitForSeconds(8f);

        // highlight main menu screen
        // disable highlight when player is interacting
    }
    
#endregion

#region Old Onboarding

    public IEnumerator TestIngredentTutorial()
    {
        SoundManager.Instance.PlaySound("onb 03", SoundGroup.TUTORIAL);
        yield return new WaitForSeconds(30f);
    }
    public IEnumerator FoodPreparationTutorial()
    {
        SoundManager.Instance.PlaySound("onb 04", SoundGroup.TUTORIAL);
        yield return new WaitForSeconds(2f);

        /// SpawnAtrium();
        yield return new WaitForSeconds(15f);

        StartCoroutine(EnableSlicingPanel());
        yield return new WaitForSeconds(3f);

        StartCoroutine(EnableMoldingPanel());
        StopAllCoroutines();

        _moldingPanel.SetActive(false);
        _slicingPanel.SetActive(false);
    }
    public IEnumerator ServingTutorial()
    {
        SoundManager.Instance.PlaySound("onb 05", SoundGroup.TUTORIAL);
        yield return new WaitForSeconds(14f);

        SoundManager.Instance.PlaySound("onb 06", SoundGroup.TUTORIAL);
        yield return new WaitForSeconds(4f);

        StartCoroutine(Onb_CleanMgr.Instance.SpawnStinkyVFX());
        yield return new WaitForSeconds(18f);
    }

    public IEnumerator NextCustomerTutorial()
    {
        SoundManager.Instance.PlaySound("onb 07", SoundGroup.TUTORIAL);
        yield return new WaitForSeconds(10f);

        // SpawnNextCustomers();
    }
    public IEnumerator EndOfDayTutorial()
    {
        SoundManager.Instance.PlaySound("onb 08", SoundGroup.TUTORIAL);
        yield return new WaitForSeconds(22f);

        SoundManager.Instance.PlaySound("onb 08", SoundGroup.TUTORIAL);
        yield return new WaitForSeconds(7f);
    }


#endregion

    IEnumerator ChangeToMainGame()
    {
        float time = 5f;

        Debug.Log($"waiting {time}s to change to main game scene");

        yield return new WaitForSeconds(time);
        StartCoroutine(SceneHandler.Instance.LoadScene("MainGameScene"));
        GameManager.Instance.ChangeShift(GameShift.PRE_SERVICE);
    } 
}