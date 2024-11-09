using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NigiriAssembly : MonoBehaviour
{
    [SerializeField]
    GameObject SalmonNigiri;

    [SerializeField]
    GameObject TunaNigiri;
    private void OnTriggerEnter(Collider other)
    {
        //for now use Sliceable Component since we're waiting for the ingredient base class
        if(other.gameObject.GetComponent<SalmonSlice>())
        {
            Vector3 currentPosition = this.transform.position;
            Quaternion currentRotation = this.transform.rotation;
            Destroy(this.gameObject);
            //Insert Smoke Vfx
            Instantiate(SalmonNigiri, currentPosition, currentRotation);
        }
        if(other.gameObject.GetComponent<TunaSlice>())
        {
            Vector3 currentPosition = this.transform.position;
            Quaternion currentRotation = this.transform.rotation;
            Destroy(this.gameObject);
            //Insert Smoke Vfx
            Instantiate(TunaNigiri, currentPosition, currentRotation);
        }
    }
}
