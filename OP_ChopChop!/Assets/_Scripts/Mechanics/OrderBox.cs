using System.Collections;
using System.Collections.Generic;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine;

public class OrderBox : MonoBehaviour
{
    [SerializeField] GameObject _fish;

    public void OpenBox(SelectEnterEventArgs args)
    {
        if(_fish != null)
        {
            Destroy(transform.parent.gameObject);
            SpawnManager.Instance.SpawnVFX(VFXType.SMOKE, transform, 1F);
            GameObject newFish = SpawnManager.Instance.SpawnObject(_fish,
                                                                   transform,
                                                                   SpawnObjectType.INGREDIENT);
        }
    }
}
