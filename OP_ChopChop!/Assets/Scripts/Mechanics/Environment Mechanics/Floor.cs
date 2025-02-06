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
                other.gameObject.GetComponent<Ingredient>().ContaminateFood();
            }
            if (other.gameObject.GetComponent<Trashable>()._trashTypes == TrashTypes.Food)
            {
                other.gameObject.GetComponent<Ingredient>().ContaminateFood();
            }
            if (other.gameObject.GetComponent<Trashable>()._trashTypes == TrashTypes.Equipment)
            {
                //Reset Equipment here
            }
        }
    }
}
