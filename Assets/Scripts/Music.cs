using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Music : MonoBehaviour
{
    public AudioSource alwaysMusic, groundOnlyAudio;
    public float lerp, airTimeThreshold, minVelocity = 5;
    [Range(0, 1)]
    public float minAirVolume, minStopVolume;

    private float time;
    private bool timeSet;

    private float highGoal, lowGoal;
    private float stopSoundLerp = 0;

    private Rigidbody rb;
    private AudioHighPassFilter highPass;
    private AudioLowPassFilter lowPass;

    private void Start()
    {
        GetRigidBodyReference();
        highPass = GetComponent<AudioHighPassFilter>();
        highGoal = highPass.highpassResonanceQ;
        highPass.enabled = false;
        lowPass = GetComponent<AudioLowPassFilter>();
        lowGoal = lowPass.lowpassResonanceQ;
        highPass.enabled = false;
    }

    void Update()
    {
        float goal = 1;
        if (!CarController.ground)
        {
            if (!timeSet) time = Time.deltaTime;
            if (Time.time > time + airTimeThreshold) goal = minAirVolume;
        }
        else timeSet = false;
        groundOnlyAudio.volume = Mathf.Lerp(groundOnlyAudio.volume, goal, lerp);

        goal = 1;
        if (rb)
        {
            if (rb.velocity.magnitude < minVelocity)
            {
                highPass.enabled = lowPass.enabled = true;
                stopSoundLerp = Mathf.Lerp(stopSoundLerp, 1, lerp);
                highPass.highpassResonanceQ = stopSoundLerp * highGoal;
                lowPass.lowpassResonanceQ = stopSoundLerp * lowGoal;
                goal = minStopVolume;
            }
            else
            {
                stopSoundLerp = Mathf.Lerp(stopSoundLerp, 0, lerp);
                if (stopSoundLerp < 0.1f)
                    highPass.enabled = lowPass.enabled = false;

                highPass.highpassResonanceQ = stopSoundLerp * highGoal;
                lowPass.lowpassResonanceQ = stopSoundLerp * lowGoal;
            }
        }
        else GetRigidBodyReference();
        alwaysMusic.volume = Mathf.Lerp(alwaysMusic.volume, goal, lerp);
    }

    private void GetRigidBodyReference()
    {
        CarController c = FindObjectOfType<CarController>();
        if (c)
            rb = c.GetComponentInChildren<Rigidbody>();
    }
}
