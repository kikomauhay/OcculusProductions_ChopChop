using UnityEditor.Experimental.GraphView;
using UnityEngine;

/// <summary>
/// 
/// - Acts as the reworked version of Plate.cs
/// 
/// </summary>

[RequireComponent(typeof(NEW_Dish))]
public class NEW_Plate : Equipment
{
#region Properties


#endregion

#region Private

    private NEW_Dish _dish;
    
#endregion

#region Unity

    private void Awake()
    {
        _dish = GetComponent<NEW_Dish>();
    }

    protected override void OnTriggerEnter(Collider other)
    {
        if (!_dish.IsPlated)
        {
            Debug.LogError($"Dish cleaning only happens when {gameObject.name} has no food!");
            return;
        }

        base.OnTriggerEnter(other);
    }

#endregion

#region Public

    public override void HitTheGround()
    {
        base.HitTheGround();

        if (_dish.IsPlated)
            _dish.SetState(DishState.MOLDY);
    }

    #endregion

#region Protected


#endregion
}
