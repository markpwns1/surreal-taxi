﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ScoreScript : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject scoreText;
    public static float theScore;
   

    void Update() {


        scoreText.GetComponent<Text>().text = "Salary: " + theScore + "$";
        theScore -= Time.deltaTime;

    }


}
