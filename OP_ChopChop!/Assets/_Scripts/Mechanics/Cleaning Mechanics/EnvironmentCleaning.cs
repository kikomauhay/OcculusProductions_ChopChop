using System.Collections;
using UnityEngine;

public class EnvironmentCleaning : MonoBehaviour
{
#region Members 

    [SerializeField] private bool _isTutorial;
    [SerializeField] private GameObject _stinkyVFX, _bubbleVFX;
    private Collider _col;

#endregion

#region Unity

    private void OnEnable() => StartCoroutine(SpawnStinkyVFX());
    private void OnDisable() => StopCoroutine(SpawnStinkyVFX());

    private void Awake()
    {
        _col = GetComponent<Collider>();
        
        if (_isTutorial)
            _col.enabled = false;    
    }

    private void OnTriggerEnter(Collider other)
    {
        Sponge sponge = other.gameObject.GetComponent<Sponge>();
     
        if (sponge == null) return;
        
        if (sponge.IsWet)
            SpawnManager.Instance.SpawnVFX(VFXType.BUBBLE, sponge.transform, 5f);
        
        if (_isTutorial) 
        {
            StartCoroutine(OnBoardingHandler.Instance.Onboarding09());
            gameObject.SetActive(false);
            return; 
        }
        
        KitchenCleaningManager.Instance.OnCleanedArea?.Invoke();
        gameObject.SetActive(false);
    }

    private IEnumerator SpawnStinkyVFX()
    {
        if (_isTutorial) yield break;

        while (gameObject.activeSelf)
        {
            Vector3 randPoint = RandomColliderPoint(_col);
            GameObject tempGameObj = new GameObject("TempSpawnPoint");
            tempGameObj.transform.position = randPoint;

            yield return new WaitForSeconds(5f);
            SpawnManager.Instance.SpawnVFX(VFXType.STINKY, tempGameObj.transform, 5f);
            Destroy(tempGameObj, 0.1F);
        }
    }

#endregion 

    private Vector3 RandomColliderPoint(Collider col)
    {
        if (col == null) return transform.position;

        Bounds bounds = col.bounds;

        return new Vector3(Random.Range(bounds.min.x, bounds.max.x),
                           bounds.center.y,
                           Random.Range(bounds.min.z, bounds.max.z));
    }
}
