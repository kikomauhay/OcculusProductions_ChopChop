using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnLocationScript : MonoBehaviour
{
    [SerializeField] private bool isPrefabPresent;

    public bool _IsPrefabPresent
    {
        get { return isPrefabPresent; }
        set { isPrefabPresent = value; }  
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
