using UnityEngine.UI;
using UnityEngine;
using TMPro;
using System.Collections;
using Unity.VisualScripting;

public class MainMenuHandler : StaticInstance<MainMenuHandler>
{
    #region Members

    public System.Action OnResetMGS { get; set; }

    [SerializeField] private GameObject playIcon;
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private GameObject settingsPanel, _eodPanel, _liveWallpaper;
    [SerializeField] private GameObject btn_continue, _pressToContinuePanel;
    [SerializeField] private TextMeshProUGUI _currentPhaseTxt, _customerCountTxt;

    [SerializeField] private Slider masterSlider;  // We need to fix this slider for the volumeeeeee
    [Space(10f), SerializeField] private bool _isTutorial;

    #endregion

    #region Unity

    protected override void Awake()
    {
        base.Awake();
        StartCoroutine(CO_DelayedEventBinding());
    }
    protected override void OnApplicationQuit() => base.OnApplicationQuit();
    private void OnDisable()
    {
        OnBoardingHandler.Instance.OnTutorialEnd -= DisableTutorial;
    }
    #endregion
    #region Toggle

    public void TogglePlayIcon(bool toggle) => playIcon.SetActive(toggle);
    public void TogglePausePanel(bool isTrue) => pausePanel.SetActive(isTrue);
    public void ToggleLiveWallpaper(bool isLive) => _liveWallpaper.SetActive(isLive);

    public void ToggleEODPanel(bool isActive)
    {
        _eodPanel.SetActive(isActive);
        btn_continue.SetActive(isActive);
        _pressToContinuePanel.SetActive(isActive);

        if (GameManager.Instance.IsGameOver)
        {
            btn_continue.SetActive(false);
            _pressToContinuePanel.SetActive(false);
        }
    }
    public void ToggleSettingsPanel(bool isTrue)
    {
        settingsPanel.SetActive(isTrue);
        pausePanel.SetActive(false);
    }

    #endregion
    #region UI Panels

    public void UpdateNameOfPhaseTxt(string phase) => _currentPhaseTxt.text = phase;
    public void UpdateCustomerCountUI() => _currentPhaseTxt.text = $"{GameManager.Instance.CustomersServed}";

    #endregion
    #region Buttons

    public void BTN_ResetMGS()
    {
        if (GameManager.Instance.IsGameOver) return;

        GameManager.Instance.ResetMGS();
        SpawnManager.Instance.ClearSeats();
        OnResetMGS?.Invoke();
    }

    #endregion
    
    private void DisableTutorial() => _isTutorial = false;
    private IEnumerator CO_DelayedEventBinding()
    {
        yield return new WaitForSeconds(1f);
        OnBoardingHandler.Instance.OnTutorialEnd += DisableTutorial;

    }
}
