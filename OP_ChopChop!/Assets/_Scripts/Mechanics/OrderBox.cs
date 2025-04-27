using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine;

public class OrderBox : MonoBehaviour
{
    public bool IsTutorial { get; set; }
    [SerializeField] private GameObject _fishPrefab, _smokePrefab;

    public void OpenBox(SelectEnterEventArgs args)
    {
        if (_fishPrefab != null)
        {
            Destroy(transform.parent.gameObject);
            
            if (IsTutorial)
            {
                Instantiate(_fishPrefab, transform.position, transform.rotation);
                GameObject smoke = Instantiate(_smokePrefab, 
                                               transform.position, 
                                               transform.rotation);
                Destroy(smoke, 1f); 
                return;
            }
            else 
            {
                SpawnManager.Instance.SpawnVFX(VFXType.SMOKE, transform, 1f);
                SpawnManager.Instance.SpawnObject(_fishPrefab,
                                                  transform,
                                                  SpawnObjectType.INGREDIENT);
            }
        }
    }
}
