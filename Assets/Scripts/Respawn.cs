using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Respawn : MonoBehaviour
{
    public float respawnY = -50;
    private ProceduralGenerator map;

    void Start()
    {
        map = GameObject.FindObjectOfType<ProceduralGenerator>();
    }
    void Update()
    {
        if(Keyboard.current.rKey.wasReleasedThisFrame || (Gamepad.current != null && Gamepad.current.selectButton.wasPressedThisFrame) || transform.position.y < respawnY)
        {
            //Debug.Log("B " + Time.time);
            bool v = false;
            RaycastHit hit;
            do
            {
                v = map.MapRaycast(out hit, true, true);
            }
            while (!v);

            {
                GetComponent<Rigidbody>().velocity = Vector3.zero;
                //Debug.Log("A");
                transform.position = hit.point + Vector3.up * 10f;
                //maybe fix camera or somthing idk
            }
        }
    }


}
