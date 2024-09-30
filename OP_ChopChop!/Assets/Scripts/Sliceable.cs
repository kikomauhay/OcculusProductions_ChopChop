using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sliceable : MonoBehaviour
{
    [SerializeField]
    GameObject ingredientPrefab;

    [SerializeField]
    GameObject finishedPrefab;

    [SerializeField]
    GameObject sharpObject;

    int chopCounter;

    // Update is called once per frame
    void Update()
    {
        Debug.Log("Chop Counter: " + chopCounter);
        if (chopCounter == 5)
        {
            Sliced();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // check other gameobject if it has a knife script
        Knife knife = collision.gameObject.GetComponent<Knife>();

        // if not null, +1 on chop counter
        if (knife != null)
        {
            // add vfx here as well
            chopCounter++;
        }

    }

    void Sliced()
    {
        if(ingredientPrefab != null)
        {
            // Get pos and rotation of prefab and then destroy
            Vector3 currentPosition = ingredientPrefab.transform.position;
            Quaternion currentRotation = ingredientPrefab.transform.rotation;

            Destroy(ingredientPrefab);

            // insert vfx here

            //Instantiate new prefab with the pos and rot values from before
            Debug.Log("Sliced Ingredient has spawned");

        }
    }
}