using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Music : MonoBehaviour
{
    public AudioSource alwaysMusic, groundOnlyAudio;
    public float lerp;

    void Update()
    {
        float goal = 0;
        if (CarController.ground) goal = 1;
        groundOnlyAudio.volume = Mathf.Lerp(groundOnlyAudio.volume, goal, lerp);
    }
}
