using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateCleanCol : MonoBehaviour
{
    //Standardized script for activating colliders on the hand, will draft it up and will ask help from isagani to clean up the code later
    //prolly rename this script to clean manager or smth
    [SerializeField] Collider[] _handWashColliders;
    [SerializeField] Collider[] _KitchenWashColliders;

    public void ActivateHandWashColliders()
    {
        if (_handWashColliders == null) return;

        //put in code to enable colliders here, tried doing it this morning but it wouldnt show up in intellisense
    }
    private void ActivateKitchenWashColliders()
    {
        if (_KitchenWashColliders == null) return;  
        // if deterioration of kitchen falls below 100, activate colliders
        // update texture of kitchen, if di kaya spark na lng siya
        // make sure smelly vfx comes out of the colliders to signify the player that their kitchen needs cleaning
    }
}
