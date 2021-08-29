using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MainMenu : MonoBehaviour
{
    CameraController cam;
    public Transform focalPoint;
    public Transform followPoint;
    public Transform mainMenuHolder;

    public GameObject[] toEnable;
    public GameObject[] toDisable;

    public static GameObject playerInstance;
    
    // Start is called before the first frame update
    void Start()
    {
        cam = GameObject.FindObjectOfType<CameraController>();

        foreach (var item in toEnable)
        {
            item.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        cam.Follow(followPoint.position, focalPoint.position);
        var p = mainMenuHolder.position;
        p.y = 20f + Mathf.Sin(Time.time * 0.3f);
        mainMenuHolder.position = p;

        if(Keyboard.current.spaceKey.wasReleasedThisFrame || (Gamepad.current != null && Gamepad.current.startButton.wasReleasedThisFrame))
        {
            foreach (var item in toDisable)
            {
                item.SetActive(false);
            }

            foreach (var item in toEnable)
            {
                item.SetActive(true);
            }

            playerInstance.SetActive(true);
            this.enabled = false;
        }
    }
}
