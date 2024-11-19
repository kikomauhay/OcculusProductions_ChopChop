using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sliceable : MonoBehaviour
{
    [SerializeField]
    GameObject currentPrefab;

    [SerializeField]
    GameObject nextPrefab;

    [SerializeField]
    GameObject sharpObject;

    [SerializeField]
    GameObject MeatBoard;

    [SerializeField]
    GameObject SmokeVFX;

    int chopCounter;
    public bool IsAttached = false;

    private void Start()
    {
        sharpObject = EquipmentManager.Instance?.Knife;
        MeatBoard = EquipmentManager.Instance?.MeatBoard;
    }

    // Update is called once per frame
    void Update()
    {
        if (chopCounter >= 5)
        {
            Debug.Log("SLICED");
            Sliced();
            MeatBoard.gameObject.GetComponent<Snap>().ResetSnap();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // check other gameobject if it has a knife script
        Knife knife = other.gameObject.GetComponent<Knife>();

        // if not null, +1 on chop counter
        if (IsAttached)
        {
            if (knife != null)
            {
                Vector3 currentPosition = currentPrefab.transform.position;
                Quaternion currentRotation = currentPrefab.transform.rotation;
                Debug.Log("Chopping");
                SpawnVFX(SmokeVFX, currentPosition, currentRotation);
                chopCounter++;
            }

        }
    }

    void Sliced()
    {
        if(currentPrefab != null)
        {
            // Get pos and rotation of prefab and then destroy
            Vector3 currentPosition = currentPrefab.transform.position;
            Quaternion currentRotation = currentPrefab.transform.rotation;

            Destroy(currentPrefab);
            SpawnVFX(SmokeVFX, currentPosition, currentRotation);
            Instantiate(nextPrefab, currentPosition, currentRotation);

        }
    }

    void SpawnVFX(GameObject vfxPrefab, Vector3 position, Quaternion rotation)
    {
        if(vfxPrefab != null)
        {
            GameObject VFXInstance = Instantiate(vfxPrefab, position, rotation);
            Destroy(VFXInstance, 2f);
        }    
    }
}