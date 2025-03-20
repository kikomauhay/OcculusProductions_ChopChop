using System.Collections;
using UnityEngine;

public class EnvironmentCleaning : MonoBehaviour
{
    void OnEnable() => StartCoroutine(SpawnStinkyVFX());
    void OnDisable() => StopCoroutine(SpawnStinkyVFX());

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<Sponge>() == null) return;
         
        if (other.gameObject.GetComponent<Sponge>().IsWet)
        {
            SpawnManager.Instance.SpawnVFX(VFXType.BUBBLE, transform, 5f);
            CleaningManager.Instance.OnCleanedArea?.Invoke();
        }
    }

    IEnumerator SpawnStinkyVFX()
    {
        SpawnManager.Instance.SpawnVFX(VFXType.STINKY, transform, 5f);

        while (gameObject.activeSelf)
        {
            yield return new WaitForSeconds(5f);
            SpawnManager.Instance.SpawnVFX(VFXType.STINKY, transform, 5f);
        }
    }
}
