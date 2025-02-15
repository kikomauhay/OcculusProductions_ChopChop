using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class SetPokeTofingerAttachPoint : MonoBehaviour
{
    [SerializeField] public Transform pokeAttachPoint;
    private XRPokeInteractor xrPokeInteractor;

    // Start is called before the first frame update
    void Start()
    {
        xrPokeInteractor = transform.parent.parent.GetComponent<XRPokeInteractor>();
        SetPokeAttachPoint();
    }

   void SetPokeAttachPoint()
    {
        if(pokeAttachPoint == null)
        {
            Debug.Log("Poke Attach point is null"); 
            return;
        }

        if(xrPokeInteractor == null)
        {
            Debug.Log("XR Poke Interactor is null");
            return;
        }

        xrPokeInteractor.attachTransform = pokeAttachPoint;
    }
}
