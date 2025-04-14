using System;
using System.Collections;
using UnityEngine;

/// <summary>
/// 
/// Acts as the tutorial for the player
/// Makes sure that the player doens't break the sequence of the onboarding  
/// 
/// </summary>

public class OnBoardingHandler : Singleton<OnBoardingHandler> 
{
#region Members

    [SerializeField] private GameObject _slicingPanel, _moldingPanel, _salmonPrefab;

    [SerializeField] private Transform _ingTransform;

    [SerializeField] private Transform[] _chairs;
    [SerializeField] private GameObject[] _tutorialCustomers;

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
                    _chairs[0].position, _chairs[0].rotation,
                    transform);
        
        Instantiate(_salmonPrefab, 
                    _ingTransform.position, _ingTransform.rotation,
                    transform);
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

#region OnBoarding

    public IEnumerator InventoryTutorial()
    {
        SoundManager.Instance.PlaySound("wrong", SoundGroup.GAME);
        // SoundManager.Instance.PlaySound("onb 01", SoundGroup.TUTORIAL);
        // yield return new WaitForSeconds(22f);

        SoundManager.Instance.PlaySound("onb 02", SoundGroup.TUTORIAL);
        yield return new WaitForSeconds(21f);
    }
    public IEnumerator IngredentTutorial()
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
