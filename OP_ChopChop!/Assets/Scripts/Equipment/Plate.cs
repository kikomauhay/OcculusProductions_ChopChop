using UnityEditor.XR.Management;
using UnityEngine;

public class Plate : MonoBehaviour
{
    public bool IsDirty => _isDirty;
    
    [SerializeField] Material _dirtyPlateMat, _cleanPlateMat;
    [SerializeField] bool _isDirty;

    void Start()
    {
        _isDirty = false;
        GetComponent<MeshRenderer>().material = _cleanPlateMat;
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Space)) 
            TogglePlateSanitation();
    }

    public void TogglePlateSanitation()
    {
        _isDirty = !_isDirty;

        if (_isDirty)
            SetContaminated();
        
        else SetCleaned();
    }
    public void SetContaminated()
    {
        _isDirty = true;
        Debug.Log("Contaminated!");
        GetComponent<MeshRenderer>().material = _dirtyPlateMat;
    }

    public void SetCleaned()
    {
        _isDirty = false;
        Debug.Log("Cleaned!");
        GetComponent<MeshRenderer>().material = _cleanPlateMat;
    }
}
