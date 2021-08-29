using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MarkovSharp;
using MarkovSharp.TokenisationStrategies;
using System.Linq;

public class SentencesGenerator : MonoBehaviour
{
    public List<SentencesSource> references;

    public string GenerateLines()
    {
        var model = new StringMarkov(2);

        foreach (SentencesSource source in references)
        {
            model.Learn(source.source);
        }

        return model.Walk().First().ToUpper();
    }
}
