using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using System;

public enum VFXType { SMOKE, BUBBLE, SPARKLE , STINKY }

public class SpawnManager : Singleton<SpawnManager>
{
#region Events 

    public Action<VFXType, Vector3, Quaternion> OnSpawnVFX; // other scripts call this and selects a VFX type
    public Action OnSpawnCustomer;

#endregion

#region Members

    [Header("Prefabs to Spawn")]
    [SerializeField] GameObject[] _vfxPrefabs; // 0 = smoke, 1 = bubble, 2 = sparkle, 3 = stinky
    [SerializeField] GameObject _customerPrefab, _platePrefab;

#endregion

#region Methods
    
#endregion

    protected override void Awake() => base.Awake();
    protected override void OnApplicationQuit()
    {
        base.OnApplicationQuit();

        OnSpawnVFX -= SpawnVFX;
    }


    void Start()
    {
        OnSpawnVFX += SpawnVFX;
    }

    void Update() => test();

    void SpawnVFX(VFXType type, Vector3 pos, Quaternion rot)
    {
        GameObject _VFXInstance = Instantiate(_vfxPrefabs[(int)type], pos, rot);
        Destroy(_VFXInstance, 2f);
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
