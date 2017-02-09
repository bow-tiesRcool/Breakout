using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour {
    public float speed = 1;
    Rigidbody body;
    void Start ()
    {
        body = GetComponent<Rigidbody>();
        Launch();
	}

    void Launch()
    {
        transform.position = PaddleController.instance.transform.position + Vector3.up;
        body.velocity = Vector3.up * speed;
    }

    void Update()
    {
        Vector3 v = body.velocity;
        if (Mathf.Abs(v.x) > Mathf.Abs(v.y))
        {
            v.x *= 0.9f;
            body.velocity = v.normalized * speed;
        }

        Vector3 view = Camera.main.WorldToViewportPoint(transform.position);
        if (view.y < 0)
        {
            GameManager.LostBall();
            Launch();
        }
    }
}
