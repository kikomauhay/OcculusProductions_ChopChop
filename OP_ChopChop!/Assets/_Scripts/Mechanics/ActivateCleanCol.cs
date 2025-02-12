using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateCleanCol : MonoBehaviour
{
    //Standardized script for activating colliders on the hand, will draft it up and will ask help from isagani to clean up the code later
    [SerializeField] Collider[] _handWashColliders;

    public void ActivateWashableColliders()
    {
        if (_handWashColliders == null) return;

        //put in code to enable colliders here, tried doing it this morning but it wouldnt show up in intellisense
    }
}
