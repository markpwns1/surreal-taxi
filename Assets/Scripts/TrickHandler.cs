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

    private bool up, down, left, right;

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
        UpdateControls();
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
                        && up
                        && !down
                        && !left
                        && !right)
                    {
                        TrickAnimationPlayer.PlayTrick(currentTrick);
                        currentTrick = TrickMove.NONE;
                        nextTrickEnd = Time.time + trickDuration;
                    }
                    else if (currentTrick == TrickMove.DOWN
                        && !up
                        && down
                        && !left
                        && !right)
                    {
                        TrickAnimationPlayer.PlayTrick(currentTrick);
                        currentTrick = TrickMove.NONE;
                        nextTrickEnd = Time.time + trickDuration;
                    }
                    else if (currentTrick == TrickMove.LEFT
                        && !up
                        && !down
                        && left
                        && !right)
                    {
                        TrickAnimationPlayer.PlayTrick(currentTrick);
                        currentTrick = TrickMove.NONE;
                        nextTrickEnd = Time.time + trickDuration;
                    }
                    else if (currentTrick == TrickMove.RIGHT
                        && !up
                        && !down
                        && !left
                        && right)
                    {
                        TrickAnimationPlayer.PlayTrick(currentTrick);
                        currentTrick = TrickMove.NONE;
                        nextTrickEnd = Time.time + trickDuration;
                    }
                    else if (up
                        || down
                        || left
                        || right)
                    {
                        currentTrick = TrickMove.NONE;
                        nextTrickEnd = Time.time + trickDuration;
                    }
                }
            }
        }
    }

    private void UpdateControls()
    {
        up = down = left = right = false;

        up = Keyboard.current.wKey.wasPressedThisFrame
            || Keyboard.current.upArrowKey.wasPressedThisFrame;

        down = Keyboard.current.sKey.wasPressedThisFrame
            || Keyboard.current.downArrowKey.wasPressedThisFrame;

        left = Keyboard.current.aKey.wasPressedThisFrame
            || Keyboard.current.leftArrowKey.wasPressedThisFrame;

        right = Keyboard.current.dKey.wasPressedThisFrame
            || Keyboard.current.rightArrowKey.wasPressedThisFrame;

        Gamepad g = Gamepad.current;
        if(g != null)
        {
            up |= Gamepad.current.leftStick.up.wasPressedThisFrame;
            down |= Gamepad.current.leftStick.down.wasPressedThisFrame;
            left |= Gamepad.current.leftStick.left.wasPressedThisFrame;
            right |= Gamepad.current.leftStick.right.wasPressedThisFrame;
        }
    }

    public TrickMove GetCurrentTrick()
    {
        return currentTrick;
    }
}
