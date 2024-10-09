using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floor : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<ResetPos>() != null)
        {
            DelayedReset(other);
        }
    }

    private IEnumerator DelayedReset(Collider other)
    {
        yield return new WaitForSeconds(10);
        Destroy(other.gameObject);
    }
}
