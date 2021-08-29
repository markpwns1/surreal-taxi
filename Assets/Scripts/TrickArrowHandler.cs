using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TrickArrowHandler : MonoBehaviour
{
    private TrickHandler trickHandler;
    private Image image;
    void Start()
    {
        Reset();
    }

    public void Reset()
    {
        trickHandler = GetComponent<TrickHandler>();
        image = GetComponent<Image>();
        trickHandler.GenerateTrickMoves();
    }
    void Update()
    {
        if (trickHandler.GetTrickState() == TrickHandler.TrickState.IN_PROCESS)
        {
            image.enabled = true;
            switch (trickHandler.GetCurrentMove())
            {
                case (TrickHandler.TrickMove.UP):
                    image.transform.rotation = Quaternion.Euler(0, 0, 90);
                    break;
                case (TrickHandler.TrickMove.DOWN):
                    image.transform.rotation = Quaternion.Euler(0, 0, 270);
                    break;
                case (TrickHandler.TrickMove.LEFT):
                    image.transform.rotation = Quaternion.Euler(0, 0, 180);
                    break;
                case (TrickHandler.TrickMove.RIGHT):
                    image.transform.rotation = Quaternion.Euler(0, 0, 0);
                    break;
            }
        }
        else if (trickHandler.GetTrickState() == TrickHandler.TrickState.FAILED || trickHandler.GetTrickState() == TrickHandler.TrickState.SUCCESS || trickHandler.GetTrickState() == TrickHandler.TrickState.NONE)
        {
            image.enabled = false;
        }

        print(trickHandler.TimeLeft());
    }
}
