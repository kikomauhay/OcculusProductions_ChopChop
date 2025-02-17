 using UnityEngine;

public class Plate : Equipment
{
    public bool IsDirty => _isDirty;
    
    [SerializeField] Material _dirtyPlateMat, _cleanPlateMat;
    [SerializeField] Collider _cleanTrigger;
    [SerializeField] bool _isDirty;

    protected override void Start()
    {
        base.Start();

        name = "Plate";
        _isDirty = false;
        _cleanTrigger.enabled = false;

        GetComponent<MeshRenderer>().material = _cleanPlateMat;
    }

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Space)) 
            TogglePlateSanitation(); // test

        if (_cleanTrigger == null) return;

        DishWash();
    }

    private void OnTriggerStay(Collider other)
    {
        Sponge sponge = other.GetComponent<Sponge>();
        Rigidbody _spongeRb = sponge.GetComponent<Rigidbody>();
        //Create a formula that tracks the change in velocity and compares it to a certain value
        //If velocity is greater than assigned value, start coroutine for cleaning
        //suggestion ko lang coroutine but if there's a more efficient way in cleaning the plate please don't hesitate to try it out
        //If you do go the coroutine route, I think you'd need to put the checker to avoid having the coroutine run again and again.       
    }

    private void DishWash()
    {
        if(_isDirty)
            _cleanTrigger.enabled = true;

        else _cleanTrigger.enabled = false;
    }
    #region Public Functions
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
    #endregion
}
