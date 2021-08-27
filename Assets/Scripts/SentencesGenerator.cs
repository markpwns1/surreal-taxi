using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MarkovSharp;
using MarkovSharp.TokenisationStrategies;
using System.Linq;

public class SentencesGenerator : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        // Some training data
        var lines = new string[]
        {
        "Frankly, my dear, I don't give a damn.",
        "Mama always said life was like a box of chocolates. You never know what you're gonna get.",
        "Many wealthy people are little more than janitors of their possessions."
        };

        // Create a new model
        var model = new StringMarkov(1);

        // Train the model
        model.Learn(lines);

        print(model.Walk().First());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
