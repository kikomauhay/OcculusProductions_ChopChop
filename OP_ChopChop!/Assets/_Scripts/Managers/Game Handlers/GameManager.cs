using System.Collections.Generic;
using UnityEngine.InputSystem;
using System.Collections;
using UnityEngine;
using System;
using Unity.VisualScripting;

public class GameManager : Singleton<GameManager>
{
    #region Properties

    public Action OnStartService, OnEndService, OnGameOver;
    public InputActionReference Continue;
    public GameShift CurrentShift { get; private set; } = GameShift.Default;

    // DIFFICULTY
    public GameDifficulty Difficulty { get; private set; }
    public int MaxCustomerCount { get; private set; }
    public bool TutorialDone { get; set; }
    public bool IsGameOver { get; private set; }
    public bool IsPaused { get; private set; }
    public float CurrentPlayerMoney { get; private set; }

    #endregion
    #region SerializeField

    [SerializeField] private float _testTimer;
    [SerializeField] private float _startingPlayerMoney;
    [SerializeField] private RestaurantReceipt _eodReceipt;
    [SerializeField] private GameObject _logo;
    [SerializeField] private bool _logoRemoved = false;
    [SerializeField] private bool _isTutorial;

    [Header("Debugging")]
    [SerializeField] private bool _isDeveloperMode;

    #endregion
    #region Private

    // SCORING VALUES
    [SerializeField] private List<float> _customerSRScores;
    public int CustomersServed; // will be used for difficulty increase
    private float _finalScore;
    private float _finalKitchenScore, _finalCustomerScore;

    private const float MAX_MONEY = 9999f;
    private const float FIVE_MINUTES = 300f; // shift duration for Service
    private const float ONE_MINUTE = 60f; // shift duration for Pre-Service

    #endregion

    #region Unity

    protected override void OnApplicationQuit()
    {
        base.OnApplicationQuit();

        Continue.action.performed -= RemoveLogo;

        if (_isDeveloperMode)
            Debug.Log($"{this} developer mode: {_isDeveloperMode}");

        OnStartService -= ResetScores;
    }
    protected override void Awake() // set starting money
    {
        base.Awake();

        Difficulty = GameDifficulty.EASY;
        CurrentPlayerMoney = _startingPlayerMoney;
        CustomersServed = 0;
        MaxCustomerCount = 3;

        _finalCustomerScore = 0;
        _finalKitchenScore = 0;
        _finalScore = 0;

        IsPaused = false;
        IsGameOver = false;
        
        _customerSRScores = new List<float>(); 

        Continue.action.Enable();
        Continue.action.performed += RemoveLogo;
        OnStartService += ResetScores;
    }
    private void Start() => StartCoroutine(CO_DelayedEventBind());
    private void Update() => Test();
    private void Test()
    {
        if (CurrentShift == GameShift.Service)
            ClockScript.Instance.UpdateNameOfPhaseTxt($"{CurrentShift}");

        if (!_isDeveloperMode) return;

        if (Input.GetKeyDown(KeyCode.Return))
        {
            Keyboard_RemoveLogo();
            Debug.Log("Removed Chop Chop Logo");
        }
        if (Input.GetKeyDown(KeyCode.Delete))
        {
            ChangeShift(GameShift.PostService);
        }
    }

    #endregion
    #region Public

    public void RemoveLogo(InputAction.CallbackContext context)
    {
        SoundManager.Instance.PlaySound("select");
        OnBoardingHandler.Instance.PlayOnboarding();

        // unpauses the game, removes logo, and start onboarding
        ChangeShift(GameShift.Training);
        Continue.action.Disable();
        _logoRemoved = true;
        _logo.SetActive(false);

        if (_logoRemoved)
        {
            // Debug.Log("LOGO REMOVED");
            Continue.action.performed -= RemoveLogo;
        }
    }
    public void Keyboard_RemoveLogo()
    {
        SoundManager.Instance.PlaySound("select");
        OnBoardingHandler.Instance.PlayOnboarding();

        // unpauses the game, removes logo, and start onboarding
        ChangeShift(GameShift.Training);
        Continue.action.Disable();
        _logoRemoved = true;
        _logo.SetActive(false);

        if (_logoRemoved)
        {
            Debug.Log("LOGO REMOVED");
            Continue.action.performed -= RemoveLogo;
        }
    }
    public void TogglePause()
    {
        if (!SceneHandler.Instance.CanPause) return;

        IsPaused = !IsPaused;

        if (IsPaused)
        {
            Time.timeScale = 0f;
            MainMenuHandler.Instance?.TogglePlayIcon(false);
            MainMenuHandler.Instance?.TogglePausePanel(true);
        }
        else
        {
            Time.timeScale = 1f;
            MainMenuHandler.Instance?.TogglePausePanel(false);
            MainMenuHandler.Instance?.TogglePlayIcon(true);
        }
    }
    public void AddToCustomerScores(float n) => _customerSRScores.Add(n);
    public void IncrementCustomersServed()
    {
        CustomersServed++;
        MainMenuHandler.Instance.UpdateCustomerCountUI();
    }
    public void AddMoney(float amt)
    {
        if (amt < 0f) return;

        CurrentPlayerMoney += amt;

        CurrentPlayerMoney = Mathf.Clamp(CurrentPlayerMoney, 0f, MAX_MONEY);
        SoundManager.Instance.PlaySound("earn money");
    }
    public void DeductMoney(float amt)
    {
        // Debug.Log("Minus Player Money");
        if (amt < 0f) return;

        CurrentPlayerMoney -= amt;

        if (CurrentPlayerMoney < 0f)
            CurrentPlayerMoney = 0f;

        CurrentPlayerMoney = Mathf.Clamp(CurrentPlayerMoney, 0f, MAX_MONEY);
    }
    public void ChangeShift(GameShift chosenShift)
    {
        if (chosenShift == CurrentShift)
        {
            // Debug.LogError("You cannot go to the same shift again!");
            return;
        }

        CurrentShift = chosenShift;

        switch (chosenShift)
        {
            case GameShift.Training: 
                ResetScores();
                EnterTraining();
                break;
            
            case GameShift.PreService: DoPreService(); break;
            case GameShift.Service: DoService(); break;
            case GameShift.PostService: DoPostService(); break;

            default:
                Debug.LogError("Invalid state chosen!");
                break;
        }
        SoundManager.Instance.PlaySound("change shift");
        // Debug.LogWarning($"Shifted to {CurrentShift}"!);
    }
    public void EnableEOD()
    {
        if (_isTutorial)
            EnableEODReceipt();
    }
    public void ResetMGS()
    {
        ChangeShift(GameShift.PreService);
        ResetScores();
        _eodReceipt.ClearRatings();

        MainMenuHandler.Instance.ToggleEODPanel(false);
        MainMenuHandler.Instance.TogglePlayIcon(true);
        MainMenuHandler.Instance.ToggleLiveWallpaper(true);

        // Debug.LogWarning("Resetting MGS");
    }
    public void CheckRemainingCustomers()
    {
        if (CustomersServed == MaxCustomerCount)
        {
            StopAllCoroutines();
            ChangeShift(GameShift.PostService);
        }
    }

    #endregion
    #region Private

    private void DisableTutorial()
    {
        if (!_isTutorial)
        {
            Debug.LogError($"{gameObject.name} is already not a tutorial!");
            return;
        }

        _isTutorial = false;
        OnBoardingHandler.Instance.OnTutorialEnd -= DisableTutorial;

        if (!_logoRemoved)
        {
            _logoRemoved = true;
            _logo.SetActive(false);
            Continue.action.performed += RemoveLogo;
        }
    }
    private void ResetScores()
    {
        if (_customerSRScores.Count > 0)
            _customerSRScores.Clear();

        _finalCustomerScore = 0f;
        _finalKitchenScore = 0f;
        _finalScore = 0f;

        CustomersServed = 0;
        DisableGameOver();
    }
    private void EnterTraining()
    {
        SoundManager.Instance.StopMusic();

        // if (TutorialDone)
            SoundManager.Instance.PlayMusic("training bgm");
    }

    #endregion
    #region Game Shifts

    private void DoPreService() // change to 1 min when done testing
    {
        float serviceTimer = _isDeveloperMode ? _testTimer : ONE_MINUTE;

        // Debug.Log($"waiting {serviceTimer}s to change to service");
        StartCoroutine(CO_ShiftCountdown(serviceTimer, GameShift.Service));

        SoundManager.Instance.StopMusic();
        SoundManager.Instance.PlayMusic("pre-service bgm");

        ClockScript.Instance.UpdateTimeRemaining(serviceTimer);
        ClockScript.Instance.UpdateNameOfPhaseTxt($"{CurrentShift}");
    }
    private void DoService()
    {
        float timer = _isDeveloperMode ? _testTimer * 3f: FIVE_MINUTES; 

        SoundManager.Instance.StopMusic();
        SoundManager.Instance.PlayMusic("service bgm");

        ClockScript.Instance.UpdateTimeRemaining(timer);
        ClockScript.Instance.UpdateNameOfPhaseTxt($"{CurrentShift}");

        OnStartService?.Invoke(); // all ingredients start decaying
        _finalScore = 0;

        // Debug.Log($"waiting {timer}s to change to service");
        StartCoroutine(KitchenCleaningManager.Instance.CO_EnableDirtyColliders());
        StartCoroutine(CO_ShiftCountdown(timer, GameShift.PostService));
    }
    private void DoPostService()
    {
        SoundManager.Instance.StopMusic();
        SoundManager.Instance.PlayMusic("post-service bgm");

        EnableEODReceipt();
        OnEndService?.Invoke();
        ClockScript.Instance.UpdateNameOfPhaseTxt($"{CurrentShift}");
    }
    private void ChangeDifficuty(int score)
    {
        if (score < 3) // player scored B or higher
        {
            if (Difficulty != GameDifficulty.HARD)
                Difficulty++;

            MaxCustomerCount++;
            KitchenCleaningManager.Instance.MaxDirtyColliders++;
        }
        else if (score == 3) // player scored C 
        {
            if (Difficulty != GameDifficulty.EASY)
                Difficulty++;

            if (MaxCustomerCount > 3)
            {
                MaxCustomerCount--;
                KitchenCleaningManager.Instance.MaxDirtyColliders--;
            }
        }
        else // player scored below C
        {
            StartCoroutine(CO_GameOver2());
            return;
        }
    }

    #endregion
    #region EOD Rating

    private void EnableEODReceipt()
    {
        MainMenuHandler.Instance.ToggleEODPanel(true);
        _eodReceipt = MainMenuHandler.Instance.gameObject?.
                      GetComponentInChildren<RestaurantReceipt>();

        MainMenuHandler.Instance.TogglePlayIcon(false);
        MainMenuHandler.Instance.ToggleLiveWallpaper(false);

        DoFinalRatings();
        _eodReceipt.GiveTotalCustomerServed();
    }
    private void DoFinalRatings()
    {
        if (_isTutorial)
        {
            _eodReceipt.SetCustomerRating(0);
            _eodReceipt.SetKitchenRating(0);
            _eodReceipt.SetRestaurantRating(0);
            CustomersServed = 2;
            return;
        }

        if (IsGameOver)
        {
            _finalCustomerScore = 0f;
            _finalKitchenScore = 0f;
            _finalScore = 0f;

            // no need to convert to the index since it's game over
            _eodReceipt.SetCustomerRating(4);
            _eodReceipt.SetKitchenRating(4);
            _eodReceipt.SetRestaurantRating(4);
        }
        else
        {
            _finalCustomerScore = GetAverageOf(_customerSRScores);
            _finalKitchenScore = KitchenCleaningManager.Instance.KitchenScore;
            _finalScore = (_finalKitchenScore + _finalCustomerScore) / 2f;

            _eodReceipt.SetCustomerRating(_eodReceipt.ConvertToScoreIndex(_finalCustomerScore));
            _eodReceipt.SetKitchenRating(_eodReceipt.ConvertToScoreIndex(_finalKitchenScore));

            // changes difficulty once service ends
            int finalIndexScore = _eodReceipt.ConvertToScoreIndex(_finalScore);
            _eodReceipt.SetRestaurantRating(finalIndexScore);
            ChangeDifficuty(finalIndexScore);
        }        
    }
    private float GetAverageOf(List<float> list)
    {
        // prevents a div/0 case
        if (list.Count < 1) return -1f;

        float n = 0f;

        for (int i = 0; i < list.Count; i++)       
            n += list[i];
        
        return n / CustomersServed; 
    }


    #endregion
    #region Game Over
    public void DisableGameOver()
    {
        IsGameOver = false;
        Debug.LogWarning("Removed game over!");
    }
    public IEnumerator CO_GameOver1()
    {
        IsGameOver = true;
        StopAllCoroutines();

        SoundManager.Instance.StopMusic();
        SoundManager.Instance.PlaySound("game over 01");

        yield return new WaitForSeconds(14f);
        yield return StartCoroutine(CO_GameOver2());
    }
    public IEnumerator CO_GameOver2()
    {
        if (!IsGameOver)
        {
            IsGameOver = true;
            SoundManager.Instance.StopMusic();
        }

        OnGameOver?.Invoke();
        SoundManager.Instance.PlaySound("game over 02");
        yield return new WaitForSeconds(2f);

        ChangeShift(GameShift.PostService);

        // SceneHandler.Instance.LoadScene("TrainingScene");
        // ChangeShift(GameShift.Training);
    }

    #endregion
    #region Enumerators

    IEnumerator CO_ShiftCountdown(float timer, GameShift shift)
    {
        if (timer < 1) yield break;

        while (timer != 0)
        {
            yield return new WaitForSeconds(1f);
            timer--;
            ClockScript.Instance.UpdateTimeRemaining(timer);
        }

        SoundManager.Instance.PlaySound("change shift");
        ChangeShift(shift);
    }
    private IEnumerator CO_DelayedEventBind()
    {
        yield return new WaitForSeconds(1f);
        OnBoardingHandler.Instance.OnTutorialEnd += DisableTutorial;
    }
    

#endregion
}

#region Enuemrations

    public enum GameDifficulty
    { 
        EASY, 
        NORMAL,
        HARD
    }

    public enum GameShift // IN A CERTAIN ORDER (DON'T RE-ORDER)
    {
        Default,
        Training,
        PreService,
        Service,
        PostService
    }

#endregion