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

        HoodOrnament.ActivateOrnament(pickup);
        if (pickup)
        {
            ScoreScript.countdown = true;
            ScoreScript.theScore += 1000;
            Destroy(transform.parent.gameObject);
        }
        else {
            ScoreScript.countdown = false;
            Destroy(gameObject);
        }
    }
}
