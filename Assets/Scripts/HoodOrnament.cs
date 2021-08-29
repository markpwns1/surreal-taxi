using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoodOrnament : MonoBehaviour
{
    private static GameObject instance;
    private static AudioSource sfx;

    private static AudioSource[] talkSounds;
    private static AudioSource currentBabble;

    public GameObject talkSoundsParent;

    private void Start()
    {
        instance = gameObject;
        sfx = GetComponent<AudioSource>();
        instance.SetActive(false);
        talkSounds = talkSoundsParent.GetComponentsInChildren<AudioSource>();
        currentBabble = talkSounds[0];
    }

    private void Update()
    {
        RunBabble();
    }

    public static void ActivateOrnament(bool b)
    {
        instance.SetActive(b);
        if (b)
        {
            sfx.Play();
        }
    }

    private static void RunBabble()
    {
        bool talking = DialogueHandler.isTalking();

        if (!talking)
            foreach (AudioSource s in talkSounds)
                s.Stop();
        else
        {
            if (!currentBabble.isPlaying)
            {
                currentBabble = talkSounds[Random.Range(0, talkSounds.Length)];
                currentBabble.Play();
            }
        }
    }
}
