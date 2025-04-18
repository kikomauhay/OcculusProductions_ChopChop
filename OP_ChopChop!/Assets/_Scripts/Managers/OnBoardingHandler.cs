using System.Collections;
using UnityEngine;
using System;
using Unity.XR.CoreUtils;

public class OnBoardingHandler : Singleton<OnBoardingHandler> 
{
#region Members

    [SerializeField] private GameObject _slicingPanel, _moldingPanel, _salmonPrefab;
    [SerializeField] private Transform _ingTransform;
    [SerializeField] private Transform[] _chairs;
    [SerializeField] private GameObject[] _tutorialCustomers;

    [Header("On-Boarding Components")]
    [SerializeField] private Material _oulineMaterial;
    [SerializeField] private Renderer _rend;
    [SerializeField] private GameObject _faucetKnob;

#endregion

#region Unity

    protected override void Awake() => base.Awake();
    protected override void OnApplicationQuit() => base.OnApplicationQuit();
    
#endregion

    public void Disable() => gameObject.SetActive(false);
    
    
#region Helpers

    void SpawnFirstCustomer()
    {
        Instantiate(_tutorialCustomers[0], 
                    _chairs[0].position, 
                    _chairs[0].rotation);
        
        Instantiate(_salmonPrefab, 
                    _ingTransform.position, 
                    _ingTransform.rotation);
    }
    void SpawnNextCustomers()
    {
        Instantiate(_tutorialCustomers[1], 
                    _chairs[1].position, _chairs[1].rotation, 
                    transform);

        GameObject customer = Instantiate(_tutorialCustomers[1],
                              _chairs[2].position, _chairs[2].rotation,
                              transform);

        customer.GetComponent<CustomerOrder>().IsLastCustomer = true;
    }
    IEnumerator EnableSlicingPanel()
    {
        while (true)
        {
            _slicingPanel.SetActive(true);
            yield return new WaitForSeconds(4f);
            _slicingPanel.SetActive(false);
        }
    }
    IEnumerator EnableMoldingPanel()
    {
        while (true)
        {
            _moldingPanel.SetActive(true);
            yield return new WaitForSeconds(4f);
            _moldingPanel.SetActive(false);
        }
    }

#endregion

#region New OnBoarding

    public IEnumerator StartingTutorial() // hand washing
    {
        // triggered by pressing A in the controller

        SoundManager.Instance.PlaySound("onb 01", SoundGroup.TUTORIAL);
        yield return new WaitForSeconds(22f);

        // highlight faucet knob
        _rend = _faucetKnob.GetComponent<Renderer>(); 
        // _rend.AddMaterial();

        
        // disable highlight when player is interacting
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
        yield return new WaitForSeconds(8f);

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

        SpawnFirstCustomer();
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

        SpawnNextCustomers();
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