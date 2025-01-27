using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Serve : MonoBehaviour
{
    // needs an explanation
    private void OnTriggerEnter(Collider _other)
    {
       if(_other.gameObject.GetComponent<Plate>()!= null)
       {
            //OrderManager.Instance.OrderComplete(other.gameObject);
            Destroy(_other.gameObject);     
       }
    }
}
