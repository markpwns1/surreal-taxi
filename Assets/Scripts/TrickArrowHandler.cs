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
        image = GetComponent<Image>();
        trickHandler = GameObject.FindObjectOfType<TrickHandler>();
    }

    void Update()
    {
        if (trickHandler == null)
        {
            trickHandler = GameObject.FindObjectOfType<TrickHandler>();
        }
        else
        {
            image.enabled = true;
            switch (trickHandler.GetCurrentTrick())
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
                case (TrickHandler.TrickMove.NONE):
                    image.enabled = false;
                    break;
            }
        }

    }
}
