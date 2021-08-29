using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TrickUI : MonoBehaviour
{
    public GameObject trickUIPanel;
    public Color[] arrowColours;

    public RectTransform arrow;
    public RawImage arrowImage;

    public RectTransform loadingBar;

    private TrickHandler trickHandler;
    public void Init(TrickHandler trickHandler)
    {
        this.trickHandler = trickHandler;
    }

    public void OnTrickChange()
    {
        var trick = trickHandler.GetCurrentTrick();
        float angle;
        switch(trick)
        {
            case TrickHandler.TrickMove.UP: angle = 90; break;
            case TrickHandler.TrickMove.RIGHT: angle = 0; break;
            case TrickHandler.TrickMove.DOWN: angle = 270; break;
            case TrickHandler.TrickMove.LEFT: angle = 180; break;
            default:
                {
                    trickUIPanel.SetActive(false);
                    return;
                }
        }

        trickUIPanel.SetActive(true);
        arrow.localEulerAngles = new Vector3(0, 0, angle);
        arrowImage.color = arrowColours[Random.Range(0, arrowColours.Length)];
    }

    void Update()
    {
        if(trickHandler.GetCurrentTrick() != TrickHandler.TrickMove.NONE)
        {
            loadingBar.sizeDelta = new Vector2(40f * (1.0f - (Time.time - trickHandler.time) / trickHandler.inputWindow), 5);
        }
    }

}
