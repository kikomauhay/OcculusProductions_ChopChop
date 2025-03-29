using System.Collections;
using System.Collections.Generic;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine;

public class OrderBox : XRBaseInteractable
{
    [SerializeField] GameObject _fish;

    protected override void OnEnable()
    {
        base.OnEnable();
        selectEntered.AddListener(OpenBox);
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        selectEntered.RemoveListener(OpenBox);
    }

    private void Start()
    {
        if (interactionManager == null)
        {
            interactionManager = FindObjectOfType<XRInteractionManager>();
        }
    }

    void OpenBox(SelectEnterEventArgs args)
    {
        if(_fish != null)
        {
            Destroy(transform.parent.gameObject);
            SpawnManager.Instance.SpawnVFX(VFXType.SMOKE, transform, 1F);
            SpawnManager.Instance.SpawnObject(_fish,
                                              transform,
                                              SpawnObjectType.INGREDIENT);
        }
    }
}
