using UnityEngine;

public class EnvironmentCleaning : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<Sponge>() == null) return;
         
        if (other.gameObject.GetComponent<Sponge>().IsWet)
        {
            SpawnManager.Instance.OnSpawnVFX?.Invoke(VFXType.BUBBLE,
                                                     transform.position,
                                                     transform.rotation);

            CleaningManager.Instance.OnCleanedArea?.Invoke();
        }
    }
}
