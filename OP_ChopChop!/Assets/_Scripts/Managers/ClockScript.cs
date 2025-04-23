using UnityEngine;
using TMPro;

public class ClockScript : StaticInstance<ClockScript>
{
#region Members

    [SerializeField] private TextMeshProUGUI _timerTxt;
    [SerializeField] private TextMeshProUGUI _currentServiceTxt;
    
    private float _timeRemaining;

#endregion

    protected override void Awake() => base.Awake();
    protected override void OnApplicationQuit() => base.OnApplicationQuit();

    private void Update()
    {
        int minutes = Mathf.FloorToInt(_timeRemaining / 60);
        int seconds = Mathf.FloorToInt(_timeRemaining % 60);
        
        _timerTxt.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    public void UpdateNameOfPhaseTxt(string phase) => _currentServiceTxt.text = phase;
    public void UpdateTimeRemaining(float variable) => _timeRemaining = variable;
}
