using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class CheatClean : MonoBehaviour
{
    [SerializeField]
    GameObject BubblesVFX;

    private void OnTriggerEnter(Collider other)
    {
       if (other.gameObject.GetComponent<Sponge>() 
            || other.gameObject.GetComponent<ActionBasedController>())
        {
            Vector3 currentPosition = other.transform.position;
            Quaternion currentRotation = other.transform.rotation;
            SpawnVFX(BubblesVFX, currentPosition, currentRotation);
        }
       if (this.gameObject.GetComponent<Sponge>() 
            && other.gameObject.GetComponent<Knife>())
        {
            Vector3 currentPosition = this.transform.position;
            Quaternion currentRotation = this.transform.rotation;
            SpawnVFX(BubblesVFX, currentPosition, currentRotation);
        }
    }
    void SpawnVFX(GameObject vfxPrefab, Vector3 position, Quaternion rotation)
    {
        if (vfxPrefab != null)
        {
            GameObject VFXInstance = Instantiate(vfxPrefab, position, rotation);
            Destroy(VFXInstance, 2f);
        }
    }
}
