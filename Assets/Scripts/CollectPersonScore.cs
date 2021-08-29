using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectPersonScore : MonoBehaviour
{


    void OnTriggerEnter(Collider other) {


        if (other.gameObject.name == "car") {

            ScoreScript.theScore += 1000;
            Destroy(gameObject);
        
        }



    }



}
