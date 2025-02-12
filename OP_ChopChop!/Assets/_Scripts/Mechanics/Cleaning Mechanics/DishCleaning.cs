using UnityEngine;

public class DishCleaning : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<Sponge>() == null) return;
        
        // bug: one collision will make the plate clean
        // future fix: add counters to imitate the feeling of cleaning

        if (other.gameObject.GetComponent<Sponge>().IsWet)
        {
            Plate plate = gameObject.GetComponent<Plate>();

            SpawnManager.Instance.OnSpawnVFX?.Invoke(VFXType.BUBBLE,
                                                     transform.position,
                                                     transform.rotation);
 
            plate.SetCleaned();
        }
    }
}
