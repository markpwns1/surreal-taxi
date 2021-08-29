using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TrickHandler : MonoBehaviour
{
    public enum TrickMove
    {
        UP,
        DOWN,
        LEFT,
        RIGHT
    }

    public enum TrickState
    {
        NONE,
        IN_PROCESS,
        FAILED,
        SUCCESS
    }

    public float trickDuration;
    public int numberOfTricks;

    private List<TrickMove> tricks;
    private float nextTrickEnd;
    private TrickState state = TrickState.NONE;

    void Update()
    {
        if (state == TrickState.IN_PROCESS)
        {
            if (Time.time < nextTrickEnd && tricks.Count > 0)
            {
                if (tricks[0] == TrickMove.UP 
                    && Keyboard.current.wKey.wasPressedThisFrame
                    && !Keyboard.current.sKey.wasPressedThisFrame
                    && !Keyboard.current.aKey.wasPressedThisFrame
                    && !Keyboard.current.dKey.wasPressedThisFrame)
                {
                    tricks.RemoveAt(0);
                    nextTrickEnd = Time.time + trickDuration;
                    state = TrickState.SUCCESS;
                }
                else if (tricks[0] == TrickMove.DOWN
                    && !Keyboard.current.wKey.wasPressedThisFrame
                    && Keyboard.current.sKey.wasPressedThisFrame
                    && !Keyboard.current.aKey.wasPressedThisFrame
                    && !Keyboard.current.dKey.wasPressedThisFrame)
                {
                    tricks.RemoveAt(0);
                    nextTrickEnd = Time.time + trickDuration;
                    state = TrickState.SUCCESS;
                }
                else if (tricks[0] == TrickMove.LEFT
                    && !Keyboard.current.wKey.wasPressedThisFrame
                    && !Keyboard.current.sKey.wasPressedThisFrame
                    && Keyboard.current.aKey.wasPressedThisFrame
                    && !Keyboard.current.dKey.wasPressedThisFrame)
                {
                    tricks.RemoveAt(0);
                    nextTrickEnd = Time.time + trickDuration;
                    state = TrickState.SUCCESS;
                }
                else if (tricks[0] == TrickMove.RIGHT
                    && !Keyboard.current.wKey.wasPressedThisFrame
                    && !Keyboard.current.sKey.wasPressedThisFrame
                    && !Keyboard.current.aKey.wasPressedThisFrame
                    && Keyboard.current.dKey.wasPressedThisFrame)
                {
                    tricks.RemoveAt(0);
                    nextTrickEnd = Time.time + trickDuration;
                    state = TrickState.SUCCESS;
                }
                else if (Keyboard.current.wKey.wasPressedThisFrame
                    || Keyboard.current.sKey.wasPressedThisFrame
                    || Keyboard.current.aKey.wasPressedThisFrame
                    || Keyboard.current.dKey.wasPressedThisFrame)
                {
                    tricks.RemoveAt(0);
                    nextTrickEnd = Time.time + 3 * trickDuration;
                    state = TrickState.FAILED;
                }
            }
            else if (tricks.Count > 0)
            {
                tricks.RemoveAt(0);
                nextTrickEnd = Time.time + 3 * trickDuration;
                state = TrickState.FAILED;
            }
            if (tricks.Count == 0)
            {
                state = TrickState.NONE;
            }
        }
        else if ((state == TrickState.FAILED || state == TrickState.SUCCESS) && Time.time > nextTrickEnd)
        {
            nextTrickEnd = Time.time + trickDuration;
            state = TrickState.IN_PROCESS;
        }
    }

    public void GenerateTrickMoves()
    {
        state = TrickState.IN_PROCESS;
        nextTrickEnd = Time.time + trickDuration;
        tricks = new List<TrickMove>();
        for (int i = 0; i < numberOfTricks; i++)
        {
            tricks.Add((TrickMove) Random.Range(0, 4));
        }
    }

    public TrickState GetTrickState()
    {
        return state;
    }

    public TrickMove GetCurrentMove()
    {
        if (state == TrickState.IN_PROCESS)
        {
            return tricks[0];
        }
        return 0;
    }

    public float TimeLeft()
    {
        return nextTrickEnd - Time.time;
    }
}
