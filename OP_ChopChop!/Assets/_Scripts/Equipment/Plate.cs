using UnityEngine;

public class Plate : Equipment
{
    public bool IsDirty => _isDirty;
    
    [SerializeField] Material _dirtyPlateMat, _cleanPlateMat;
    [SerializeField] bool _isDirty;

    protected override void Start()
    {
        base.Start();

        name = "Plate";
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
        GetComponent<MeshRenderer>().material = _dirtyPlateMat;
    }
    public void SetCleaned()
    {
        _isDirty = false;
        GetComponent<MeshRenderer>().material = _cleanPlateMat;
    }
}
