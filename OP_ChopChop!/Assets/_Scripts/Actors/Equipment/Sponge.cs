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
        yield return new WaitForSeconds(6f);
        IsWet = false;
        GetComponent<Renderer>().material = _cleanMat;
    }

#endregion

    public void ToggleWet() 
    {
        IsWet = !IsWet;

        if (!IsClean)
        {
            GetComponent<Renderer>().material = _dirtyMat;
            return;
        }

        StartCoroutine(Dry());

        // ternary operator syntax -> condition ? val_if_true : val_if_false
        GetComponent<Renderer>().material = IsWet ? 
                                            _wetMat : _cleanMat;
    }
}
