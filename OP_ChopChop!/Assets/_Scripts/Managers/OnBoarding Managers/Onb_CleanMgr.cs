using System.Collections;
using UnityEngine;

public class Onb_CleanMgr : StaticInstance<Onb_CleanMgr> 
{
    Collider _collider;

#region Unity

    protected override void Awake() => base.Awake();
    protected override void OnApplicationQuit() => base.OnApplicationQuit();

    [SerializeField] GameObject _dirtySpot;

    void Start ()
    {
        _collider = GetComponent<Collider>();
        _collider.enabled = false;
    }
    void OnDisable() 
    {
        StopCoroutine(SpawnStinkyVFX());
    }
private void OnTriggerEnter(Collider other)
    {
        Sponge sponge = other.gameObject.GetComponent<Sponge>();

        if (sponge == null) return;

        if (sponge.IsWet)
        {
            SpawnManager.Instance.SpawnVFX(VFXType.BUBBLE, sponge.transform, 5f);
            StartCoroutine(DisableObject());
        }
    }

#endregion

#region Enumerators

    public IEnumerator SpawnStinkyVFX()
    {
        _collider.enabled = true;
        
        while (gameObject.activeSelf)
        {
            yield return new WaitForSeconds(5f);
            SpawnManager.Instance.SpawnVFX(VFXType.STINKY, _dirtySpot.transform, 5f);
        }
    }

    IEnumerator DisableObject()
    {
        yield return new WaitForSeconds(5f);
        _collider.enabled = false;
        gameObject.SetActive(false);
    }

#endregion
}
