using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floor : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<Trashable>() != null)
        {
            if (other.gameObject.GetComponent<Trashable>()._trashTypes == TrashTypes.Ingredients)
            {
                //Increase Decay Rate
                //Call public function from ingredient
            }
            if (other.gameObject.GetComponent<Trashable>()._trashTypes == TrashTypes.Food)
            {
                //Increase Decay Rate
                //Call public function from ingredient
            }
            if (other.gameObject.GetComponent<Trashable>()._trashTypes == TrashTypes.Equipment)
            {
                //Reset Equipment here
            }
        }
    }
}
