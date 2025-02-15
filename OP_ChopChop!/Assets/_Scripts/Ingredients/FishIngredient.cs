using UnityEngine;
using System;

/// <summary>
/// 
/// Base class for different types of fish
/// 
/// </summary>

public enum SliceType { THICK, THIN, SLAB }

public enum FishType { SALMON, TUNA }

public class FishIngredient : Ingredient
{
    public Action<int> OnFishSliced;

    public SliceType SliceType => _sliceType;
    public FishType FishType => _fishType;

    [Tooltip("Types of Slices Available")]
    [SerializeField] protected GameObject _fishSlices;
    
    [Header("Food Attributes")]
    [SerializeField] protected SliceType _sliceType;
    [SerializeField] protected FishType _fishType; 
    

    protected override void Start()
    {
        base.Start();
        _sliceType = SliceType.SLAB;

        OnFishSliced += UpdateSlice; 
    }
    protected void Reset() => OnFishSliced -= UpdateSlice;

    protected void UpdateSlice(int i)
    {
        _sliceType = (SliceType)i;
        // change to incrementing index so it's easier to change (and prevents backtracking)
    }
}