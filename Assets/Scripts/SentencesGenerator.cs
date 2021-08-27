using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MarkovSharp;
using MarkovSharp.TokenisationStrategies;
using System.Linq;

public class SentencesGenerator : MonoBehaviour
{
    public List<SentencesSource> references;
    public StringMarkov model;
    // Start is called before the first frame update
    void Start()
    {
        // Create a new model
        model = new StringMarkov(1);

        foreach(SentencesSource source in references)
        {
            model.Learn(source.source);
        }
    }

    public string GenerateLines()
    {
        return model.Walk().First();
    }
}
