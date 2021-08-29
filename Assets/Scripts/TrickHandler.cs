using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TrickHandler : MonoBehaviour
{
    public enum TrickMove
    {
        NONE,
        UP,
        DOWN,
        LEFT,
        RIGHT
    }

    public float trickDuration;

    private TrickMove currentTrick;
    private float nextTrickEnd;
    private bool onAir = false;

    public void StartTricks()
    {
        currentTrick = TrickMove.NONE;
        nextTrickEnd = Time.time;
        onAir = true;
    }

    public void EndTricks()
    {
        currentTrick = TrickMove.NONE;
        onAir = false;
    }

    public void ToggleTricks()
    {
        if (onAir)
        {
            EndTricks();
        }
        else
        {
            StartTricks();
        }
    }

    public void Update()
    {
        if (onAir)
        {
            if (currentTrick == TrickMove.NONE)
            {
                if (Time.time >= nextTrickEnd)
                {
                    currentTrick = (TrickMove) Random.Range(1, 5);
                    nextTrickEnd = Time.time + trickDuration;
                }
            }
            else
            {
                if (Time.time < nextTrickEnd)
                {
                    if (currentTrick == TrickMove.UP
                        && Keyboard.current.wKey.wasPressedThisFrame
                        && !Keyboard.current.sKey.wasPressedThisFrame
                        && !Keyboard.current.aKey.wasPressedThisFrame
                        && !Keyboard.current.dKey.wasPressedThisFrame)
                    {
                        TrickAnimationPlayer.PlayTrick(currentTrick);
                        currentTrick = TrickMove.NONE;
                        nextTrickEnd = Time.time + trickDuration;
                    }
                    else if (currentTrick == TrickMove.DOWN
                        && !Keyboard.current.wKey.wasPressedThisFrame
                        && Keyboard.current.sKey.wasPressedThisFrame
                        && !Keyboard.current.aKey.wasPressedThisFrame
                        && !Keyboard.current.dKey.wasPressedThisFrame)
                    {
                        TrickAnimationPlayer.PlayTrick(currentTrick);
                        currentTrick = TrickMove.NONE;
                        nextTrickEnd = Time.time + trickDuration;
                    }
                    else if (currentTrick == TrickMove.LEFT
                        && !Keyboard.current.wKey.wasPressedThisFrame
                        && !Keyboard.current.sKey.wasPressedThisFrame
                        && Keyboard.current.aKey.wasPressedThisFrame
                        && !Keyboard.current.dKey.wasPressedThisFrame)
                    {
                        TrickAnimationPlayer.PlayTrick(currentTrick);
                        currentTrick = TrickMove.NONE;
                        nextTrickEnd = Time.time + trickDuration;
                    }
                    else if (currentTrick == TrickMove.RIGHT
                        && !Keyboard.current.wKey.wasPressedThisFrame
                        && !Keyboard.current.sKey.wasPressedThisFrame
                        && !Keyboard.current.aKey.wasPressedThisFrame
                        && Keyboard.current.dKey.wasPressedThisFrame)
                    {
                        TrickAnimationPlayer.PlayTrick(currentTrick);
                        currentTrick = TrickMove.NONE;
                        nextTrickEnd = Time.time + trickDuration;
                    }
                    else if (Keyboard.current.wKey.wasPressedThisFrame
                        || Keyboard.current.sKey.wasPressedThisFrame
                        || Keyboard.current.aKey.wasPressedThisFrame
                        || Keyboard.current.dKey.wasPressedThisFrame)
                    {
                        currentTrick = TrickMove.NONE;
                        nextTrickEnd = Time.time + trickDuration;
                    }
                }
            }
        }
    }

    public TrickMove GetCurrentTrick()
    {
        return currentTrick;
    }
}
