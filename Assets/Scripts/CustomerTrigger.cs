using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerTrigger : MonoBehaviour
{
    public bool pickup = true;
    CustomerManager cm;
    void Start()
    {
        cm = GameObject.FindObjectOfType<CustomerManager>();
    }

    void OnTriggerEnter(Collider col)
    {
        if (pickup) cm.CustomerPickedUp();
        else cm.CustomerDroppedOff();

        Destroy(gameObject);
    }
}
