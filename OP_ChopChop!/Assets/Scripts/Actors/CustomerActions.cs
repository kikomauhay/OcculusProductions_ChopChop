using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerActions : MonoBehaviour
{
    [SerializeField] private float customerSpeed;
    [SerializeField] private Transform[] targetLocation;

    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        CustomerMoveToSeat();
    }

    private void CustomerMoveToSeat()
    {
        customerSpeed = customerSpeed * Time.deltaTime;

        this.transform.position = Vector3.MoveTowards(this.transform.position, targetLocation[0].transform.position, customerSpeed);
    }
}
