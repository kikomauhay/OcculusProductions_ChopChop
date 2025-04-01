using System.Collections;
using UnityEngine;

public class Sponge : Equipment
{
    public bool IsWet { get; private set; } 

    [SerializeField] Material _wetMat;

#region Unity_Methods

    protected override void Start()
    {
        base.Start();

        name = "Sponge";
        IsWet = false;
        _maxUsageCounter = 10;
    }
    protected override void OnTriggerEnter(Collider other) {}
    
    IEnumerator Dry()
    {
        GetComponent<Renderer>().material = _cleanMat;
        yield return new WaitForSeconds(4f);
        IsWet = false;
    }

#endregion

    public void SetWet() 
    {
        IsWet = true;
        DoCleaning();
        GetComponent<MeshRenderer>().material = _wetMat;
        StartCoroutine(Dry());
    }
}
