using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floor : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<ResetPos>() != null)
        {
            Debug.Log("Detected");
            StartCoroutine(IDelayedReset(other));
        }
    }

    private IEnumerator IDelayedReset(Collider other)
    {
        yield return new WaitForSeconds(5);
        Destroy(other.gameObject);
    }
}
