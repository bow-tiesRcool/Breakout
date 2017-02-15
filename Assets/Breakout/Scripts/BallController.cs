﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour
{
    public ParticleSystem hitParticles;
    public ParticleSystem paddle;
    public float speed = 1;
    Rigidbody body;
    public AudioSource sound;
    public AudioSource sound2;
    void Start()
    {
        body = GetComponent<Rigidbody>();
        sound = GetComponent<AudioSource>();
        PreLaunch();
    }

    void PreLaunch()
    {
        body.velocity = Vector3.zero;
        transform.SetParent(PaddleController.instance.transform);
        transform.localPosition = Vector3.up;
    }
    void Launch()
    {
        transform.SetParent(null);
        transform.position = PaddleController.instance.transform.position + Vector3.up;
        body.velocity = Vector3.up * speed;
    }

    void Update()
    {
        if (transform.parent == null)
        {
            Vector3 v = body.velocity;
            v = v.normalized * speed;
            body.velocity = v;
            if (Mathf.Abs(v.x) > 2 * Mathf.Abs(v.y))
            {
                v.x *= 0.9f;
                body.velocity = v.normalized * speed;
            }

            DeathCheck();
        }
        else
        {
            if (Input.GetButton("Jump"))
            {
                Launch();
            }
        }
    }
    void DeathCheck()
    {
        Vector3 view = Camera.main.WorldToViewportPoint(transform.position);
        if (view.y < 0)
        {
            GameManager.LostBall();
            if (GameManager.instance.lives > 0)
            {
                PreLaunch();
            }
            else
            {
                gameObject.SetActive(false);
            }
        }
    }

    void OnCollisionEnter(Collision c)
    {
        ShakeController shake = Camera.main.gameObject.GetComponent<ShakeController>();
        shake.Shake();

        ParticleSystem p = (c.gameObject.tag == "Player") ? paddle : hitParticles;
        AudioSource s = (c.gameObject.tag == "Player") ? sound2 : sound;
        
        p.Stop();
        p.transform.position = transform.position;
        p.transform.up = body.velocity;
        p.Play();
        s.Stop();
        s.Play();
    }
}
