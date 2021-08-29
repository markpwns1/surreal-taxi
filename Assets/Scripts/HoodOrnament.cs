using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoodOrnament : MonoBehaviour
{
    private static GameObject instance;

    private void Start()
    {
        instance = gameObject;
        instance.SetActive(false);
    }

    public static void ActivateOrnament(bool b)
    {
        instance.SetActive(b);
    }
}
