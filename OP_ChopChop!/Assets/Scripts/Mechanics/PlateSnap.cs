using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateSnap : MonoBehaviour
{
    [SerializeField]
    float AttachY;
    [SerializeField]
    float Timer;
    [SerializeField]
    float VFXScale;
    [SerializeField]
    GameObject BubblesVFX;

    public Collider SnapCollider;
    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<SalmonNigiri>() != null)
        {
            SnapToObject(other.transform);
            DisableRigidbody(other);
            SnapCollider.enabled = false;
        }
        else if(other.GetComponent<Sponge>() != null)
        {
            Vector3 currentPosition = this.transform.position;
            Quaternion currentRotation = this.transform.rotation;

            SpawnVFX(BubblesVFX, currentPosition, currentRotation);
        }
        else
        {
            StartCoroutine(IResetTrigger());
        }
    }

    void SnapToObject(Transform FoodObject)
    {
        FoodObject.SetParent(transform);
        FoodObject.localPosition = new Vector3(0, AttachY, 0);
        FoodObject.localRotation = Quaternion.Euler(0, FoodObject.localRotation.eulerAngles.y, 0);

    }

    void DisableRigidbody(Collider other)
    {
        Rigidbody rb = other.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = true;
        }
    }

    private IEnumerator IResetTrigger()
    {
        yield return new WaitForSeconds(Timer);
        SnapCollider.enabled = true;
    }
    void SpawnVFX(GameObject vfxPrefab, Vector3 position, Quaternion rotation)
    {
        if (vfxPrefab != null)
        {
            GameObject VFXInstance = Instantiate(vfxPrefab, position, rotation);
            VFXInstance.transform.localScale *= VFXScale; 
            Destroy(VFXInstance, 2f);
        }
    }
}
