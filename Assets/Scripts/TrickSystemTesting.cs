using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TrickSystemTesting : MonoBehaviour
{
    private TrickHandler trickHandler;
    private Text text;
    void Start()
    {
        trickHandler = GetComponent<TrickHandler>();
        text = GetComponent<Text>();
        trickHandler.GenerateTrickMoves();
    }

    void Update()
    {
        if (trickHandler.GetTrickState() == TrickHandler.TrickState.IN_PROCESS)
        {
            switch (trickHandler.GetCurrentMove())
            {
                case (TrickHandler.TrickMove.UP):
                    text.text = "UP";
                    break;
                case (TrickHandler.TrickMove.DOWN):
                    text.text = "DOWN";
                    break;
                case (TrickHandler.TrickMove.LEFT):
                    text.text = "LEFT";
                    break;
                case (TrickHandler.TrickMove.RIGHT):
                    text.text = "RIGHT";
                    break;
            }
        }
        else if (trickHandler.GetTrickState() == TrickHandler.TrickState.FAILED)
        {
            text.text = "FAILED";
        }
        else if (trickHandler.GetTrickState() == TrickHandler.TrickState.SUCCESS)
        {
            text.text = "SUCCESS";
        }

        print(trickHandler.TimeLeft());
    }
}
