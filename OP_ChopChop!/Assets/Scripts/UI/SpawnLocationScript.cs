using UnityEngine;

public class SpawnLocationScript : MonoBehaviour
{
    // what the fuck does this mean
    [SerializeField] public bool _isPrefabPresent;

    public bool IsPrefabPresent
    {
        get { return _isPrefabPresent; }
        set { _isPrefabPresent = value; }  
    }
    
}
