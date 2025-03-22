using UnityEngine.UI;
using UnityEngine;

public class MainMenuUIScript : Singleton<MainMenuUIScript>
{
    [SerializeField] private GameObject playIcon;
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private GameObject settingsPanel;

    [SerializeField] private Slider masterSlider;  //We need to fix this slider for the volumeeeeee

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
}
