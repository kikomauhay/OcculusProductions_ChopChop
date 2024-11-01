using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Reparent : MonoBehaviour
{
    private XRGrabInteractable grabInteractable;

    private void Awake()
    {
        grabInteractable = GetComponent<XRGrabInteractable>();
        grabInteractable.selectEntered.AddListener(DetachFromBoard);
    }

    private void OnDestroy()
    {
        grabInteractable.selectEntered.RemoveListener(DetachFromBoard);
    }

    void DetachFromBoard(SelectEnterEventArgs args)
    {
      if(this.gameObject.GetComponent<Sliceable>().IsAttached == true)
        {
            transform.SetParent(null);

            Rigidbody rb = this.gameObject.GetComponent<Rigidbody>();
            this.gameObject.GetComponent<Sliceable>().IsAttached = false;
            rb.isKinematic = false;
            rb.WakeUp();
        }
    }
}
