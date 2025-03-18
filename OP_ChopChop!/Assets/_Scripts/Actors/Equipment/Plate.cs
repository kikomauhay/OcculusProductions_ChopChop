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
        if (other.GetComponent<Food>() != null && !IsPlated)
        {
            Destroy(gameObject);
            Destroy(other.gameObject);

            StartCoroutine(PlateTheFood(other.GetComponent<Food>()));
        }

        if (other.GetComponent<Sponge>().IsWet && !IsClean)
        {
            StartCoroutine(DoCleaning());
            DishWash();
        }
    }
    IEnumerator DoCleaning()
    {
        yield return new WaitForSeconds(2f);
        ToggleClean();
    }

    IEnumerator PlateTheFood(Food food)
    {
        TogglePlated();
        yield return null;
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

#region Testing

    void test()
    {
        if (Input.GetKeyUp(KeyCode.Space))
            ToggleClean(); // test
    }

#endregion
}
