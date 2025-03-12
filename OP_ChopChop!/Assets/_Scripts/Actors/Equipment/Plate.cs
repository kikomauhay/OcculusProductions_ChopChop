using System.Collections;
using UnityEngine;

public class Plate : Equipment
{
#region Members

    public bool IsPlated { get; private set; }

    [Tooltip("The Box Collider Component")] 
    [SerializeField] Collider _cleanTrigger;

#endregion    

    protected override void Start()
    {
        base.Start();

        name = "Plate";
        _maxUsageCounter = 1;

        IsPlated = false;
        _cleanTrigger.enabled = false;
    }

    void Update() => test();
    

    void test()
    {
        if (Input.GetKeyUp(KeyCode.Space)) 
            ToggleClean(); // test

    }

    void OnTriggerEnter(Collider other)
    {
        Sponge sponge = other.gameObject.GetComponent<Sponge>();


        // snap the Food to the plate instead
        
        
        if (other.GetComponent<Food>() != null && !IsPlated)
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

    void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<Sponge>() != null)
        {
            StopAllCoroutines();
        }
    }

    public void TogglePlated() => IsPlated = !IsPlated;

    private void DishWash()
    {
        if (_cleanTrigger == null) return;

        if (!IsClean && !IsPlated)
            _cleanTrigger.enabled = true;

        else
            _cleanTrigger.enabled = false;
    }


    IEnumerator CleaningPlate()
    {
        yield return new WaitForSeconds(2f);
        ToggleClean ();
    }
}
