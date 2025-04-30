using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// 
/// - Acts as the reworked version of ColliderCheck.cs
/// 
/// </summary> 

[RequireComponent(typeof(BoxCollider))]
public class NEW_ColliderCheck : MonoBehaviour 
{
#region Properties

    public CustomerOrder Order { get; set; }

#endregion

    private Collider _collider;

#region Unity

    private void Awake()
    {
        _collider = GetComponent<BoxCollider>();
    }
    private void Start() 
    {
        _collider.enabled = true;    
        _collider.isTrigger = true;
    }

    private void OnTriggerEnter(Collider other) 
    {
        if (Order == null)
            throw new ArgumentNullException("Order is null", nameof(Order));

        Ingredient ing = other.gameObject.GetComponent<Ingredient>();
        NEW_Dish dish = other.gameObject.GetComponent<NEW_Dish>();

        // player serves AN INGREDIENT
        if (ing != null)
        {
            DoIngredientCollision();
            StartCoroutine(DisableCollider());
            return;
        }
        
        // player serves A DISH
        if (dish != null) 
        {
            DoDishCollision();
            StartCoroutine(DisableCollider());
        }


    }

#region Enumerators

    private IEnumerator DisableCollider()
    {
        _collider.enabled = false;
        yield return new WaitForSeconds(3f);
        _collider.enabled = true;
    }
#endregion

#endregion

#region Helpers

    private void DoIngredientCollision() {}
    private void DoDishCollision() {}

#endregion

}
