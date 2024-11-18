using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NigiriAssembly : MonoBehaviour
{
    [SerializeField]
    GameObject SalmonNigiri;

    [SerializeField]
    GameObject TunaNigiri;

    [SerializeField]
    GameObject SmokeVFX;
    private void OnTriggerEnter(Collider other)
    {
        //for now use Sliceable Component since we're waiting for the ingredient base class
        if(other.gameObject.GetComponent<SalmonSlice>())
        {
            Vector3 currentPosition = this.transform.position;
            Quaternion currentRotation = this.transform.rotation;
            Destroy(this.gameObject);
            Destroy(other.gameObject);
            SpawnVFX(SmokeVFX, currentPosition, currentRotation);
            Instantiate(SalmonNigiri, currentPosition, currentRotation);
        }
        if(other.gameObject.GetComponent<TunaSlice>())
        {
            Vector3 currentPosition = this.transform.position;
            Quaternion currentRotation = this.transform.rotation;
            Destroy(this.gameObject);
            SpawnVFX(SmokeVFX, currentPosition, currentRotation);
            Instantiate(TunaNigiri, currentPosition, currentRotation);
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
