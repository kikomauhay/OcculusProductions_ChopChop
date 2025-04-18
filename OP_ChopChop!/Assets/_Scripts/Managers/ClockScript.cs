using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class ClockScript : StaticInstance<ClockScript>
{
    #region Members

    [SerializeField] TextMeshProUGUI _timerTxt;
    [SerializeField] TextMeshProUGUI _currentServiceTxt;
    private float timeRemaining;

    #endregion

    protected override void Awake() => base.Awake();
    protected override void OnApplicationQuit() => base.OnApplicationQuit();

    // Update is called once per frame
    void Update()
    {
        int minutes = Mathf.FloorToInt(timeRemaining / 60);
        int seconds = Mathf.FloorToInt(timeRemaining % 60);
        _timerTxt.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    public void UpdateNameOfPhaseTxt(string phase)
    {
        _currentServiceTxt.text = phase;
    }

    public void UpdateTimeRemaining(float variable)
    {
        timeRemaining = variable;
    }
}
