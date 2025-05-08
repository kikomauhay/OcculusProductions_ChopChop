using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine;

public class OrderBox : MonoBehaviour
{
    [SerializeField] private GameObject _fishPrefab;
    
    public void OpenBox(SelectEnterEventArgs args)
    {
        if (_fishPrefab == null) return;

        Destroy(transform.parent.gameObject);

        SpawnManager.Instance.SpawnVFX(VFXType.SMOKE, transform, 2f);
        SpawnManager.Instance.SpawnObject(_fishPrefab,
                                          transform,
                                          SpawnObjectType.INGREDIENT);
    }
}
