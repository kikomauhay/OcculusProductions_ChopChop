using UnityEngine;

public class DishCleaning : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<Sponge>() == null) return;
        
        // bug: one collision will make the plate clean
        // future fix: add counters to imitate the feeling of cleaning

        if (other.gameObject.GetComponent<Sponge>().IsWet)
        {
            SpawnManager.Instance.SpawnVFX(VFXType.BUBBLE, transform);
            other.gameObject.GetComponent<Plate>().ToggleClean();
        }
    }
}
