using UnityEngine;

public enum CustomerVariant { PENGUIN, PIG, CAT }

[RequireComponent(typeof(CustomerOrder), typeof(CustomerActions))]
public class Customer : MonoBehaviour 
{
#region Members

    [Header("Customer Appearance")]
    [SerializeField] GameObject _body, _face;
    [SerializeField] GameObject[] _customerFaces; // it's important that they're in a certain order
    public CustomerVariant CustomerType => _customerType;


    [Header("Customer Stats")]
    [SerializeField] CustomerVariant _customerType;
    [SerializeField] CustomerOrder _order;
    [SerializeField] CustomerActions _action;

#endregion

#region Methods

    void Awake() => _customerType = (CustomerVariant)Random.Range(0, 3);
    void Start() 
    {
        switch (_customerType) 
        {
            case CustomerVariant.PENGUIN: _face = _customerFaces[0]; break;
            case CustomerVariant.PIG:     _face = _customerFaces[1]; break;
            case CustomerVariant.CAT:     _face = _customerFaces[2]; break;
            default: break;
        }
    }

#endregion
}
