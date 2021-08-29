using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraTargetController : MonoBehaviour
{
    public float followLerp, centreLerp;
    public Vector2 mouseSensitivity, gamePadSensitivity;

    private Rigidbody rb;
    private Transform targetTransform;
    public CameraController cam;
    private Vector3 target;

    private float rotX, rotY;

    // Start is called before the first frame update
    void Start()
    {
        targetTransform = transform.GetChild(0);
        cam = GameObject.FindObjectOfType<CameraController>();
        target = targetTransform.position;

        rb = transform.parent.parent.GetComponentInChildren<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked;
    }

    void FixedUpdate()
    {
        MoveAndRotate();
        cam.Follow(target, transform.position);
    }


    private void MoveAndRotate()
    {
        Vector2 look = Vector2.zero;
        Gamepad g = Gamepad.current;
        if(g != null)
        {
            Vector2 gD = g.rightStick.ReadValue();
            look.x = gD.x * gamePadSensitivity.x;
            look.y = gD.y * gamePadSensitivity.y;
        }
        Vector2 mouseD = Mouse.current.delta.ReadValue();
        look.x += mouseD.x * mouseSensitivity.x;
        look.y -= mouseD.y * mouseSensitivity.y;

        rotX = Mathf.Lerp(rotX + look.x, 0, centreLerp * rb.velocity.magnitude);
        rotY = Mathf.Lerp(rotY + look.y, 0, centreLerp * rb.velocity.magnitude);
        rotY = Mathf.Clamp(rotY, -25, 70);
        if (rotX > 180)
            rotX -= 360;
        else if (rotX < -180)
            rotX += 360;

        transform.localEulerAngles = Vector3.up * rotX + Vector3.right * rotY;

        target = Vector3.Lerp(target, targetTransform.position, followLerp);
    }
}
