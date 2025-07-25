using System.Collections;
using UnityEngine;

public class EnvironmentCleaning : MonoBehaviour
{
    #region Members 

    [SerializeField] private bool _isTutorial;  
    [SerializeField] private Collider _col;

    private static bool _tutorialDone = false;

    #endregion

    #region Unity

    private void OnEnable() 
    {
        StartCoroutine(CO_SpawnStinkyVFX());
        _col.enabled = true;
    }
    private void OnDisable() => StopCoroutine(CO_SpawnStinkyVFX());
    private void OnTriggerEnter(Collider other)
    {
        Sponge sponge = other.gameObject.GetComponent<Sponge>();

        if (sponge == null)
        {
            Debug.LogError($"{other.name} doens't have a Sponge Component!");
            return;
        }

        if (sponge.IsWet)
            SpawnManager.Instance.SpawnVFX(VFXType.BUBBLE, sponge.transform, 5f);

        if (_isTutorial && !_tutorialDone)
        {
            OnBoardingHandler.Instance.AddOnboardingIndex();
            OnBoardingHandler.Instance.PlayOnboarding();

            _tutorialDone = true;
            gameObject.SetActive(false);
            Debug.LogWarning($"{this} is disabled!");
            return;
        }

        // confirmation that the cleaning has been done
        KitchenCleaningManager.Instance.OnCleanedArea?.Invoke();
        gameObject.SetActive(false);
        Debug.LogWarning($"{this} has been disabled!");
    }

    #endregion
    #region Helpers

    private Vector3 RandomColliderPoint(Collider col)
    {
        if (col == null) return transform.position;

        Bounds bounds = col.bounds;

        return new Vector3(Random.Range(bounds.min.x, bounds.max.x),
                           bounds.center.y,
                           Random.Range(bounds.min.z, bounds.max.z));
    }

    #endregion
    
    #region Enumerators

    private IEnumerator CO_SpawnStinkyVFX()
    {
        // loops while it's still enabled
        while (gameObject.activeSelf)
        {
            // spawns an empty game obj at a random point in the collider
            Vector3 randPoint = RandomColliderPoint(_col);
            GameObject tempGameObj = new GameObject("TempSpawnPoint");
            tempGameObj.transform.position = randPoint;
            yield return new WaitForSeconds(5f);

            // spawns the VFX in the new game obj's position
            SpawnManager.Instance.SpawnVFX(VFXType.STINKY, tempGameObj.transform, 5f);
            Destroy(tempGameObj, 0.1f);
        }
    }

    #endregion     
}
