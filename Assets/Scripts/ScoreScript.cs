using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ScoreScript : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject scoreText;
    public static float theScore;
    public static bool countdown=false;

   

    void Update() {


       scoreText.GetComponent<Text>().text = "Salary: " + Mathf.Round(theScore) + "$";
        if (countdown)
        {
            theScore -= Time.deltaTime;

        }
    }


}
