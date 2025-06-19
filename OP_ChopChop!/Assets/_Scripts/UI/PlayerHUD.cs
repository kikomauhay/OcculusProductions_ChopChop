using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerHUD : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI txtTopHUD;

    private void Awake() 
    {
        txtTopHUD.text = " ";
    }

    public void txtTopHUDUpdate(string text)
    {
        txtTopHUD.text = "Current Task: " + text;
    }
}
