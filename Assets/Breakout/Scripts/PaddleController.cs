using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaddleController : MonoBehaviour {

    public float accel = 10;

    void Update()
    {
        float x = Input.GetAxis("Horizontal") * accel;
        transform.position += Vector3.right * x * Time.deltaTime;
    }
}
    