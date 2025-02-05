using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class Tuna : Ingredient
{

    [SerializeField] SliceType _sliceType;

    public SliceType SliceType => _sliceType;

    protected override void Start()
    {
        base.Start();

        _type = IngredientType.TUNA;
        name = "Tuna";
    }
}
