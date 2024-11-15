using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Serve : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
       if(other.gameObject.GetComponent<Plate>()!= null)
       {
            Destroy(other.gameObject);
       }
    }
}
