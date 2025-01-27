using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum UIDishType
{
    Nigiri_Salmon,
    Nigiri_Tuna,
    Maki_Salmon,
    Maki_Tuna,
}

public class SushiDishUI : MonoBehaviour
{
    [SerializeField] public UIDishType dishType;
    [SerializeField] private GameObject orderLocation;

    public GameObject _OrderLocation
    {
        get { return orderLocation; }
        set { orderLocation = value; }
    }

    [SerializeField] private Image timerRectBar;
    [SerializeField] private float timeLeft;
    [SerializeField] public float maxTime;

    public float GetTimeLeft
    {
        get { { return timeLeft; } }
    }
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

        orderLocation.GetComponent<SpawnLocationScript>().IsPrefabPresent = false;
        Destroy(this.gameObject); 
    }

}
