using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NigiriDish : MonoBehaviour
{
    [SerializeField] private GameObject sashimiCheck;  //Upon checking of plating, see if Ingredients are place correctly
    [SerializeField] private GameObject riceCheck;
    [SerializeField] private GameObject orderLocation;

    public GameObject _OrderLocation
    {
        get { return orderLocation; }
        set { orderLocation = value; }
    }

    [SerializeField] private Image timerRectBar;
    [SerializeField] private float timeLeft;
    [SerializeField] public float maxTime;

    public float MaxTime //value to be set via OrderManger
    {
        get { { return maxTime; } }
        set { MaxTime = Mathf.Clamp(value, 0.0f, Mathf.Infinity); }
    }

    // Start is called before the first frame update
    void Start()
    {
        
        timeLeft = maxTime;
    }

    // Update is called once per frame
    void Update()
    {
        if(maxTime > 0)
        {
            StartTimer();
        }
        if (timeLeft <= 0)
        {
            DestroyPrefab();
        }
    }

    private void ActivateSashimiCheck() //Upon successful placement of ingredient on plate, check On
    {
        sashimiCheck.SetActive(true);
    }

    private void ActivateRiceCheck()
    {
        riceCheck.SetActive(true);
    }

    public void StartTimer() //Timer to start moving
    {
        if(timeLeft > 0)
        {
            timeLeft -= Time.deltaTime;
            timerRectBar.fillAmount = timeLeft/maxTime;

            if(timeLeft <= maxTime/3.0)
            {
                timerRectBar.color = Color.red;
            }
        }
    }

    public void DestroyPrefab()
    {
        OrderManager.Instance.RemoveDishFromList(this.gameObject);
        
        if(!OrderManager.Instance.IsEmptySpawnLocation())
        {
            OrderManager.Instance.StartCoroutine("TimerForNextOrder");
        }

        orderLocation.GetComponent<SpawnLocationScript>()._IsPrefabPresent = false;
        Destroy(this.gameObject); 
    }

}
