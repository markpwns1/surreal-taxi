using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SentencesSource", menuName = "Sentences Source", order = 1)]
public class SentencesSource : ScriptableObject
{
    [TextArea(3, 50)]
    public string source;
}
