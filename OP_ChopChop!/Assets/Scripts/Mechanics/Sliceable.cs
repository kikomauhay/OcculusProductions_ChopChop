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

    int chopCounter;
    public bool IsAttached = false;

    // Update is called once per frame
    void Update()
    {
        if (chopCounter == 5)
        {
            Sliced();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // check other gameobject if it has a knife script
        Knife knife = other.gameObject.GetComponent<Knife>();

        // if not null, +1 on chop counter
        if (!IsAttached)
            return;
        if (knife != null)
        {
            Debug.Log("Chopping");
            // add vfx here as well
            chopCounter++;
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

            // insert vfx here

            //Instantiate new prefab with the pos and rot values from before
            //Debug.Log("Sliced Ingredient has spawned");
            
            Instantiate(nextPrefab, currentPosition, currentRotation);

        }
    }
}