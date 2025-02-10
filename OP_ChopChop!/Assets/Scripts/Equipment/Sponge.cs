using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sponge : MonoBehaviour
{
   [SerializeField] public bool _isWet;

    public void Dried()
    {
        _isWet = false;
    }
    public void Wet()
    {
        _isWet = true;   
    }

}
