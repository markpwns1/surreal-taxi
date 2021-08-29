using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Linq;

public class TrickAnimationPlayer : MonoBehaviour
{
    private static Animator[] anim = null;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponentsInChildren<Animator>().Where(x =>
        x.parameterCount > 0).ToArray();
    }

    //// testing
    //private void Update()
    //{
    //    if (Keyboard.current.lKey.wasPressedThisFrame)
    //        PlayTrick(TrickHandler.TrickMove.UP);
    //}

    public static void PlayTrick(TrickHandler.TrickMove move)
    {
        if (anim == null)
            return;

        switch (move)
        {
            case TrickHandler.TrickMove.UP:
                PlayAnimation("front");
                break;
            case TrickHandler.TrickMove.DOWN:
                PlayAnimation("back");
                break;
            case TrickHandler.TrickMove.LEFT:
                PlayAnimation("left");
                break;
            case TrickHandler.TrickMove.RIGHT:
                PlayAnimation("right");
                break;
        }
    }

    private static void PlayAnimation(string s)
    {
        foreach (Animator a in anim)
            a.SetTrigger(s);
    }
}
