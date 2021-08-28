using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class otherScript : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject scoreText;
    public int theScore;

    void OnTriggerEnter(Collider other) {

        if (other.gameObject.name == "car")
        {
            theScore = 1000;
            scoreText.GetComponent<Text>().text = "Salary: " + theScore+ "$";
            Destroy(gameObject);

        }
        //Time.deltaTime //variable

        //

      
    }


}
