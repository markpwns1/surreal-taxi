using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrickAnimationPlayer : MonoBehaviour
{
    private static Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponentInChildren<Animator>();
    }

    public static void PlayTrick(TrickHandler.TrickMove move)
    {
        if (!anim)
            return;
        switch (move)
        {
            case TrickHandler.TrickMove.UP:
                anim.SetTrigger("front");
                break;
            case TrickHandler.TrickMove.DOWN:
                anim.SetTrigger("back");
                break;
            case TrickHandler.TrickMove.LEFT:
                anim.SetTrigger("left");
                break;
            case TrickHandler.TrickMove.RIGHT:
                anim.SetTrigger("right");
                break;
        }
    }
}
