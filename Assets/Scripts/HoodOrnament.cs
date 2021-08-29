using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoodOrnament : MonoBehaviour
{
    private static GameObject instance;
    private static AudioSource sfx;

    private void Start()
    {
        instance = gameObject;
        sfx = GetComponent<AudioSource>();
        instance.SetActive(false);
    }

    public static void ActivateOrnament(bool b)
    {
        instance.SetActive(b);
        sfx.Play();
    }
}
