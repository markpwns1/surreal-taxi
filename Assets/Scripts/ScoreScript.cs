using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ScoreScript : MonoBehaviour
{
    // Start is called before the first frame update

    public Image background;
    public float payoutDecayFactor = 1;
    public float bigBackgroundExtraHeight = 8;

    private Text scoreText;
    private Vector2 backgroundSize;

    public static float theScore, payout;
    public static int stunts;
    public static bool countdown=false;

    private void Start()
    {
        backgroundSize = background.rectTransform.sizeDelta;
        scoreText = GetComponent<Text>();
    }

    void Update() {
        string s = "CAPITAL GENERATED: " + theScore.ToString("c2") +
        "$ \nSTUNTS PERFORMED: " + stunts;
        if (payout > 0)
        {
            background.rectTransform.sizeDelta =
                backgroundSize + Vector2.up * bigBackgroundExtraHeight;
            s += "\nPAYOUT: " + payout.ToString("c2") + "$";
            payout -= Time.deltaTime * payoutDecayFactor;
        }
        else
        {
            payout = 0;
            background.rectTransform.sizeDelta = backgroundSize;
        }

        scoreText.text = s;
    }

    public static void Payout()
    {
        theScore += payout;
        payout = 0;
    }

    public static void OnStunt(float pay)
    {
        stunts += 1;
        if (countdown)
            payout += pay;
    }
}
