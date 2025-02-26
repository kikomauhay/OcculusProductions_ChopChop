using UnityEngine;

public class CustomerSeat : MonoBehaviour
{
    [SerializeField] private bool _hasCustomer;

    public bool HasCustomer
    {
        get => _hasCustomer;
        set => _hasCustomer = value;  
    }    
}
