using System.Collections;
using UnityEngine;
using System;

public class Plate : Equipment
{
    public bool IsDirty => _isDirty;
    
    [SerializeField] Material _dirtyPlateMat, _cleanPlateMat;
    [SerializeField] Collider _cleanTrigger;
    [SerializeField] bool _isDirty,_isPlated;

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
    #region OnTriggerSht
    private void OnTriggerEnter(Collider other)
    {
        Sponge sponge = other.gameObject.GetComponent<Sponge>();
        
        if (other.GetComponent<Food>() != null && !_isPlated)
        {
            Destroy(gameObject);
            Destroy(other.gameObject);
            TogglePlated();
            other.GetComponent<Food>().CreateDish(transform);
        }

        
        // if (sponge.IsWet)
        // {
        //     SetCleaned();
        //     Debug.Log("Cleaning Plate");
        // }
    }

    /*
    private void OnTriggerStay(Collider other)
    {
        Sponge sponge = other.gameObject.GetComponent<Sponge>();
        Rigidbody _spongeRb = sponge.gameObject.GetComponent<Rigidbody>();
        Vector3 _lastVelocity = _spongeRb.velocity;
        float dif = Mathf.Abs((_spongeRb.velocity - _lastVelocity).magnitude / Time.fixedDeltaTime);

        if (sponge == null) return;

    }
    */

    private void OnTriggerExit(Collider other)
    {
        if(other.GetComponent<Sponge>() != null)
        {
            StopAllCoroutines();
        }
    }
    #endregion

    private void DishWash()
    {
        if(_isDirty || !_isPlated)
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
    public void TogglePlated()
    {
        _isPlated = !_isPlated;
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

    IEnumerator CleaningPlate()
    {
        SetCleaned();
        yield return new WaitForSeconds(2f);
    }
}
