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

    public float nextTrickDelay, failureDelay, inputWindow;
    public AudioSource successSound;

    private TrickMove currentTrick;
    public float time, stuntPayout;
    private bool onAir = false, failed = false;

    private bool up, down, left, right;

    private TrickUI ui;

    void Start()
    {
        ui = FindObjectOfType<TrickUI>();
        ui.Init(this);
    }

    public void StartTricks()
    {
        currentTrick = TrickMove.NONE;
        NotifyUIOfTrickChange();
        time = 0;
        onAir = true;
    }

    public void EndTricks()
    {
        currentTrick = TrickMove.NONE;
        onAir = false;
        NotifyUIOfTrickChange();
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

    private void NotifyUIOfTrickChange()
    {
        if (ui)
            ui.OnTrickChange();
        else
        {
            ui = FindObjectOfType<TrickUI>();
            ui.Init(this);
            if (ui)
                ui.OnTrickChange();
        }
    }

    public void Update()
    {
        UpdateControls();
        if (onAir)
        {
            if (currentTrick == TrickMove.NONE)
            {
                if (failed)
                {
                    if (Time.time > time + failureDelay)
                    {
                        currentTrick = (TrickMove)Random.Range(1, 5);
                        time = Time.time;
                        failed = false;
                        NotifyUIOfTrickChange();
                    }
                }
                else
                {
                    if (Time.time > time + nextTrickDelay)
                    {
                        currentTrick = (TrickMove)Random.Range(1, 5);
                        time = Time.time;
                        NotifyUIOfTrickChange();
                    }
                }
            }
            else
            {
                if (Time.time < time + inputWindow)
                {
                    if (currentTrick == TrickMove.UP
                        && up
                        && !down
                        && !left
                        && !right)
                    {
                        TrickAnimationPlayer.PlayTrick(currentTrick);
                        currentTrick = TrickMove.NONE;
                        time = Time.time;
                        successSound.Play();
                        NotifyUIOfTrickChange();
                        ScoreScript.OnStunt(stuntPayout);
                    }
                    else if (currentTrick == TrickMove.DOWN
                        && !up
                        && down
                        && !left
                        && !right)
                    {
                        TrickAnimationPlayer.PlayTrick(currentTrick);
                        currentTrick = TrickMove.NONE;
                        time = Time.time;
                        successSound.Play();
                        NotifyUIOfTrickChange();
                        ScoreScript.OnStunt(stuntPayout);
                    }
                    else if (currentTrick == TrickMove.LEFT
                        && !up
                        && !down
                        && left
                        && !right)
                    {
                        TrickAnimationPlayer.PlayTrick(currentTrick);
                        currentTrick = TrickMove.NONE;
                        time = Time.time;
                        successSound.Play();
                        NotifyUIOfTrickChange();
                        ScoreScript.OnStunt(stuntPayout);
                    }
                    else if (currentTrick == TrickMove.RIGHT
                        && !up
                        && !down
                        && !left
                        && right)
                    {
                        TrickAnimationPlayer.PlayTrick(currentTrick);
                        currentTrick = TrickMove.NONE;
                        time = Time.time;
                        successSound.Play();
                        NotifyUIOfTrickChange();
                        ScoreScript.OnStunt(stuntPayout);
                    }
                    else if (up
                        || down
                        || left
                        || right)
                    {
                        currentTrick = TrickMove.NONE;
                        time = Time.time;
                        failed = true;
                        NotifyUIOfTrickChange();
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
