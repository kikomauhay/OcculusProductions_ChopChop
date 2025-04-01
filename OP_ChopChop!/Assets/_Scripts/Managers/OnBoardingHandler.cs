using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;

/// <summary>
/// 
/// Acts as the tutorial for the player
/// Makes sure that the player doens't break the sequence of the onboarding  
/// 
/// </summary>

public class OnBoardingHandler : Singleton<OnBoardingHandler> 
{
    // int _tutorialIndex = 0;

    [SerializeField] GameObject _slicingPanel, _moldingPanel, _salmonPrefab;

    [SerializeField] Transform _ingTransform;

    [SerializeField] Transform[] _chairs;
    [SerializeField] GameObject[] _tutorialCustomers;

    protected override void Awake() => base.Awake();
    protected override void OnApplicationQuit() => base.OnApplicationQuit();

    // void Update() => test();    
    void test()
    {
        if (Input.GetKeyDown(KeyCode.Return) && !SceneHandler.Instance.IsFading)
        {
            StartCoroutine(SceneHandler.Instance.LoadScene("MainGameScene"));
            GameManager.Instance.ChangeShift(GameShift.PRE_SERVICE);
        }
    }

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
        SoundManager.Instance.PlaySound("onb 01", SoundGroup.TUTORIAL);
        yield return new WaitForSeconds(22f);

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
    }
    public IEnumerator ServingTutorial()
    {
        SoundManager.Instance.PlaySound("onb 05", SoundGroup.TUTORIAL);
        yield return new WaitForSeconds(14f);

        SoundManager.Instance.PlaySound("onb 06", SoundGroup.TUTORIAL);
        yield return new WaitForSeconds(22f);
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
