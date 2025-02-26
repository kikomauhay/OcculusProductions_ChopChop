using UnityEngine;
using System;

/// </summary> -WHAT DOES THIS SCRIPT DO-
///
/// Spawns anything that comes out of the game
/// Uses events to handle different spawning types
///
/// </summary>

public class SpawnManager : Singleton<SpawnManager>
{
#region Members

    public Action OnSpawnCustomer, OnSpawnFood;

    [Header("Prefabs to Spawn"), Tooltip("0 = smoke, 1 = bubble, 2 = sparkle, 3 = stinky")]
    [SerializeField] GameObject[] _vfxPrefabs; 
    [SerializeField] GameObject _customerPrefab, _platePrefab;
    
    [Header("Instantiated Areas"), Tooltip("0 = ingredients, 1 = foods, 2 = dishes, 3 = customers, 4 = VFXs")]
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

    public void SpawnVFX(VFXType type, Transform t)
    {
        GameObject vfxInstance = Instantiate(_vfxPrefabs[(int)type], t.position, t.rotation);
        vfxInstance.transform.SetParent(_areas[4]);

        Destroy(vfxInstance, 2f); // destory time could also be variable
    }
    public void SpawnFoodItem(GameObject obj, FoodItemType type, Transform t) 
    {
        obj = Instantiate(obj, t.position, t.rotation); // will need to test this on H if this really works 
        obj.transform.SetParent(_areas[(int)type]);
    }
    public GameObject SpawnCustomer(GameObject obj, Transform t)
    {
        obj = Instantiate(obj, t.position, t.rotation);
        obj.transform.SetParent(_areas[3]);

        Debug.Log("Spawned the customer");

        return obj;
    }
    

#endregion

    void test()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SpawnVFX((VFXType)UnityEngine.Random.Range(0, _vfxPrefabs.Length),
                      transform);
        }

        if (Input.GetKeyDown(KeyCode.Delete))
        {
            SpawnFoodItem(_platePrefab,
                          FoodItemType.INGREDIENT,
                          transform);
        }
    }
}
