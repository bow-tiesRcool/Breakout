using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaddleController : MonoBehaviour
{

    
    public static PaddleController instance;

    public float speed = 10;
    public float tilt = 5;
    Renderer renderer;
    public string triggerName = "PowerUp";
    int powerupUse = 0;

    public Animator animator;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    void Start()
    {
        renderer = GetComponentInChildren<Renderer>();
    } 
    void Update()
    {
        float x = Input.GetAxis("Horizontal");
        transform.position += Vector3.right * speed * x * Time.deltaTime;
        transform.localEulerAngles = Vector3.back * tilt * x;

        ClampToScreen(renderer.bounds.extents.x);
        ClampToScreen(-renderer.bounds.extents.x);

    }
    void ClampToScreen (float xOffset)
    {
        Vector3 v = Camera.main.WorldToViewportPoint(transform.position + Vector3.right * xOffset);
        v.x = Mathf.Clamp01(v.x);
        transform.position = Camera.main.ViewportToWorldPoint(v) - Vector3.right * xOffset;
    }

    void OnTriggerEnter(Collider c)
    {
        if (c.gameObject.tag == "PowerUp")
        {
            Debug.Log("Power up aquired!");
            StartCoroutine("PowerUpTimerPos");
        }
        else if (c.gameObject.tag == "NegativePowerUp")
        {
            Debug.Log("Negative power up aquired!");
            StartCoroutine("PowerUpTimerNeg");
        }
        else if (c.gameObject.tag == "LifeUp")
        {
            Debug.Log("Life added");
            GameManager.AddLife();
        }
    }
    IEnumerator PowerUpTimerPos()
    {
        Debug.Log("PowerUp is working");
        if (powerupUse == 0)
        {
            powerupUse++;
            transform.localScale += new Vector3(0.5f, 0, 0);
            yield return new WaitForSeconds(10);
            transform.localScale -= new Vector3(0.5f, 0, 0);
            Debug.Log("Powerup Ended");
            powerupUse--;
            
        }
        else
        {
            yield return null;
        }
    }
    IEnumerator PowerUpTimerNeg()
    {
        Debug.Log("PowerUp is working");
        if (powerupUse == 0)
        {
            powerupUse++;
            transform.localScale -= new Vector3(0.5f, 0, 0);
            yield return new WaitForSeconds(10);
            transform.localScale += new Vector3(0.5f, 0, 0);
            Debug.Log("Powerup Ended");
            powerupUse--;
        }
        else
        {
            yield return null;
        }
    }
}


