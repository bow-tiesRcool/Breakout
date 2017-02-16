using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour
{
    //public ParticleSystem hitParticles;
    public ParticleSystem paddle;
    public ParticleSystem hitParticlesPrefab;
    List<ParticleSystem> particlePool = new List<ParticleSystem>();
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

        ParticleSystem hitParticles = null;
        for (int i = 0; i < particlePool.Count; i++)
        {
            ParticleSystem p = particlePool[i];
            if (p.isStopped)
            {
                hitParticles = p;
                Debug.Log("reusing from my pool");
                break;
            }
        }

        if (hitParticles == null)
        {
            hitParticles = Instantiate(hitParticlesPrefab) as ParticleSystem;
            particlePool.Add(hitParticles);
        } 
        
        ParticleSystem a = (c.gameObject.tag == "Player") ? paddle : hitParticles;
        AudioSource s = (c.gameObject.tag == "Player") ? sound2 : sound;

        //hitParticles.Stop();
        //hitParticles.transform.position = transform.position;
        //hitParticles.transform.up = body.velocity;
        //hitParticles.Play();

        a.Stop();
        a.transform.position = transform.position;
        a.transform.up = body.velocity;
        a.Play();
        s.Stop();
        s.Play();
    }
}
