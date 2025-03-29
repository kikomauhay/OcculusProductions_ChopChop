using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Freezer : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
       if(other.gameObject.GetComponent<Ingredient>() != null)
       {
            other.gameObject.GetComponent<Ingredient>().Stored();
       }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.GetComponent<Ingredient>() != null)
        {
            other.gameObject.GetComponent<Ingredient>().Unstored();
        }
    }
}
