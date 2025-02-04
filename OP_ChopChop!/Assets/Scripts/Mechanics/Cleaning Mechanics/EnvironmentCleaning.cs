using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentCleaning : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.GetComponent<Sponge>() != null)
        {
            //Instantiate bubbles
            //Reduce Dirty meter
            //make sure to add another if statement to check if the sponge is wet or just add it in the first condition as &&
        }
    }
}
