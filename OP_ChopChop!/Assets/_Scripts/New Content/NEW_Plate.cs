using UnityEngine;

/// <summary>
/// 
/// - Acts as the reworked version of Plate.cs
/// 
/// </summary>

[RequireComponent(typeof(NEW_Dish))]
public class NEW_Plate : Equipment
{

    private NEW_Dish _dish;

#region Unity

    protected override void Awake()
    {
        base.Awake();
        _dish = GetComponent<NEW_Dish>();

        if (_isDeveloperMode)
            Debug.Log($"{this} developer mode: {_isDeveloperMode}");
    }
    protected override void OnTriggerEnter(Collider other)
    {
        if (_dish.HasFood)
        {
            // Debug.LogError($"{name} already contains food!");
            return;
        }

        if (!IsClean)
            base.OnTriggerEnter(other);
    }

#region Testing

    protected override void Update() => Test();
    protected override void Test()
    {
        base.Test();

        if (Input.GetKeyDown(KeyCode.C) && _isDeveloperMode)
        {
            _usageCounter = 0;
            _isClean = true;
            _rend.materials = new Material[] { _cleanMat };
        }
    }

#endregion
#endregion

#region Override

    public override void HitTheGround()
    {
/*        if (_dish.HasFood)
        {
            _dish.SetFoodCondition(FoodCondition.MOLDY);
            // Debug.LogWarning("The food got moldy!");
        }*/

        /*        SoundManager.Instance.PlaySound(Random.value > 0.5f ?
                                                "plate placed 01" :
                                                "plate placed 02");*/
        base.HitTheGround();
    }
    public override void Trashed()
    {
        if (_dish.HasFood)
        {
            _dish.DisableDish();
            SoundManager.Instance.PlaySound("dispose food");
            // Debug.LogWarning("Food on the plate has been removed!");
        }

        base.Trashed();
    }
    public void Served()
    {
        IncrementUseCounter();
        _dish.DisableDish();
    }
    public override void PickUpEquipment()
    {
        string soundName = string.Empty;

        switch (Random.Range(0, 3))
        {
            case 0: soundName = "plate grabbed 01"; break;
            case 1: soundName = "plate grabbed 02"; break;
            case 2: soundName = "plate grabbed 03"; break;
            default: break;
        }

        SoundManager.Instance.PlaySound(soundName);
    }

#endregion
}