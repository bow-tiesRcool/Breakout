using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour
{
    //public ParticleSystem hitParticles;
    public ParticleSystem paddle;
    public ParticleSystem hitParticlesPrefab;
    public ParticleSystem lifeLost;
    List<ParticleSystem> particlePool = new List<ParticleSystem>();
    public float speed = 1;
    Rigidbody body;
    public AudioSource sound;
    public AudioClip hitPaddle;
    public AudioClip hitWall;
    public AudioClip hitBrick;
    public AudioClip lostLife;
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
            transform.up = v;
            transform.localScale = new Vector3(0.9f, 1.1f, 1);

            DeathCheck();
        }
        else
        {
            transform.localScale = Vector3.one;
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
                sound.clip = lostLife;
                sound.Play();
                lifeLost.Play();

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
        ContactPoint cp = c.contacts[0];
        transform.up = cp.normal;
        transform.localScale = new Vector3(1.5f, 0.5f, 1);

        ShakeController shake = Camera.main.gameObject.GetComponent<ShakeController>();
        shake.Shake();

        if (c.gameObject.tag == "Player")
        {
            sound.clip = hitPaddle;
            sound.Play();
        }
        else if (c.gameObject.tag == "Brick")
        {
            sound.clip = hitBrick;
            sound.Play();
        }
        else
        {
            sound.clip = hitWall;
            sound.Play();
        }
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
        
        //hitParticles.Stop();
        //hitParticles.transform.position = transform.position;
        //hitParticles.transform.up = body.velocity;
        //hitParticles.Play();

        a.Stop();
        a.transform.position = transform.position;
        a.transform.up = body.velocity;
        a.Play();
        
    }
}
