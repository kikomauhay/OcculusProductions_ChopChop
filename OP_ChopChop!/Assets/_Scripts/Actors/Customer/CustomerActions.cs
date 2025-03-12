using System.Collections;
using UnityEngine;

public class CustomerActions : MonoBehaviour
{
    public Vector3 TargetSeat { get; set; }
    public int SeatIndex { get; set; } 

    float _customerSpeed = 2f;

    // void Start() => StartCoroutine(DeleteCustomer());

    void LateUpdate() => transform.position = Vector3.MoveTowards(transform.position, 
                                                                  TargetSeat, 
                                                                  _customerSpeed * Time.deltaTime);


    IEnumerator DeleteCustomer()
    {
        yield return new WaitForSecondsRealtime(5f);

        SpawnManager.Instance.RemoveCustomer(gameObject);
        Destroy(gameObject);
    }
}
