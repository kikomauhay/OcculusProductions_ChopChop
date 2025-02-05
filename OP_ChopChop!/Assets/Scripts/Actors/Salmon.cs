using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Salmon : Ingredient
{
    [SerializeField] SliceType _sliceType;

    public SliceType SliceType => _sliceType;

    protected override void Start()
    {
        base.Start();

        _type = IngredientType.SALMON;
        name = "Salmon";
    }

}
