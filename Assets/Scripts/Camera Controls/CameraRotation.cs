using System;
using UnityEngine;

public class CameraRotation : MonoBehaviour {
    [Range(0.3f, 1)]
    public float speed;
    void FixedUpdate()
    {
        transform.Rotate(0, speed, 0);
    }
}
