using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CarController : MonoBehaviour
{
    public float accelaration, friction;

    private Rigidbody rb;
    private GameObject mesh;

    private float forward, left, right;

    // Start is called before the first frame update
    void Start()
    {
        mesh = GetComponentInChildren<MeshRenderer>().gameObject;
        rb = GetComponent<Rigidbody>();

    }

    // Update is called once per frame
    void Update()
    {
        Gamepad g = Gamepad.current;
        if (g != null)
        {
            forward = g.leftStick.y.ReadValue();
            print(g.rightTrigger.ReadValue());
        }
        var keyboard = Keyboard.current;
        forward += keyboard.wKey.isPressed ? 1 : 0;
        forward -= keyboard.sKey.isPressed ? 1 : 0;
        left += keyboard.lKey.IsPressed ? 1 : 0;
        right += keyboard.rKey.IsPressed ? 1 : 0;
    }
}
