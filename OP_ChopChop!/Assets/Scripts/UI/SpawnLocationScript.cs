using UnityEngine;

public class SpawnLocationScript : MonoBehaviour
{
    [SerializeField] private bool _isPrefabPresent;

    public bool IsPrefabPresent
    {
        get => _isPrefabPresent;
        set => _isPrefabPresent = value;  
    }    
}
