using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoodOrnament : MonoBehaviour
{
    private static GameObject instance;
    private static AudioSource sfx;

    private static AudioSource[] talkSounds;

    public GameObject talkSoundsParent;

    private void Start()
    {
        instance = gameObject;
        sfx = GetComponent<AudioSource>();
        instance.SetActive(false);
        talkSounds = talkSoundsParent.GetComponentsInChildren<AudioSource>();
    }

    public static void ActivateOrnament(bool b)
    {
        instance.SetActive(b);
        if (b)
        {
            sfx.Play();
            talkSounds[Random.Range(0, talkSounds.Length)].Play();
        }
    }
}
