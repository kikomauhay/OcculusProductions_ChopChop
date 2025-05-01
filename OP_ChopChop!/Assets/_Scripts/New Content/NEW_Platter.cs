using System;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

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

        if (_materials.Length != 3)
            Debug.LogWarning($"{_materials} has missing materials!");

        ResetMaterial();

        Debug.Log($"Developer mode: {_isDevloperMode}");
    }

#region Testing

    private void Update() 
    {
        test();
    }
    private void test()
    {
        if (!_isDevloperMode) 
        {
            Debug.LogError("Not in developer mode!");            
            return;
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            ResetMaterial();
            Debug.LogWarning("Plate is clean!");
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            SetRotten();
            Debug.LogWarning("Plate is rotten!");
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            SetMoldy();
            Debug.LogWarning("Plate is moldy!");
        }
    }

#endregion

#region Public

    // just makes the material clean again
    public void ResetMaterial() => 
        _rend.materials = new Material[] { _materials[0] };

    public void SetRotten() => 
        _rend.materials = new Material[] { _materials[1], _dirtyOSM };
        
    public void SetMoldy() => 
        _rend.materials = new Material[] { _materials[2], _dirtyOSM };

#endregion
}
