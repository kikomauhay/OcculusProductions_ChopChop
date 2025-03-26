using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Water : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.GetComponent<Sponge>() != null)
        {
            other.gameObject.GetComponent<Sponge>().Wet();
        }
    }
}
