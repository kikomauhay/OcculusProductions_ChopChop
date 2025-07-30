using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class MainMenuHandler : StaticInstance<MainMenuHandler>
{
    [SerializeField] private GameObject playIcon;
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private GameObject settingsPanel, _eodPanel, _liveWallpaper;
    [SerializeField] private GameObject btn_continue;
    [SerializeField] private TextMeshProUGUI _currentPhaseTxt;

    [SerializeField] private Slider masterSlider;  //We need to fix this slider for the volumeeeeee
    [Space(10f), SerializeField] private bool _isTutorial;

    protected override void Awake() => base.Awake();
    protected override void OnApplicationQuit() => base.OnApplicationQuit();

    public void TogglePlayIcon(bool toggle)
    {
        playIcon.SetActive(toggle);
    }
    public void TogglePausePanel(bool isTrue)
    {
        pausePanel.SetActive(isTrue);
    }

    public void ToggleEODPanel()
    {
        _eodPanel.SetActive(!_eodPanel.activeSelf);
        btn_continue.SetActive(!btn_continue.activeSelf);
    }
    public void ToggleLiveWallpaper() => _liveWallpaper.SetActive(!_liveWallpaper.activeSelf);

    public void ExitSettingsPanel()
    {
        settingsPanel.SetActive(false);
        pausePanel.SetActive(true);
    }
    public void ToggleSettingsPanel(bool isTrue)
    {
        settingsPanel.SetActive(isTrue);
        pausePanel.SetActive(false);
    }
    public void UpdateNameOfPhaseTxt(string phase)
    {
        _currentPhaseTxt.text = phase;
    }
    public void BTN_ResetMGS() // this method has a button reference 
    {
        GameManager.Instance.ResetMGS();
        SpawnManager.Instance.ClearSeats();
    }
}
