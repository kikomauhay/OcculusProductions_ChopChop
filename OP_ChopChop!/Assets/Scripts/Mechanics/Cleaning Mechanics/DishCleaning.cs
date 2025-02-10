using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DishCleaning : MonoBehaviour
{
    [SerializeField] GameObject _bubblesVfx;
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.GetComponent<Sponge>() != null &&
            other.gameObject.GetComponent<Sponge>()._isWet == true)
        {
            Plate _plate = this.gameObject.GetComponent<Plate>();
            Vector3 _currentPosition = this.transform.position;
            Quaternion _currentRotation = this.transform.rotation;
            SpawnVFX(_bubblesVfx, _currentPosition, _currentRotation);
            _plate.Cleaned();
            // make sure to add another if statement to check if the sponge is wet or just add it in the first condition as &&
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
