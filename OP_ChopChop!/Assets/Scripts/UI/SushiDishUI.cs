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




}
