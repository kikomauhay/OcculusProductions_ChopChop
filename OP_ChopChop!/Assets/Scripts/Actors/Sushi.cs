using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DishType
{
    Nigiri_Salmon,
    Nigiri_Tuna,
    Maki_Salmon,
    Maki_Tuna,
}

public class Sushi : MonoBehaviour
{
    [SerializeField] public DishType dishType;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
