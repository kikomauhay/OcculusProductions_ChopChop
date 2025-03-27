using System.Collections;
using UnityEngine;

public class Plate : Equipment
{
#region Members

    public bool IsPlated { get; private set; }

    [Tooltip("The Box Collider Component")] 
    [SerializeField] Collider _boxTrigger;

#endregion

#region Unity_Methods

    protected override void Start()
    {
        base.Start();

        name = "Plate";
        _maxUsageCounter = 1;

        IsPlated = false;
        _boxTrigger.enabled = true;
    }
    void OnTriggerEnter(Collider other)
    {
        /*if (IsPlated) return;

        // plating logic
        if (other.gameObject.GetComponent<Food>() != null)
        {
            Food food = other.gameObject.GetComponent<Food>();

            Destroy(other.gameObject);
            Destroy(gameObject);
            StartCoroutine(AttachToPlate(food));
            return;
        }

        if (other.gameObject.GetComponent<Sponge>() != null)
        {
            
        }
            */

        if (other.gameObject.GetComponent<Food>() != null && !IsPlated)
        {
            Destroy(gameObject);
            Destroy(other.gameObject);

            StartCoroutine(AttachToPlate(other.GetComponent<Food>()));
        }

        if (other.gameObject.GetComponent<Sponge>().IsWet && !IsClean)
        {
            SpawnManager.Instance.SpawnVFX(VFXType.BUBBLE, transform, 3f);
            
            StopCoroutine(DoCleaning());
            StartCoroutine(DoCleaning());
            DishWash();
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.GetComponent<Sponge>() != null) 
            StopCoroutine(DoCleaning());
    }

    IEnumerator DoCleaning()
    {
        yield return new WaitForSeconds(2f);

        if (!IsClean)
            ToggleClean();
    }
    IEnumerator AttachToPlate(Food food)
    {
        TogglePlated();
        yield return new WaitForSeconds(1.5f);
        food.CreateDish(transform);
    }

#endregion

    public void TogglePlated() => IsPlated = !IsPlated;
    private void DishWash()
    {
        if (_boxTrigger == null) return;

        if (!IsClean && !IsPlated)
            _boxTrigger.enabled = true;

        else
            _boxTrigger.enabled = false;
    }
}
