using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TrickHandler : MonoBehaviour
{
    enum TrickMove
    {
        UP,
        DOWN,
        LEFT,
        RIGHT
    }

    public float trickDuration;

    private List<TrickMove> tricks;
    private float nextTrickEnd;
    private bool failed = true;

    void Start()
    {
        GenerateTrickMoves();
        PrintState();
    }

    void Update()
    {
        if (!failed)
        {
            if (Time.time < nextTrickEnd)
            {
                if (tricks[0] == TrickMove.UP && Keyboard.current["W"].IsPressed())
                {
                    tricks.RemoveAt(0);
                    nextTrickEnd = Time.time + trickDuration;
                    PrintState();
                }
                else if (tricks[0] == TrickMove.DOWN && Keyboard.current["S"].IsPressed())
                {
                    tricks.RemoveAt(0);
                    nextTrickEnd = Time.time + trickDuration;
                    PrintState();
                }
                else if (tricks[0] == TrickMove.LEFT && Keyboard.current["A"].IsPressed())
                {
                    tricks.RemoveAt(0);
                    nextTrickEnd = Time.time + trickDuration;
                    PrintState();
                }
                else if (tricks[0] == TrickMove.RIGHT && Keyboard.current["D"].IsPressed())
                {
                    tricks.RemoveAt(0);
                    nextTrickEnd = Time.time + trickDuration;
                    PrintState();
                }
            }
            else if (tricks.Count > 0)
            {
                failed = true;
                PrintState();
            }
        }
    }

    void GenerateTrickMoves()
    {
        failed = false;
        nextTrickEnd = Time.time + trickDuration;
        tricks = new List<TrickMove>();
        for (int i = 0; i < 7; i++)
        {
            tricks.Add((TrickMove) Random.Range(0, 4));
        }
    }

    void PrintState()
    {
        if (!failed)
        {
            if (tricks.Count > 0)
            {
                switch (tricks[0])
                {
                    case (TrickMove.UP):
                        print("UP");
                        break;
                    case (TrickMove.DOWN):
                        print("DOWN");
                        break;
                    case (TrickMove.LEFT):
                        print("LEFT");
                        break;
                    case (TrickMove.RIGHT):
                        print("RIGHT");
                        break;
                }
            }
            else
            {
                print("SUCCESS");
                failed = true;
            }
        }
        else
        {
            print("FAILED");
        }
    }
}
