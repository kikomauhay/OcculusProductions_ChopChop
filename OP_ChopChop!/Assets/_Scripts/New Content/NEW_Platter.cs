using UnityEngine;

/// <summary>
/// 
/// - Used for the child GameObjects in the reworked dish
/// 
/// WHAT THIS SCRIPT SHOULD DO:
///     - Changes the material of the food bases on the dish's state.
///     - 
/// 
/// </summary>

public class NEW_Platter : MonoBehaviour
{
#region Members

    [Tooltip("0 = clean, 1 = Rotten, 2 = Moldy")]
    [SerializeField] private Material[] _materials;
    [SerializeField] private Material _dirtyOSM;
    [SerializeField] private bool _isDevloperMode;

    private Renderer _rend;

#endregion 
    
    private void Awake()
    {
        _rend = GetComponent<Renderer>();

        if (_materials.Length < 3)
            Debug.LogWarning($"{_materials} has missing materials!");

        Debug.Log($"{name} developer mode: {_isDevloperMode}");
    }

#region Testing

    private void Update() 
    {
        test();
    }
    private void test()
    {
        if (Input.GetKeyDown(KeyCode.Q) && _isDevloperMode) ResetMaterial();
        if (Input.GetKeyDown(KeyCode.W) && _isDevloperMode) SetRotten();
        if (Input.GetKeyDown(KeyCode.E) && _isDevloperMode) SetMoldy();
    }
#endregion

#region Public

    public void ResetMaterial() // just makes the material clean again
    {
        _rend.materials = new Material[] { _materials[0] };
        Debug.LogWarning($"{name} is clean!");
    }
    public void SetRotten()
    {
        _rend.materials = new Material[] { _materials[1], _dirtyOSM };
        Debug.LogWarning($"{name} is rotten!");
    }        
    public void SetMoldy() 
    {
        _rend.materials = new Material[] { _materials[2], _dirtyOSM };
        Debug.LogWarning($"{name} is moldy!");
    }

#endregion
}
