using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestingGiveMeWordsButton : MonoBehaviour
{
    public Text text;
    public SentencesGenerator generator;

    public void GetSentences()
    {
        string lines = generator.GenerateLines();
        print(lines);
        text.text = lines;
    }
}
