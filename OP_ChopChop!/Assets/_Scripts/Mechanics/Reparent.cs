using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Reparent : MonoBehaviour
{
    private XRGrabInteractable _grabInteractable;

    private void Awake()
    {
        _grabInteractable = GetComponent<XRGrabInteractable>();
        _grabInteractable.selectEntered.AddListener(DetachFromBoard);
    }

    private void OnDestroy()
    {
        _grabInteractable.selectEntered.RemoveListener(DetachFromBoard);
    }

    void DetachFromBoard(SelectEnterEventArgs args)
    {
      if(this.gameObject.GetComponent<Sliceable>().IsAttached == true)
        {
            transform.SetParent(null);

            this.gameObject.GetComponent<Rigidbody>().isKinematic = false;
            this.gameObject.GetComponent<Collider>().isTrigger = false;
            this.gameObject.GetComponent<Sliceable>().IsAttached = false;
        }
    }
}
