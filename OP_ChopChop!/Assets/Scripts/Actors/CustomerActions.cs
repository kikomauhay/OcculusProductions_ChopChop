using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class CustomerActions : MonoBehaviour
{
    [SerializeField] private float customerSpeed;
    [SerializeField] private Transform[] targetLocation;

    void Update() => CustomerMoveToSeat();

    private void CustomerMoveToSeat()
    {
        customerSpeed = customerSpeed * Time.deltaTime;

        //this.transform.position = Vector3.MoveTowards(this.transform.position, targetLocation[0].transform.position, customerSpeed);
    }
}
