using System.Collections;
using UnityEngine;

public class Onb_CleanMgr : StaticInstance<Onb_CleanMgr> 
{

    protected override void Awake() => base.Awake();
    protected override void OnApplicationQuit() => base.OnApplicationQuit();

    //Enable GameObject once you reach onb_P6

    void OnEnable() => StartCoroutine(SpawnStinkyVFX());
    void OnDisable() => StopCoroutine(SpawnStinkyVFX());

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

    IEnumerator SpawnStinkyVFX()
    {
        while (gameObject.activeSelf)
        {

            yield return new WaitForSeconds(5f);
            SpawnManager.Instance.SpawnVFX(VFXType.STINKY, transform, 5f);
        }
    }

    IEnumerator DisableObject()
    {
        yield return new WaitForSeconds(5F);
        this.gameObject.SetActive(false);
        //proceed to onb_P7 once disabled
    }

}
