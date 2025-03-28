using System.Collections;
using UnityEngine;

public class EnvironmentCleaning : MonoBehaviour
{
    [SerializeField]private Collider _collider;

    bool _coroutineActivated = false;

    private void Awake()
    {
        _collider = GetComponent<Collider>();
    }

    /*    void OnEnable() => StartCoroutine(SpawnStinkyVFX());
        void OnDisable() => StopCoroutine(SpawnStinkyVFX());*/

    private void Update()
    {
        if (_collider.enabled && !_coroutineActivated)
        {
            _coroutineActivated = true;
            StartCoroutine(SpawnStinkyVFX());
        }
        else if (!_collider.enabled) 
            StopCoroutine(SpawnStinkyVFX());
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<Sponge>() == null) return;
         
        if (other.gameObject.GetComponent<Sponge>().IsWet)
        {
            SpawnManager.Instance.SpawnVFX(VFXType.BUBBLE, transform, 5f);
            KitchenCleaningManager.Instance.OnCleanedArea?.Invoke();
        }
    }

    private Vector3 RandomColliderPoint(Collider col)
    {
        if (col == null) return transform.position;

        Bounds bounds = col.bounds;

        return new Vector3(Random.Range(bounds.min.x, bounds.max.x),
                           bounds.center.y,
                           Random.Range(bounds.min.z, bounds.max.z));
    }

    IEnumerator SpawnStinkyVFX()
    {
        Debug.Log("Meow");

        while (_collider.enabled)
        {
            Debug.Log("Baho naman");
            Vector3 randPoint = RandomColliderPoint(_collider);
            GameObject tempGameObj = new GameObject("TempSpawnPoint");
            tempGameObj.transform.position = randPoint;

            yield return new WaitForSeconds(5f);
            SpawnManager.Instance.SpawnVFX(VFXType.STINKY, tempGameObj.transform , 5f);
            Destroy(tempGameObj, 0.1F);
            _coroutineActivated = false;
        }
    }
}
