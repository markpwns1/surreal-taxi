using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueHandler : MonoBehaviour
{
    public SentencesGenerator generator;

    public float defaultPrintDelay = 3f;
    public int maxDisplay = 5;

    private Text dialogue;
    private List<string> buffer;
    private List<string> display;

    private string currentlyPrinting;
    private string currentlyPrinted;
    private float nextPrintTime;
    private float printDelay;

    public void Start()
    {
        dialogue = gameObject.GetComponent<Text>();
        buffer = new List<string>();
        display = new List<string>();
        currentlyPrinted = null;
        currentlyPrinting = null;
        printDelay = defaultPrintDelay / 60f;

        nextPrintTime = Time.time;

        StartDialogue();
    }

    public void StartDialogue()
    {
        string generated = generator.GenerateLines();
        string[] sentences = generated.Split('.');
        foreach (string sentence in sentences)
        {
            FeedLine(sentence.Trim());
        }
        print(generated);
    }

    public void FeedLine(string line)
    {
        buffer.Add(line);
    }

    public void Update()
    {
        if (buffer.Count > 0 && currentlyPrinted == null)
        {
            currentlyPrinted = "";
            currentlyPrinting = buffer[0];
            buffer.RemoveAt(0);
        }
        else if (currentlyPrinting != null && Time.time > nextPrintTime)
        {
            if (currentlyPrinting.Length > 1)
            {
                currentlyPrinted += currentlyPrinting[0];
                currentlyPrinting = currentlyPrinting.Substring(1);
            }
            else
            {
                currentlyPrinted += currentlyPrinting;
                currentlyPrinting = null;
            }
            nextPrintTime = Time.time + printDelay;

            if (currentlyPrinting == null)
            {
                if (display.Count > maxDisplay)
                {
                    display.RemoveAt(0);
                }
                display.Add(currentlyPrinted);
                currentlyPrinted = null;
            }
        }

        dialogue.text = "";

        foreach (string displayed in display)
        {
            dialogue.text += displayed + "\n";
        }
        dialogue.text += currentlyPrinted;
    }
}