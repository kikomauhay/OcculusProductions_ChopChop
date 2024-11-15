using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NigiriDish : MonoBehaviour
{
    [SerializeField] private GameObject sashimiCheck;
    [SerializeField] private GameObject riceCheck;
    [SerializeField] private Image timerRectBar;
    [SerializeField] private float timer;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void ActivateSashimiCheck() //Upon successful placement of ingredient on plate, check On
    {
        sashimiCheck.SetActive(true);
    }

    private void ActivateRiceCheck()
    {
        riceCheck.SetActive(true);
    }
}
