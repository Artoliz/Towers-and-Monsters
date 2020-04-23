using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float MoveSpeed = 0.5f;
    public float ScrollSpeed = 1000f;

    private void Update()
    {
        transform.position += MoveSpeed * new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        transform.position += ScrollSpeed * new Vector3(0, -Input.GetAxis("Mouse ScrollWheel"), 0);
    }
}
