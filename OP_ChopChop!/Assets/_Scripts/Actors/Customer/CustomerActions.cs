using UnityEngine;

/// <summary>
/// 
/// Anything that needs to move about the customer goes here
/// 
/// </summary>

public class CustomerActions : MonoBehaviour
{
    [SerializeField] private float _customerSpeed;
    [SerializeField] private Transform[] _targetLocations;

    void Start() => _customerSpeed *= Time.deltaTime;
    void LateUpdate() => transform.position = Vector3.MoveTowards(transform.position, 
                                                                  _targetLocations[0].position, 
                                                                  _customerSpeed);
}
