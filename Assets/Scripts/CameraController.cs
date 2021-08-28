using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float lerpMod;
    public Transform follow, lookAt;

    private void FixedUpdate()
    {
    }

    private void LateUpdate()
    {
        transform.position = Vector3.Lerp(transform.position,
            follow.position, lerpMod);
        transform.LookAt(lookAt);
    }
}
