using System.Runtime.InteropServices;
using UnityEngine;

public class Sponge : Equipment
{
    public bool IsWet { get; private set; } 

    [SerializeField] Material _wetMat;

    protected override void Start()
    {
        base.Start();

        name = "Sponge";
        IsWet = false;
        _maxUsageCounter = 10;
    }

    public void ToggleWetness() 
    {
        IsWet = !IsWet;

        if (!IsClean)
        {
            GetComponent<Renderer>().material = _dirtyMat;
            return;
        }

        // ternary operator syntax -> condition ? val_if_true : val_if_false
        GetComponent<Renderer>().material = IsWet ? 
                                            _wetMat : _cleanMat;
    }
}
