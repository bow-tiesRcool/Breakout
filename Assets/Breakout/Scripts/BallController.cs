using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour {
    public float speed = 1;
    void Start ()
    {
        Rigidbody body = GetComponent<Rigidbody>();
        
        body.velocity = Vector3.up * speed;
	}
}
