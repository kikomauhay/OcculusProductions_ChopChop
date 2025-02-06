using UnityEngine;
using System;

/// <summary>
/// 
/// Base class for different types of fish
/// 
/// </summary>

public enum SliceType { THICK, THIN, SLAB }

public class FishIngredient : Ingredient
{
    public Action<int> OnFishSliced;

    [Header("Fish Slices")]
    [SerializeField] protected GameObject _fishSlices;
    [SerializeField] SliceType _sliceType;
    public SliceType SliceType => _sliceType;


    protected override void Start()
    {
        base.Start();
        _sliceType = SliceType.SLAB;

        OnFishSliced += UpdateSlice; 
    }

    protected void UpdateSlice(int i)
    {
        _sliceType = (SliceType)i;
    }
}