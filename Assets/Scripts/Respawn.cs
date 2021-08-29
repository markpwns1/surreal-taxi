using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        if(transform.position.y < respawnY)
        {
            GetComponent<Rigidbody>().velocity = Vector3.zero;
            if(map.MapRaycast(out RaycastHit hit, true, true))
            {
                transform.position = hit.point + Vector3.up * 10f;
                //maybe fix camera or somthing idk
            }
        }
    }
}
