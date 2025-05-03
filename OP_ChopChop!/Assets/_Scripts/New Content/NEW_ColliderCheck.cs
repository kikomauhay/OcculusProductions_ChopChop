using System.Collections;
using UnityEngine;

/// <summary>
/// 
/// - Acts as the reworked version of ColliderCheck.cs
/// 
/// WHAT THIS SCRIPT SHOULD DO: 
///     - Connects a Customer Oorder and the Dish being served
///     - Disable the dish once it's served to the customer
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
        if (Order == null) return;
    
        Ingredient ing = other.gameObject.GetComponent<Ingredient>();
        NEW_Dish dish = other.gameObject.GetComponent<NEW_Dish>();

        // player serves AN INGREDIENT
        if (ing != null)
            DoIngredientCollision();

        // player serves A DISH
        if (dish != null)
            DoDishCollision(dish);

        StartCoroutine(DisableCollider());
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
    private void DoDishCollision(NEW_Dish dish) 
    {
        
    }

#endregion

}
