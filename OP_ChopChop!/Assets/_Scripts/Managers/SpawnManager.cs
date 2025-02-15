using UnityEngine;
using System;

/// </summary> -WHAT DOES THIS SCRIPT DO-
///
/// Spawns anything that comes out of the game
/// Uses events to handle different spawning types
///
/// </summary>

public enum VFXType { SMOKE, BUBBLE, SPARKLE, STINKY }

public class SpawnManager : Singleton<SpawnManager>
{
#region Members

    // Events
    public Action<VFXType, Vector3, Quaternion> OnSpawnVFX; // other scripts call this and selects a VFX type
    public Action OnSpawnCustomer, OnSpawnFood;

    [Header("Prefabs to Spawn"), Tooltip("0 = smoke, 1 = bubble, 2 = sparkle, 3 = stinky")]
    [SerializeField] GameObject[] _vfxPrefabs; 
    [SerializeField] GameObject _customerPrefab, _platePrefab;

#endregion

    protected override void Awake() => base.Awake();
    protected override void OnApplicationQuit()
    {
        base.OnApplicationQuit();
        Reset();
    }
    void Reset() 
    {
        OnSpawnVFX -= SpawnVFX;
    }


    void Start()
    {
        OnSpawnVFX += SpawnVFX;
    }

    void Update() => test();

    void SpawnVFX(VFXType type, Vector3 pos, Quaternion rot)
    {
        GameObject vfxInstance = Instantiate(_vfxPrefabs[(int)type], pos, rot);
        Destroy(vfxInstance, 2f); // destory time could also be variable
    }


    void test()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            OnSpawnVFX?.Invoke((VFXType)UnityEngine.Random.Range(0, _vfxPrefabs.Length),
                               transform.position,
                               transform.rotation);
        }
    }
}
