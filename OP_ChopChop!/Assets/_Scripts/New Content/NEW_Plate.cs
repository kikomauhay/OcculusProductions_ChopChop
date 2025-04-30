using System.Collections;
using UnityEngine;
using System;

/// <summary>
/// 
/// - Acts as the reworked version of Plate.cs
/// 
/// </summary>

[RequireComponent(typeof(NEW_Dish))]
public class NEW_Plate : Equipment
{
#region Properties

    public bool IsPlated { get; private set; }

#endregion

#region Private

    private NEW_Dish _dish;

#endregion

#region Unity

    private void Awake()
    {
        _dish = GetComponent<NEW_Dish>();
    }
    protected override void Start()
    {
        base.Start();
        _maxUsageCounter = 1;
    }

#endregion

#region Public

    public override void HitTheFloor()
    {
        base.HitTheFloor();
        // contaminate the enabled child ingredient
    }
#endregion

#region Helpers

#endregion
}
