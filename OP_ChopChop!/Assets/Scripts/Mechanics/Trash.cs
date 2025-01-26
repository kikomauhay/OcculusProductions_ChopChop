using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trash : MonoBehaviour
{
    private void OnTriggerEnter(Collider _other)
    {
        if (_other.gameObject.GetComponent<Destructable>() != null)
        {
            Destroy(_other.gameObject);
        }
    }
}
