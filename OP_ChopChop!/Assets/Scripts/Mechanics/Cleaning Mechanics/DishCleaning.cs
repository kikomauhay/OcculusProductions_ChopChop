using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DishCleaning : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.GetComponent<Sponge>() != null)
        {
            // Instantiate bubbles
            // Set dirty varaible to false
            // make sure to add another if statement to check if the sponge is wet or just add it in the first condition as &&
        }
    }
}
