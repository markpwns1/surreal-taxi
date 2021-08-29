using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ScoreScript : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject scoreText;
    public static float theScore;
    public static int stunts;
    public static bool countdown=false;

    void Update() {
        scoreText.GetComponent<Text>().text = "CAPITAL GENERATED: " + Mathf.Round(theScore) + "$ \nSTUNTS PERFORMED: " + stunts;
    }


}
