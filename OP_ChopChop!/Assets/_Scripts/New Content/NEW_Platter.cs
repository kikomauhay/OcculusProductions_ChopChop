using UnityEngine;

/// <summary>
/// 
/// - used for the child GameObjects in the dish
/// - mainly used to change their material
/// 
/// </summary>

public class NEW_Platter : MonoBehaviour
{
#region Private

    [Tooltip("0 = clean, 1 = Rotten, 2 = Moldy")]
    [SerializeField] private Material[] _materials;
    [SerializeField] private Material _dirtyOSM;

    private Renderer _rend;

#endregion

#region Unity    
    
    private void Awake()
    {
        _rend = GetComponent<Renderer>();

        if (_materials.Length != 3)
            Debug.LogWarning($"{_materials} has missing materials!");

        ResetMaterial();
    }

#endregion

#region Public

    public void SetRotten() =>
        _rend.materials = new Material[] { _materials[1], _dirtyOSM };

    public void SetMoldy() => 
        _rend.materials = new Material[] { _materials[2], _dirtyOSM };

    public void ResetMaterial() => 
        _rend.material = _materials[1];

#endregion

}
