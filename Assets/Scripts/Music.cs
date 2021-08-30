using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Music : MonoBehaviour
{
    public AudioSource alwaysMusic, groundOnlyAudio;
    public float lerp, airTimeThreshold;
    [Range(0, 1)]
    public float minVolume;

    private float time;
    private bool timeSet;

    void Update()
    {
        float goal = 1;
        if (!CarController.ground)
        {
            if (!timeSet) time = Time.deltaTime;
            if (Time.time > time + airTimeThreshold) goal = minVolume;
        }
        else timeSet = false;
        groundOnlyAudio.volume = Mathf.Lerp(groundOnlyAudio.volume, goal, lerp);
    }
}
