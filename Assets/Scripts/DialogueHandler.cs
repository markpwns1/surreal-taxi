using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class DialogueHandler : MonoBehaviour
{
    public SentencesGenerator generator;
    public Image profile;
    public GameObject bg;

    public float defaultPrintDelay = 3f;
    public int maxDisplay = 5;

    private Text dialogue;
    private List<string> buffer;
    private List<string> display;

    private string currentlyPrinting;
    private string currentlyPrinted;
    private float nextPrintTime;
    private float printDelay;

    public static bool talking = false;

    private void Start()
    {
        Reset();
    }

    public void Reset()
    {
        bg.SetActive(false);
        talking = false;
        profile.enabled = false;
        dialogue = gameObject.GetComponent<Text>();
        buffer = new List<string>();
        display = new List<string>();
        currentlyPrinted = null;
        currentlyPrinting = null;
        printDelay = defaultPrintDelay / 60f;

        nextPrintTime = Time.time;
    }

    public void StartDialogue()
    {
        Reset();

        bg.SetActive(true);
        talking = true;
        profile.enabled = true;
        string generated = generator.GenerateLines();
        //string[] sentences = generated.Split('\n');
        //foreach (string sentence in sentences)
        //{
        //    FeedLine(sentence.Trim());
        //}
        var sentences = generated.Split('.', '!', '?');
        FeedLine(sentences[0] + ".");
        FeedLine(sentences[1] + ".");
        FeedLine("[LEFT CLICK TO END DIALOGUE]");
        //print(generated);
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
                if (currentlyPrinted.Length > 2)
                {
                    if (display.Count > maxDisplay && dialogue.text.Length > 320)
                    {
                        display.RemoveAt(0);
                    }
                    display.Add(currentlyPrinted);
                }
                currentlyPrinted = null;
            }
        }
        else if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            Reset();
        }

        dialogue.text = "";

        foreach (string displayed in display)
        {
            dialogue.text += displayed.Trim() + "\n";
        }
        dialogue.text += currentlyPrinted;
    }

    public static bool isTalking()
    {
        return talking;
    }
}
