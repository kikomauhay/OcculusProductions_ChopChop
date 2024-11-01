using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floor : MonoBehaviour
{
    [SerializeField]
    float timer;
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
        yield return new WaitForSeconds(timer);
        Destroy(other.gameObject);
    }
}
