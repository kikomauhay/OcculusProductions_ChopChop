using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerHUD : Singleton<PlayerHUD>
{
    [SerializeField] private TextMeshProUGUI txtTopHUD;

    protected override void Awake() // set starting money
    {
        base.Awake();
        txtTopHUD.text = " ";
    }

    protected override void OnApplicationQuit()
    {
        base.OnApplicationQuit();
    }

    public void txtTopHUDUpdate(string text)
    {
        txtTopHUD.text = "Current Task: " + text;
    }
}
