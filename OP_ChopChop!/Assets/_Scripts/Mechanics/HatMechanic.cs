using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HatMechanic : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        ChefHat hat = other.gameObject.GetComponent<ChefHat>();
        if(hat != null)
        {
            hat.StartService();
        }
    }
}
