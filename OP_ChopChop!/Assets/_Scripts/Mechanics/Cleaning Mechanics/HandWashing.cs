using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandWashing : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.GetComponent<Sponge>() != null)
        {
            //change sponge into soap or something along the way
            //instantiate bubble vfx
            //set dirty to false after a few seconds of cleaning
        }
    }
}
