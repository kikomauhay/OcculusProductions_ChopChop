using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plate : MonoBehaviour
{
    [SerializeField] bool _isDirty;

    private void Start()
    {
        _isDirty = false;
    }

    public void Contaminated()
    {
        _isDirty = true;
    }

    public void Cleaned()
    {
        _isDirty = false;
    }


}
