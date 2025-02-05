using System;
using UnityEngine;

public enum MoldType { UNMOLDED, GOOD, PERFECT, BAD }

public class Rice : Ingredient
{
    [Header("Finished Dishes"), Tooltip("Possible dishes the rice can combine with.")]
    [SerializeField] GameObject[] _dishPrefabs; // make sure the order is correct

    [Header("VFX Settings")]
    [SerializeField] GameObject _smokeVFX;
    [SerializeField] float _vfxDestroyTime; // was initially at 2f  

    [Header("Molding Attributes")]
    [SerializeField] MoldType _moldType;
    public Action<int> OnRiceMolded;

    protected override void Start() 
    {
        base.Start();

        OnRiceMolded += ChangeRiceMold;
    }

    void OnTriggerEnter(Collider other)
    {
        // prevents mixing the same ingredients together
        if (other.gameObject.name == name) return;

        // only perfectly-molded rice is allowed
        if (_moldType != MoldType.PERFECT) return;

        Ingredient ing = other.GetComponent<Ingredient>();
        Vector3 pos = transform.position;
        Quaternion rot = transform.rotation;

        // gets the freshness rates of both ingredients before deleting them
        GameManager.Instance.OnDishCreated?.Invoke(FreshnessRate, ing.FreshnessRate);
        Destroy(gameObject);
        Destroy(other.gameObject);
        CreateVFX(_smokeVFX, pos, rot);

        if (ing.Type == IngredientType.SALMON &&
            ing.GetComponent<Salmon>().SliceType == SliceType.THIN)
        {
            Instantiate(_dishPrefabs[0], pos, rot); // salmon nigiri
        }

        else if (ing.Type == IngredientType.TUNA &&
            ing.GetComponent<Tuna>().SliceType == SliceType.THIN)
        {
            Instantiate(_dishPrefabs[1], pos, rot); // tuna nigiri
        }
    }
    void CreateVFX(GameObject vfxPrefab, Vector3 pos, Quaternion rot)
    {
        if (vfxPrefab != null)
        {
            GameObject _VFXInstance = Instantiate(vfxPrefab, pos, rot);
            Destroy(_VFXInstance, _vfxDestroyTime);
        }
    }

    void ChangeRiceMold(int moldIndex) => _moldType = (MoldType)moldIndex;
    
}