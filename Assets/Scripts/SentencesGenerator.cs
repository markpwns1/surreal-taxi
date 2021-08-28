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

    public string GenerateLines()
    {
        model = new StringMarkov(2);

        foreach (SentencesSource source in references)
        {
            model.Learn(source.source);
        }

        return model.Walk().First();
    }
}
