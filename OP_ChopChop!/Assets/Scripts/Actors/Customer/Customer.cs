using UnityEngine;

// uncomment once AJ's done adding these scripts
// [RequireComponent(typeof(CustomerOrder), typeof(CustomerAction))]

public enum CustomerVariant { PENGUIN, PIG, CAT }

public class Customer : MonoBehaviour 
{
    [Header("Customer Appearance")]
    [SerializeField] GameObject _body, _face;
    [SerializeField] GameObject[] _customerFaces;


    [Header("Customer Stats")]
    [SerializeField] CustomerVariant _customerType;
    public CustomerVariant CustomerType => _customerType;


    void Start() 
    {
        // customer randomizer
        _customerType = (CustomerVariant)Random.Range(0, 3);

        switch (_customerType) 
        {
            case CustomerVariant.PENGUIN: _face = _customerFaces[0]; break;
            case CustomerVariant.PIG:     _face = _customerFaces[1]; break;
            case CustomerVariant.CAT:     _face = _customerFaces[2]; break;
            default: break;
        }
    }
}
