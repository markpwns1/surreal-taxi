using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerTrigger : MonoBehaviour
{
    public bool pickup = true;
    public float maxPayout = 300, minPayout = 100;
    CustomerManager cm;
    DialogueHandler dh;
    void Start()
    {
        cm = GameObject.FindObjectOfType<CustomerManager>();
        dh = GameObject.FindObjectOfType<DialogueHandler>();
    }

    void OnTriggerEnter(Collider col)
    {
        if (pickup) cm.CustomerPickedUp();
        else cm.CustomerDroppedOff();

        HoodOrnament.ActivateOrnament(pickup);
        if (pickup)
        {
            ScoreScript.countdown = true;
            Destroy(transform.parent.gameObject);
            if(!dh) dh = FindObjectOfType<DialogueHandler>();
            dh.StartDialogue();
            ScoreScript.payout = Random.Range(minPayout, maxPayout);
        }
        else {
            ScoreScript.countdown = false;
            Destroy(gameObject);
            if (!dh) dh = FindObjectOfType<DialogueHandler>();
            dh.Reset();
            //ScoreScript.theScore += Random.Range(600, 1000);
            ScoreScript.Payout();
        }
    }
}
