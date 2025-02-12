using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentCleaning : MonoBehaviour
{
    [SerializeField] GameObject _bubblesVfx;
    private void OnTriggerEnter(Collider other)
    {

        if(other.gameObject.GetComponent<Sponge>() != null &&
            other.gameObject.GetComponent<Sponge>().IsWet == true)
        {
            Vector3 _currentPosition = this.transform.position;
            Quaternion _currentRotation = this.transform.rotation;
            SpawnVFX(_bubblesVfx, _currentPosition, _currentRotation);
            //Reduce Dirty meter
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
