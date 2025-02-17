using UnityEngine;
using System;
using System.Collections;

/// </summary> -WHAT DOES THIS SCRIPT DO-
///
/// Spawns anything that comes out of the game
/// Uses events to handle different spawning types
///
/// </summary>


public enum FoodItemType { INGREDIENT, FOOD, DISH }

public enum VFXType { SMOKE, BUBBLE, SPARKLE, STINKY }

public class SpawnManager : Singleton<SpawnManager>
{
#region Members

    public Action OnSpawnCustomer, OnSpawnFood;

    [Header("Prefabs to Spawn"), Tooltip("0 = smoke, 1 = bubble, 2 = sparkle, 3 = stinky")]
    [SerializeField] GameObject[] _vfxPrefabs; 
    [SerializeField] GameObject _customerPrefab, _platePrefab;
    
    [Header("Areas"), Tooltip("0 = ingredients, 1 = foods, 2 = dishes")]
    [SerializeField] Transform[] _areas; // avoids clutters in the hierarchy  

#endregion

    protected override void Awake() => base.Awake();
    
    protected override void OnApplicationQuit()
    {
        base.OnApplicationQuit();
        Reset();
    }
    void Reset() {} // will add code soon
    void Update() => test();

#region Spawn_Methods

    public void SpawnVFX(VFXType type, Vector3 pos, Quaternion rot)
    {
        GameObject vfxInstance = Instantiate(_vfxPrefabs[(int)type], pos, rot);
        Destroy(vfxInstance, 2f); // destory time could also be variable
    }
    public void SpawnFoodItem(GameObject obj, FoodItemType type, Vector3 pos, Quaternion rot) 
    {
        obj = Instantiate(obj, pos, rot); // will need to test this on H if this really works or not

        // adds the GameObj in another GameObj to avoid cluttering the inspector when testing
        obj.transform.SetParent(_areas[(int)type]);
    }
    

#endregion

    void test()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SpawnVFX((VFXType)UnityEngine.Random.Range(0, _vfxPrefabs.Length),
                      transform.position,
                      transform.rotation);
        }
    }
}
